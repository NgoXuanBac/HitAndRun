using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;

namespace HitAndRun.Character
{
    [CustomEditor(typeof(MbGroup))]
    public class EGroupInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            var group = (MbGroup)target;
            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button("Add Character"))
            {
                group.AddCharacter();
            }
            GUI.enabled = true;
            EditorGUILayout.Space();
            DrawDefaultInspector();
        }
    }

    public class MbGroup : MonoBehaviour
    {
        [Header("Line up")]
        [SerializeField, Range(0, 1)] private float _offset = 0.4f;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private List<MbCharacter> _row = new();
        private Queue<MbCharacter> _pool = new();
        private Queue<(MbCharacter, int)> _insertQueue = new();
        [SerializeField] private Transform _follow;
        private CancellationToken _ctk;
        private void Reset()
        {
            _prefab = Resources.Load<GameObject>("Prefabs/Character");
            _follow = transform.Find("Follow");
        }
        private void Start()
        {
            _row.Add(SpawnCharacter(transform.position));
            _ctk = this.GetCancellationTokenOnDestroy();
            ProcessInsertCharactes();
        }


        private MbCharacter SpawnCharacter(Vector3 position)
        {
            if (_pool.Count == 0)
            {
                var newCharacter = Instantiate(_prefab, transform).GetComponent<MbCharacter>();
                _pool.Enqueue(newCharacter);
            }

            var character = _pool.Dequeue();
            character.transform.position = position;
            character.gameObject.SetActive(true);
            return character;
        }

        private void DespawnCharacter(MbCharacter character)
        {
            character.Reset();
            character.gameObject.SetActive(false);
            _row.Remove(character);
            _pool.Enqueue(character);
        }


        private async void ProcessInsertCharactes()
        {
            while (!_ctk.IsCancellationRequested)
            {
                await UniTask.WaitUntil(() => _insertQueue.Count != 0);
                var (character, index) = _insertQueue.Dequeue();
                await ProcessInsertCharacterAt(character, index);
            }
        }

        private async Task MergeCharacter(int one, int two)
        {
            var center = _follow.localPosition.x;
            if (Mathf.Abs(_row[one].transform.localPosition.x - center)
            < Mathf.Abs(_row[two].transform.localPosition.x - center))
                (two, one) = (one, two);

            _row[two].Body.Level *= 2;

            var target = _row[two].transform.localPosition;
            await _row[one].transform
                .DOLocalMove(target, 0.3f)
                .SetEase(Ease.OutQuad)
                .WithCancellation(_ctk);

            DespawnCharacter(_row[one]);
        }

        private async UniTask RearrangeCharacters()
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
                character.transform
                    .DOLocalMove(positions[i] * Vector3.right, 0.3f)
                    .SetEase(Ease.OutQuad)
                    .WithCancellation(_ctk)
            );

            await UniTask.WhenAll(tasks);
        }

        public void InsertCharacterAt(MbCharacter character, int index)
        {
            _insertQueue.Enqueue((character, index));
        }

        private async UniTask ProcessInsertCharacterAt(MbCharacter character, int index)
        {
            if (index < -1 || index > _row.Count) return;
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
                        .DOLocalMove((_row[i].transform.localPosition.x + offset) * Vector3.right, 0.3f)
                        .SetEase(Ease.OutQuad)
                        .WithCancellation(_ctk));
                }
            }

            tasks.Add(character.transform
                .DOLocalMove(insert * Vector3.right, 0.3f)
                .SetEase(Ease.OutQuad)
                .WithCancellation(_ctk));

            await UniTask.WhenAll(tasks);
            _row.Insert(index, character);

            bool merged, needArrange = false;
            do
            {
                merged = false;
                if (index > 0 && _row[index - 1].Body.Level == _row[index].Body.Level)
                {
                    await MergeCharacter(index, index - 1);
                    index--;
                    merged = true;
                }
                else if (index < _row.Count - 1 && _row[index + 1].Body.Level == _row[index].Body.Level)
                {
                    await MergeCharacter(index + 1, index);
                    merged = true;
                }
                if (merged && !needArrange) needArrange = true;

            } while (merged);
            if (needArrange) await RearrangeCharacters();

            var center = _row.Average(c => c.transform.localPosition.x);
            _follow.localPosition = new Vector3(center, _follow.localPosition.y, _follow.localPosition.z);
        }


        public void AddCharacter()
        {
            var c1 = SpawnCharacter(transform.position + new Vector3(0, 0, 8f));
            InsertCharacterAt(c1, -1);
        }
    }
}