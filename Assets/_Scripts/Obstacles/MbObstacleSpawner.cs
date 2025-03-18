using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HitAndRun.Obstacles
{
    public class MbObstacleSpawner : MbSingleton<MbObstacleSpawner>
    {
        [SerializeField]
        private List<MbObstacle> _prefabs = new List<MbObstacle>();
        private Dictionary<Type, Queue<MbObstacle>> _pools = new();
        private void Reset()
        {
            _prefabs = Resources.LoadAll<MbObstacle>("Prefabs/Obstacles").ToList();
        }
        private void Awake()
        {
            foreach (var prefab in _prefabs)
            {
                var type = prefab.GetType();
                if (!_pools.ContainsKey(type))
                    _pools.Add(type, new Queue<MbObstacle>());
            }
        }

        public MbObstacle SpawnRandom(Vector3 position, Transform parent)
        {
            var type = _prefabs[UnityEngine.Random.Range(0, _prefabs.Count)].GetType();

            if (!_pools.ContainsKey(type)) return null;

            MbObstacle obstacle;
            if (_pools[type].Count > 0)
            {
                obstacle = _pools[type].Dequeue();
            }
            else
            {
                var prefab = _prefabs.Find(e => e.GetType() == type);
                if (prefab == null) return null;
                obstacle = Instantiate(prefab);
            }

            obstacle.transform.position = position;
            obstacle.transform.SetParent(parent);
            obstacle.gameObject.SetActive(true);

            return obstacle;
        }

        public T Spawn<T>(Vector3 position, Transform parent) where T : MbObstacle
        {
            var type = typeof(T);
            if (!_pools.ContainsKey(type)) return null;

            MbObstacle obstacle;
            if (_pools[type].Count > 0)
            {
                obstacle = _pools[type].Dequeue();
            }
            else
            {
                var prefab = _prefabs.Find(e => e.GetType() == type);
                if (prefab == null) return null;
                obstacle = Instantiate(prefab);
            }

            obstacle.transform.position = position;
            obstacle.transform.SetParent(parent);
            obstacle.gameObject.SetActive(true);
            return (T)obstacle;
        }

        public void Despawn(MbObstacle obstacle)
        {
            var type = obstacle.GetType();
            if (!_pools.ContainsKey(type)) return;

            obstacle.transform.parent = transform;
            obstacle.gameObject.SetActive(false);
            _pools[type].Enqueue(obstacle);
        }
    }
}

