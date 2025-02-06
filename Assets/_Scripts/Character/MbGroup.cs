using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace HitAndRun.Character
{
    [CustomEditor(typeof(MbGroup))]
    public class EGroupInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var group = (MbGroup)target;
            EditorGUILayout.Space();
            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button("Add Character"))
            {
                group.AddCharacter();
            }
            GUI.enabled = true;
        }
    }

    public class MbGroup : MonoBehaviour
    {
        [Header("Line up")]
        [SerializeField, Range(1, 5)] private float _offset = 2;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private List<MbCharacter> _row = new();
        private Queue<MbCharacter> _pool = new();
        private void Reset()
        {
            _prefab = Resources.Load<GameObject>("Prefabs/Character");
        }
        private void Start()
        {
            _row.Add(SpawnCharacter(transform.position));
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

        private void MergeCharacter(int from, int to)
        {
            _row[to].Level.Value *= 2;
            DespawnCharacter(_row[from]);
        }

        private async UniTask RearrangeCharacters(int center)
        {
            var tasks = new List<UniTask>();
            for (int i = 0; i < _row.Count; i++)
            {
                var offset = (i - center) * _offset;
                var target = new Vector3(offset, 0, 0);

                var task = _row[i].transform
                    .DOLocalMove(target, 0.3f)
                    .SetEase(Ease.OutQuad)
                    .ToUniTask();

                tasks.Add(task);
            }

            await UniTask.WhenAll(tasks);
        }

        public async void AddCharacterAt(MbCharacter character, int index)
        {
            if (index < 0 || index > _row.Count) return;
            character.transform.SetParent(transform);
            var insert = _row[index == _row.Count ? index - 1 : index].transform.localPosition.x;
            var tasks = new List<UniTask>();
            if (index == 0) insert -= _offset;
            else if (index == _row.Count) insert += _offset;
            else
            {
                insert -= 0.5f * _offset;
                for (int i = 0; i < _row.Count; i++)
                {
                    var move = _row[i].transform.localPosition + Mathf.Sign(i - index)
                            * 0.5f * _offset * Vector3.right;

                    tasks.Add(_row[i].transform
                        .DOLocalMove(move, 0.3f)
                        .SetEase(Ease.OutQuad)
                        .ToUniTask());
                }

            }
            tasks.Add(character.transform
                .DOLocalMove(insert * Vector3.right, 0.3f)
                .SetEase(Ease.OutQuad)
                .ToUniTask());
            await UniTask.WhenAll(tasks);

            _row.Insert(index, character);

            bool merged;
            do
            {
                merged = false;
                if (index > 0 && _row[index - 1].Level.Value == _row[index].Level.Value)
                {
                    await UniTask.Delay(100);
                    MergeCharacter(index, index - 1);
                    index--;
                    merged = true;

                }
                else if (index < _row.Count - 1 && _row[index + 1].Level.Value == _row[index].Level.Value)
                {
                    await UniTask.Delay(100);
                    MergeCharacter(index + 1, index);
                    merged = true;
                }

            } while (merged);
            await RearrangeCharacters(_row.Count / 2);
        }

        public void AddCharacter()
        {
            var character = SpawnCharacter(transform.position + new Vector3(0, 0, 8f));
            AddCharacterAt(character, UnityEngine.Random.Range(0, _row.Count + 1));
        }


    }
}