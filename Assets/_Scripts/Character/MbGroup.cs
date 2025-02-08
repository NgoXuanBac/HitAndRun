using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;
using System.Linq;

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
        [SerializeField, Range(0, 1)] private float _offset = 0.4f;
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

        private async Task MergeCharacter(int one, int two)
        {
            if (Mathf.Abs(_row[one].transform.position.x) < Mathf.Abs(_row[two].transform.position.x))
                (two, one) = (one, two);

            _row[two].Body.Level *= 2;

            var target = _row[two].transform.localPosition;
            await _row[one].transform
                .DOLocalMove(target, 0.3f)
                .SetEase(Ease.OutQuad).ToUniTask();

            DespawnCharacter(_row[one]);
        }

        private async UniTask RearrangeCharacters()
        {
            if (_row.Count == 0) return;

            var start = -0.5f * (_row.Sum(c => c.Body.Width) + (_row.Count - 1) * _offset);

            var tasks = _row.Select((character, i) =>
            {
                var target = new Vector3(start + character.Body.Width * 0.5f, 0, 0);

                start += character.Body.Width + _offset;

                return character.transform
                    .DOLocalMove(target, 0.3f)
                    .SetEase(Ease.OutQuad)
                    .ToUniTask();
            });

            await UniTask.WhenAll(tasks);
        }

        public async void AddCharacterAt(MbCharacter character, int index)
        {
            if (index < 0 || index > _row.Count) return;
            character.transform.SetParent(transform);
            _row.Insert(index, character);
            await RearrangeCharacters();

            bool merged;
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

            } while (merged);

            await RearrangeCharacters();
        }

        public void AddCharacter()
        {
            var character = SpawnCharacter(transform.position + new Vector3(0, 0, 8f));
            AddCharacterAt(character, UnityEngine.Random.Range(0, _row.Count + 1));
        }


    }
}