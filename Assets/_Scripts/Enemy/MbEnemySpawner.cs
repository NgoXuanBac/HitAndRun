using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HitAndRun.Enemy
{
    public class MbEnemySpawner : MbSingleton<MbEnemySpawner>
    {
        [SerializeField] private List<MbEnemy> _prefabs;
        private Dictionary<Type, Queue<MbEnemy>> _pools = new();

        private void Reset()
        {
            _prefabs = Resources.LoadAll<MbEnemy>("Prefabs/Enemies").ToList();
        }

        private void Awake()
        {
            foreach (var prefab in _prefabs)
            {
                var type = prefab.GetType();
                if (!_pools.ContainsKey(type))
                    _pools.Add(type, new Queue<MbEnemy>());
            }
        }

        public T Spawn<T>(Vector3 position, Quaternion rotation, Transform parent, long health) where T : MbEnemy
        {
            var type = typeof(T);
            if (!_pools.ContainsKey(type)) return null;

            MbEnemy enemy;
            if (_pools[type].Count > 0)
            {
                enemy = _pools[type].Dequeue();
            }
            else
            {
                var prefab = _prefabs.Find(e => e.GetType() == type);
                if (prefab == null) return null;
                enemy = Instantiate(prefab);
            }

            enemy.transform.position = position;
            enemy.transform.rotation = rotation;
            enemy.transform.SetParent(parent);
            enemy.Health = health;
            enemy.gameObject.SetActive(true);
            return (T)enemy;
        }

        public void Despawn(MbEnemy enemy)
        {
            var type = enemy.GetType();
            if (!_pools.ContainsKey(type)) return;

            enemy.transform.parent = transform;
            enemy.Reset();
            enemy.gameObject.SetActive(false);
            _pools[type].Enqueue(enemy);
        }
    }
}