using System.Collections.Concurrent;
using HitAndRun.Map;
using UnityEngine;

namespace HitAndRun.Tower
{
    public class MbTowerSpawner : MbSingleton<MbTowerSpawner>
    {
        [SerializeField] private SOSpawnRule _rule;
        [SerializeField] private MbTower _prefab;
        private ConcurrentQueue<MbTower> _pool = new();
        private void Reset()
        {
            _rule ??= Resources.Load<SOSpawnRule>("Scriptables/TowerSpawnRule");
            _prefab ??= Resources.Load<MbTower>("Prefabs/Tower");
        }

        public bool CanSpawn()
        {
            return true;
        }

        public MbTower Spawn(Vector3 position, Transform parent, long health)
        {
            if (_pool.Count == 0)
            {
                var newTower = Instantiate(_prefab, transform);
                _pool.Enqueue(newTower);
            }

            _pool.TryDequeue(out var tower);
            tower.transform.parent = parent;
            tower.transform.position = position;
            tower.Health = health;
            tower.gameObject.SetActive(true);
            return tower;
        }

        public void Despawn(MbTower tower)
        {
            tower.Reset();
            tower.transform.parent = transform;
            tower.gameObject.SetActive(false);
            _pool.Enqueue(tower);
        }
    }
}
