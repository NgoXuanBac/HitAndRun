using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using System.Collections.Concurrent;

namespace HitAndRun.Character
{
    public class MbTeam : MonoBehaviour
    {
        [Header("Line up")]
        [SerializeField, Range(0, 1)] private float _offset = 0.4f;
        [SerializeField, Range(0, 1)] private float _delay = 0.2f;
        [SerializeField] private List<MbCharacter> _row = new();
        private ConcurrentQueue<Func<UniTask>> _actions = new();
        [SerializeField] private Transform _follow;
        private CancellationToken _ctk;
        private bool _needRearrange = false;
        private void Reset()
        {
            _follow = transform.Find("Follow");
        }
        private void Start()
        {
            var character = MbCharacterSpawner.Instance.Spawn(transform.position, transform);
            AddToRow(character);
            _ctk = this.GetCancellationTokenOnDestroy();
            ProcessActionsAsync();
        }

        private async void ProcessActionsAsync()
        {
            while (!_ctk.IsCancellationRequested && _row.Count > 0)
            {
                await UniTask.WaitUntil(() => _actions.Count > 0 || _needRearrange);
                if (_actions.TryDequeue(out var action)) await action();

                if (_needRearrange)
                {
                    _needRearrange = false;
                    await RearrangeCharactersAsync();
                }
            }
        }

        private void AddToRow(MbCharacter character, int index = 0)
        {
            character.Grabber.OnGrab += HandleInsertCharacter;
            character.OnDead += HandleRemoveCharacter;
            _row.Insert(index, character);
        }

        private void RemoveFromRow(MbCharacter character)
        {
            character.Grabber.OnGrab -= HandleInsertCharacter;
            character.OnDead -= HandleRemoveCharacter;
            _row.Remove(character);
        }

        private void HandleInsertCharacter(MbCharacter collider, MbCharacter other, int offset)
        {
            other.tag = MbCharacter.ACTIVE_TAG;
            var current = _row.IndexOf(collider);
            int index;
            if (offset > 0 && current == _row.Count - 1) index = -1;
            else index = _row.IndexOf(collider) + offset;

            _actions.Enqueue(() => ProcessInsertCharacterAtAsync(other, index));
        }

        private void HandleRemoveCharacter(MbCharacter dead)
        {
            RemoveFromRow(dead);
            _needRearrange = true;
        }

        private async UniTask MergeCharacterAsync(int one, int two)
        {
            var center = _follow.localPosition.x;
            if (Mathf.Abs(_row[one].transform.localPosition.x - center)
            < Mathf.Abs(_row[two].transform.localPosition.x - center))
                (two, one) = (one, two);

            _row[two].Body.Level *= 2;

            var target = _row[two].transform.localPosition;
            await _row[one].transform
                .DOLocalMove(target, _delay)
                .SetEase(Ease.OutQuad)
                .WithCancellation(_ctk);

            var character = _row[one];
            RemoveFromRow(character);
            MbCharacterSpawner.Instance.Despawn(character);
        }

        private async UniTask RearrangeCharactersAsync()
        {
            if (_row.Count == 0) return;

            var center = 0;
            var min = Mathf.Abs(_row[0].transform.localPosition.x - _follow.localPosition.x);
            for (int i = 1; i < _row.Count; i++)
            {
                var distance = Mathf.Abs(_row[i].transform.localPosition.x - _follow.localPosition.x);
                if (distance < min)
                {
                    min = distance;
                    center = i;
                }
            }

            var positions = new float[_row.Count];
            positions[center] = _row[center].transform.localPosition.x;

            for (int i = center + 1; i < _row.Count; i++)
                positions[i] = positions[i - 1] + _row[i - 1].Body.Width + _offset;

            for (int i = center - 1; i >= 0; i--)
                positions[i] = positions[i + 1] - _row[i].Body.Width - _offset;

            var tasks = _row.Select((character, i) =>
            {
                if (!Mathf.Approximately(character.transform.localPosition.x, positions[i]))
                {
                    return character.transform
                        .DOLocalMove(positions[i] * Vector3.right, _delay)
                        .SetEase(Ease.OutQuad)
                        .WithCancellation(_ctk);
                }
                return UniTask.CompletedTask;
            });

            await UniTask.WhenAll(tasks);
            RecalculateFollow();
        }

        private async UniTask ProcessInsertCharacterAtAsync(MbCharacter character, int index)
        {
            index = Mathf.Clamp(index, -1, _row.Count);
            index = (index == -1) ? _row.Count : index;

            character.transform.SetParent(transform);
            var tasks = new List<UniTask>();
            var insert = (index == _row.Count ? _row[^1] : _row[index]).transform.localPosition.x;
            var gap = _offset + character.Body.Width * 0.5f;

            if (index == 0) insert -= gap + _row[index].Body.Width * 0.5f;
            else if (index == _row.Count) insert += gap + _row[index - 1].Body.Width * 0.5f;
            else
            {
                insert -= 0.5f * (gap + _row[index].Body.Width * 0.5f);
                for (int i = 0; i < _row.Count; i++)
                {
                    var offset = Mathf.Sign(i - index) * 0.5f * (gap + _row[i].Body.Width * 0.5f);
                    tasks.Add(_row[i].transform
                        .DOLocalMove((_row[i].transform.localPosition.x + offset) * Vector3.right, _delay)
                        .SetEase(Ease.OutQuad)
                        .WithCancellation(_ctk));
                }
            }

            tasks.Add(character.transform
                .DOLocalMove(insert * Vector3.right, _delay)
                .SetEase(Ease.OutQuad)
                .WithCancellation(_ctk));

            await UniTask.WhenAll(tasks);
            AddToRow(character, index);

            bool merged, needArrange = false;
            do
            {
                merged = false;
                if (index > 0 && _row[index - 1].Body.Level == _row[index].Body.Level)
                {
                    await MergeCharacterAsync(index, index - 1);
                    index--;
                    merged = true;
                }
                else if (index < _row.Count - 1 && _row[index + 1].Body.Level == _row[index].Body.Level)
                {
                    await MergeCharacterAsync(index, index + 1);
                    merged = true;
                }
                if (merged && !needArrange) needArrange = true;

            } while (merged);
            if (needArrange) _needRearrange = true;
            else RecalculateFollow();
        }

        private void RecalculateFollow()
        {
            if (_row.Count == 0) return;
            var sum = 0f;
            for (int i = 0; i < _row.Count; i++) sum += _row[i].transform.localPosition.x;
            _follow.localPosition = new Vector3(sum / _row.Count, _follow.localPosition.y, _follow.localPosition.z);
        }

#if UNITY_EDITOR

        public void AddRandom()
        {
            var c1 = MbCharacterSpawner.Instance.Spawn(transform.position + new Vector3(0, 0, 8f), transform);
            _actions.Enqueue(() => ProcessInsertCharacterAtAsync(c1, UnityEngine.Random.Range(0, _row.Count + 1)));
        }

        public void RemoveRandom()
        {
            var dead = _row[UnityEngine.Random.Range(0, _row.Count)];
            HandleRemoveCharacter(dead);
            MbCharacterSpawner.Instance.Despawn(dead);
        }
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MbTeam))]
    public class ETeamInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            var team = (MbTeam)target;
            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button("Add Random"))
            {
                team.AddRandom();
            }
            if (GUILayout.Button("Remove Random"))
            {
                team.RemoveRandom();
            }
            GUI.enabled = true;
            EditorGUILayout.Space();
            DrawDefaultInspector();
        }
    }
#endif

}