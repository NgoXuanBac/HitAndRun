using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace HitAndRun.Character
{
    public class MbTeam : MonoBehaviour
    {
        [SerializeField, Range(0, 1)] private float _gap = 0.4f;
        [SerializeField, Range(0, 1)] private float _animationDelay = 0.2f;
        [SerializeField, Range(0, 1)] private float _batchDelay = 0.2f;
        [SerializeField] private Transform _follow;
        private BatchJobQueue<Addon> _addons;
        [SerializeField] private List<MbCharacter> _row = new();
        private volatile bool _needRearrange = false;

        private void Reset()
        {
            _follow = transform.Find("Follow");
        }

        private void Start()
        {
            _addons = new BatchJobQueue<Addon>(TimeSpan.FromSeconds(_batchDelay), 3, AddCharacters);
            var character = MbCharacterSpawner.Instance.Spawn(transform.position, transform);
            Insert(0, character);
            ProcessRearrangeAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTaskVoid ProcessRearrangeAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await UniTask.WaitUntilValueChanged(this, x => x._needRearrange,
                    cancellationToken: cancellationToken);
                if (_needRearrange)
                {
                    _needRearrange = false;
                    await RearrangeAsync(cancellationToken);
                }
            }
        }

        public void AddCharacter()
        {
            var character = MbCharacterSpawner.Instance.Spawn(transform.position + new Vector3(0, 0, 8f), transform);
            _addons.Enqueue(new Addon
            {
                Character = character,
                Index = UnityEngine.Random.Range(0, _row.Count + 1)
            });
        }

        private void Insert(int index, MbCharacter character)
        {
            character.Grabber.OnGrab += CollectCharacter;
            character.OnDead += LeaveTeam;
            _row.Insert(index, character);
        }

        private void Remove(MbCharacter character)
        {
            character.Grabber.OnGrab -= CollectCharacter;
            character.OnDead -= LeaveTeam;
            _row.Remove(character);
        }

        private void CollectCharacter(MbCharacter collider, MbCharacter other, int offset)
        {
            other.tag = MbCharacter.ACTIVE_TAG;
            var current = _row.IndexOf(collider);
            int index;
            if (offset > 0 && current == _row.Count - 1) index = -1;
            else index = _row.IndexOf(collider) + offset;

            _addons.Enqueue(new Addon { Character = other, Index = index });
        }

        private void LeaveTeam(MbCharacter dead)
        {
            Remove(dead);
            _needRearrange = true;
        }

        private async UniTask AddCharacters(List<Addon> addons, CancellationToken cancellationToken)
        {
            await UniTask.WaitUntil(() => !_needRearrange, cancellationToken: cancellationToken);

            if (_row.Count == 0 || addons == null || addons.Count == 0) return;
            var updates = _row.ToDictionary(x => x, x => x.transform.localPosition);
            foreach (var addon in addons)
            {
                var character = addon.Character;

                int index = Mathf.Clamp(addon.Index, -1, _row.Count);
                index = (index == -1) ? _row.Count : index;

                var gap = _gap + character.Body.Width * 0.5f;

                var insert = updates[index == _row.Count ? _row[^1] : _row[index]].x;
                if (index == 0) insert -= gap + _row[index].Body.Width * 0.5f;
                else if (index == _row.Count) insert += gap + _row[index - 1].Body.Width * 0.5f;
                else
                {
                    insert -= 0.5f * (gap + _row[index].Body.Width * 0.5f);
                    for (int i = 0; i < _row.Count; i++)
                    {
                        var offset = Mathf.Sign(i - index) * 0.5f * (gap + _row[i].Body.Width * 0.5f);
                        var position = updates[_row[i]];
                        updates[_row[i]] = (position.x + offset) * Vector3.right;
                    }
                }

                updates[character] = insert * Vector3.right;

                character.transform.SetParent(transform);
                Insert(index, character);
            }

            var tasks = new List<UniTask>();
            foreach (var character in _row)
            {
                if (!updates.ContainsKey(character)) continue;
                tasks.Add(character.transform
                    .DOLocalMove(updates[character], _animationDelay)
                    .SetEase(Ease.OutQuad)
                    .WithCancellation(cancellationToken));
            }

            await UniTask.WhenAll(tasks);


            bool needRearrange = false;
            foreach (var addon in addons)
            {
                int index = _row.IndexOf(addon.Character);
                if (index != -1)
                {
                    needRearrange |= await MergeCharactersAtAsync(index, cancellationToken);
                }
            }

            _needRearrange = needRearrange;
            if (!_needRearrange) CalculateFollowPoint();
        }

        private async UniTask<bool> MergeCharactersAtAsync(int index, CancellationToken cancellationToken)
        {
            bool merged, needArrange = false;
            do
            {
                merged = false;
                if (index > 0 && _row[index - 1].Body.Level == _row[index].Body.Level)
                {
                    await MergeCharacterAsync(index, index - 1, cancellationToken);
                    index--;
                    merged = true;
                }
                else if (index < _row.Count - 1 && _row[index + 1].Body.Level == _row[index].Body.Level)
                {
                    await MergeCharacterAsync(index, index + 1, cancellationToken);
                    merged = true;
                }
                if (merged && !needArrange) needArrange = true;

            } while (merged);
            return needArrange;
        }

        private async UniTask MergeCharacterAsync(int one, int two, CancellationToken cancellationToken)
        {
            var center = _follow.localPosition.x;
            if (Mathf.Abs(_row[one].transform.localPosition.x - center)
            < Mathf.Abs(_row[two].transform.localPosition.x - center))
                (two, one) = (one, two);

            _row[two].Body.Level *= 2;

            var target = _row[two].transform.localPosition;
            await _row[one].transform
                .DOLocalMove(target, _animationDelay)
                .SetEase(Ease.OutQuad)
                .WithCancellation(cancellationToken);

            var character = _row[one];
            Remove(character);
            MbCharacterSpawner.Instance.Despawn(character);
        }

        private async UniTask RearrangeAsync(CancellationToken cancellationToken)
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
                positions[i] = positions[i - 1] + _row[i - 1].Body.Width + _gap;

            for (int i = center - 1; i >= 0; i--)
                positions[i] = positions[i + 1] - _row[i].Body.Width - _gap;

            var tasks = _row.Select((character, i) =>
            {
                if (!Mathf.Approximately(character.transform.localPosition.x, positions[i]))
                {
                    return character.transform
                        .DOLocalMove(positions[i] * Vector3.right, _animationDelay)
                        .SetEase(Ease.OutQuad)
                        .WithCancellation(cancellationToken);
                }
                return UniTask.CompletedTask;
            });

            await UniTask.WhenAll(tasks);
            CalculateFollowPoint();
        }

        private void CalculateFollowPoint()
        {
            var sum = 0f;
            for (int i = 0; i < _row.Count; i++) sum += _row[i].transform.localPosition.x;
            _follow.localPosition = new Vector3(sum / _row.Count, _follow.localPosition.y, _follow.localPosition.z);
        }

        private struct Addon
        {
            public MbCharacter Character;
            public int Index;
        }
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(MbTeam))]
    public class EConcakInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            var concak = (MbTeam)target;
            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button("Add"))
            {
                concak.AddCharacter();
            }
            GUI.enabled = true;
            EditorGUILayout.Space();
            DrawDefaultInspector();
        }
    }
#endif
}

