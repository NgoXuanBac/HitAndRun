using System.Collections.Concurrent;
using UnityEngine;

namespace HitAndRun.Tower
{
    public class MbTowerSpawner : MbSingleton<MbTowerSpawner>
    {
        [SerializeField] private MbTower _prefab;
        private ConcurrentQueue<MbTower> _pools = new();

        private void Reset()
        {
            _prefab ??= Resources.Load<MbTower>("Prefabs/Tower");
        }

        public MbTower Spawn(Vector3 position, Transform parent, long health)
        {
            if (_pools.Count == 0)
            {
                var newTower = Instantiate(_prefab, transform);
                _pools.Enqueue(newTower);
            }

            _pools.TryDequeue(out var tower);
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
            _pools.Enqueue(tower);
        }
    }
}
