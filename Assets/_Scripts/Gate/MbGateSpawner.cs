using System;
using System.Collections.Generic;
using System.Linq;
using HitAndRun.Gate.Modifier;
using UnityEngine;

namespace HitAndRun.Gate
{
    public class MbGateSpawner : MbSingleton<MbGateSpawner>
    {
        [SerializeField] private List<MbModifierBase> _prefabs;
        private Dictionary<Type, Queue<MbModifierBase>> _pools = new();

        private void Reset()
        {
            _prefabs = Resources.LoadAll<MbModifierBase>("Prefabs/Gates").ToList();
        }

        private void Awake()
        {
            foreach (var prefab in _prefabs)
            {
                var type = prefab.GetType();
                if (!_pools.ContainsKey(type))
                    _pools.Add(type, new Queue<MbModifierBase>());
            }
        }

        public MbModifierBase SpawnRandom(Vector3 position, Transform parent, ModifierCategory category)
        {
            var types = _prefabs.Where(e => e.HasCategory(category)).ToList();
            if (types.Count == 0) return null;

            var type = types[UnityEngine.Random.Range(0, types.Count)].GetType();
            if (!_pools.ContainsKey(type)) return null;

            MbModifierBase gate;
            if (_pools[type].Count > 0)
            {
                gate = _pools[type].Dequeue();
            }
            else
            {
                var prefab = _prefabs.Find(e => e.GetType() == type);
                if (prefab == null) return null;
                gate = Instantiate(prefab);
            }

            gate.SetCategory(category);
            gate.transform.position = position;
            gate.transform.SetParent(parent);
            gate.gameObject.SetActive(true);
            return gate;
        }

        public T Spawn<T>(Vector3 position, Transform parent, ModifierCategory category) where T : MbModifierBase
        {
            var type = typeof(T);
            if (!_pools.ContainsKey(type)) return null;

            MbModifierBase gate;
            if (_pools[type].Count > 0)
            {
                gate = _pools[type].Dequeue();
            }
            else
            {
                var prefab = _prefabs.Find(e => e.GetType() == type);
                if (prefab == null) return null;
                gate = Instantiate(prefab);
            }

            gate.SetCategory(category);
            gate.transform.position = position;
            gate.transform.SetParent(parent);
            gate.gameObject.SetActive(true);
            return (T)gate;
        }

        public void Despawn(MbModifierBase gate)
        {
            var type = gate.GetType();
            if (!_pools.ContainsKey(type)) return;

            gate.transform.parent = transform;
            gate.gameObject.SetActive(false);
            _pools[type].Enqueue(gate);
        }
    }
}

