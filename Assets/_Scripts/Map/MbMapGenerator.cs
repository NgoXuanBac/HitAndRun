using System.Linq;
using HitAndRun.Character;
using HitAndRun.Gate;
using HitAndRun.Tower;
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
        [SerializeField] private bool _renderMap = true;
        [SerializeField] private List<SOSpawnRule> _spawnRules;

        [Header("Seed")]
        [SerializeField, Range(100f, 500f)] private long _bossBaseHP = 300;
        [SerializeField, Range(0, 1)] private float _bossScale = 0.1f;
        [SerializeField, Range(1, 10)] private float _timeToKill = 8f;
        [SerializeField, Range(1, 50)] private int _baseTowerHP = 20;
        [SerializeField, Range(0.1f, 1f)] private float _towerPercent = 0.8f;

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

        public void GenerateMap(GameData data)
        {
            var towerHp = _baseTowerHP;
            var bossHp = (long)(_bossBaseHP * (1 + data.Level * _bossScale));
            var rate = 1f / (0.1f + (0.5f - 0.1f) * Mathf.Exp(-0.2f * data.FireRate));
            var characterNum = Mathf.CeilToInt(bossHp / (2 * data.Damage * _timeToKill * rate));

            var chunks = _levelGeneration.Generate(data.Level, characterNum);
            _ground.ChunkCount = chunks.Count;

            if (data.Level % 3 == 0)
            {
                MbEnemySpawner.Instance.Spawn<MbMonster>(_ground.Enemy, Quaternion.Euler(0, 180, 0), transform, bossHp);
            }
            else
            {
                MbEnemySpawner.Instance.Spawn<MbZombie>(_ground.Enemy + new Vector3(5, 0, 0), Quaternion.Euler(0, 180, 0), transform, bossHp / 3);
                MbEnemySpawner.Instance.Spawn<MbZombie>(_ground.Enemy + new Vector3(-5, 0, 0), Quaternion.Euler(0, 180, 0), transform, bossHp / 3);
                MbEnemySpawner.Instance.Spawn<MbZombie>(_ground.Enemy, Quaternion.Euler(0, 180, 0), transform, bossHp / 3);
            }

            if (!_renderMap) return;

            var chunkWidth = _ground.Width;
            var chunkHeight = _ground.Length / chunks.Count;

            for (int index = 3; index < chunks.Count; index++)
            {
                var type = chunks[index];

                if (!_spawnRuleWithType.TryGetValue(type, out var rule))
                    continue;
                var ratios = rule.GetRandomSpawnRatios();

                switch (type)
                {
                    case SpawnType.Character:
                        foreach (var ratio in ratios)
                        {
                            var isTower = Random.Range(0, 1f) <= _towerPercent;
                            if (isTower)
                            {
                                MbCharacterSpawner.Instance.Spawn(new Vector3(ratio * chunkWidth * 0.5f, 8, index * chunkHeight), transform);
                                MbTowerSpawner.Instance.Spawn(new Vector3(ratio * chunkWidth * 0.5f, 0, index * chunkHeight), transform, towerHp);
                            }
                            else
                            {
                                MbCharacterSpawner.Instance.Spawn(new Vector3(ratio * chunkWidth * 0.5f, 0, index * chunkHeight), transform);
                            }
                        }
                        break;
                    case SpawnType.Gate:
                        var category = Random.Range(0, 2) == 1 ? ModifierCategory.Positive : ModifierCategory.Negative;
                        foreach (var ratio in ratios)
                        {
                            MbGateSpawner.Instance.SpawnRandom(new Vector3(ratio * chunkWidth * 0.5f, 0, index * chunkHeight), transform, category);
                            category = category == ModifierCategory.Positive ? ModifierCategory.Negative : ModifierCategory.Positive;
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
        Gate,
        Obstacle,
        Character,
        Coin,
        Empty
    }

}
