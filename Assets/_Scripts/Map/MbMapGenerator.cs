using System.Linq;
using HitAndRun.Character;
using HitAndRun.Gate;
using HitAndRun.Tower;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using HitAndRun.Enemy;
using HitAndRun.Bullet;
using HitAndRun.Coin;
using HitAndRun.Obstacles;
using HitAndRun.Gate.Modifier;

namespace HitAndRun.Map
{
    public class MbMapGenerator : MonoBehaviour
    {
        [SerializeField] private MbLevelGeneration _levelGeneration;
        [SerializeField] private MbGround _ground;
        [SerializeField] private bool _spawnObject = true;
        [SerializeField] private List<SOSpawnRule> _spawnRules;

        private Dictionary<SpawnType, SOSpawnRule> _spawnRuleWithType;

        private void Reset()
        {
            _ground = GetComponentInChildren<MbGround>();
            _spawnRules = Resources.LoadAll<SOSpawnRule>("Scriptables").ToList();
            _levelGeneration = GetComponent<MbLevelGeneration>();
        }

        private void Awake()
        {
            _spawnRuleWithType = _spawnRules.GroupBy(x => x.SpawnType).ToDictionary(g => g.Key, g => g.Single());
        }

        public void GenerateMap()
        {
            if (_spawnObject)
            {
                var chunks = _levelGeneration.Generate(MbGameManager.Instance.Data.Level);
                _ground.ChunkCount = chunks.Length;

                var chunkWidth = _ground.Width;
                var chunkHeight = _ground.Length / chunks.Length;

                for (int index = 3; index < chunks.Length; index++)
                {
                    var type = chunks[index];

                    if (!_spawnRuleWithType.TryGetValue(type, out var rule))
                        continue;
                    var ratios = rule.GetRandomSpawnRatios();

                    switch (type)
                    {
                        case SpawnType.Tower:
                            foreach (var ratio in ratios)
                            {
                                MbCharacterSpawner.Instance.Spawn(new Vector3(ratio * chunkWidth * 0.5f, 8, index * chunkHeight), transform);
                                MbTowerSpawner.Instance.Spawn(new Vector3(ratio * chunkWidth * 0.5f, 0, index * chunkHeight), transform, 300);
                            }
                            break;
                        case SpawnType.Gate:
                            var random = UnityEngine.Random.Range(0, 2) == 1 ? ModifierCategory.Positive : ModifierCategory.Negative;
                            foreach (var ratio in ratios)
                            {
                                MbGateSpawner.Instance.SpawnRandom(new Vector3(ratio * chunkWidth * 0.5f, 0, index * chunkHeight), transform, random);
                                random = random == ModifierCategory.Positive ? ModifierCategory.Negative : ModifierCategory.Positive;
                            }
                            break;
                        case SpawnType.Coin:
                            foreach (var ratio in ratios)
                            {
                                MbCoinSpawner.Instance.SpawnGrid(new Vector3(ratio * chunkWidth * 0.5f, 1, index * chunkHeight), 3, 3);
                            }
                            break;
                        case SpawnType.Obstacle:

                            foreach (var ratio in ratios)
                            {
                                MbObstacleSpawner.Instance.SpawnRandom(new Vector3(ratio * chunkWidth * 0.5f, 0, index * chunkHeight), transform);
                            }
                            break;
                    }
                }
            }

            // MbEnemySpawner.Instance.Spawn<MbMonster>(_ground.Enemy, Quaternion.Euler(0, 180, 0), transform, 300);
            MbEnemySpawner.Instance.Spawn<MbZombie>(_ground.Enemy + new Vector3(5, 0, 0), Quaternion.Euler(0, 180, 0), transform, 300);
            MbEnemySpawner.Instance.Spawn<MbZombie>(_ground.Enemy + new Vector3(-5, 0, 0), Quaternion.Euler(0, 180, 0), transform, 300);
            MbEnemySpawner.Instance.Spawn<MbZombie>(_ground.Enemy, Quaternion.Euler(0, 180, 0), transform, 300);

        }


        public void CleanMap()
        {
            var enemies = FindObjectsOfType<MbEnemy>();
            var characters = FindObjectsOfType<MbCharacter>();
            var bullets = FindObjectsOfType<MbBullet>();
            var towers = FindObjectsOfType<MbTower>();
            var coins = FindObjectsOfType<MbCoin>();
            var obstacles = FindObjectsOfType<MbObstacle>();
            var gates = FindObjectsOfType<MbModifierBase>();

            foreach (var gate in gates) MbGateSpawner.Instance.Despawn(gate);
            foreach (var obstacle in obstacles) MbObstacleSpawner.Instance.Despawn(obstacle);
            foreach (var coin in coins) MbCoinSpawner.Instance.Despawn(coin);
            foreach (var enemy in enemies) MbEnemySpawner.Instance.Despawn(enemy);
            foreach (var character in characters) MbCharacterSpawner.Instance.Despawn(character);
            foreach (var bullet in bullets) MbBulletSpawner.Instance.Despawn(bullet);
            foreach (var tower in towers) MbTowerSpawner.Instance.Despawn(tower);
        }
    }

    public enum SpawnType
    {
        Tower,
        Gate,
        Obstacle,
        Character,
        Coin
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
