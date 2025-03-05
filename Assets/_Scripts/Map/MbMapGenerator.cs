using System.Collections.Generic;
using System.Linq;
using HitAndRun.Character;
using HitAndRun.Gate;
using HitAndRun.Inspector;
using HitAndRun.Tower;
using UnityEditor;
using UnityEngine;

namespace HitAndRun.Map
{
    public class MbMapGenerator : MonoBehaviour
    {
        [SerializeField] private MbGround _ground;

        [Header("Chunk")]
        [SerializeField] private int _chunkCount = 60;
        [SerializeField, ReadOnly] private float _chunkHeight;
        [SerializeField, ReadOnly] private float _chunkWidth;
        [SerializeField] private List<SOSpawnRule> _spawnRules;

        private Dictionary<SpawnType, SOSpawnRule> _spawnRuleWithType;
        private void Reset()
        {
            _ground = GetComponentInChildren<MbGround>();
            _spawnRules = Resources.LoadAll<SOSpawnRule>("Scriptables").ToList();
            _ground.ChunkCount = _chunkCount;
            _chunkHeight = _ground.Length / _chunkCount;
            _chunkWidth = _ground.Width;
        }

        private void Awake()
        {
            _spawnRuleWithType = _spawnRules.GroupBy(x => x.SpawnType).ToDictionary(g => g.Key, g => g.Single());
        }

        private void Start()
        {
            GenerateMap();
        }

        public void GenerateMap()
        {
            var chunks = new int[_chunkCount];
            for (int index = 3; index < _chunkCount; index++)
            {
                chunks[index] = Random.Range((int)SpawnType.Tower, (int)SpawnType.Item + 1);
            }

            for (int index = 3; index < _chunkCount; index++)
            {
                var type = (SpawnType)chunks[index];
                switch (type)
                {
                    case SpawnType.Tower:
                        if (_spawnRuleWithType.TryGetValue(type, out var rule))
                        {
                            var ratios = rule.GetSpawnRatios();
                            foreach (var ratio in ratios)
                            {
                                MbCharacterSpawner.Instance.Spawn(new Vector3(ratio * _chunkWidth * 0.5f, 8, index * _chunkHeight), transform, MbCharacter.INACTIVE_TAG);
                                MbTowerSpawner.Instance.Spawn(new Vector3(ratio * _chunkWidth * 0.5f, 0, index * _chunkHeight), transform, 300);
                            }
                        }
                        break;
                    case SpawnType.Gate:
                        MbGateSpawner.Instance.SpawnDual(index, _chunkHeight, _chunkWidth, transform);
                        break;
                }


            }
        }
    }

    public enum SpawnType
    {
        Tower,
        Gate,
        Obstacle,
        Item
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MbMapGenerator))]
    public class EMapGeneratorInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            var generator = (MbMapGenerator)target;
            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button($"Generate"))
            {
                generator.GenerateMap();
            }
            GUI.enabled = true;
            EditorGUILayout.Space();
            DrawDefaultInspector();
        }
    }
#endif

}
