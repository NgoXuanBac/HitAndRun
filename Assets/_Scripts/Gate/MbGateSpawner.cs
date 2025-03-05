using System.Collections.Concurrent;
using UnityEngine;

namespace HitAndRun.Gate
{
    public class MbGateSpawner : MbSingleton<MbGateSpawner>
    {
        [SerializeField] private MbGate _prefab;
        private ConcurrentQueue<MbGate> _pool = new();
        private void Reset()
        {
            _prefab = Resources.Load<MbGate>("Prefabs/Gate");
        }
        public void SpawnDual(int index, float chunkHeight, float chunkWidth, Transform parent)
        {
            var posR = new Vector3(0.25f * chunkWidth, 0, index * chunkHeight);
            var posL = new Vector3(-0.25f * chunkWidth, 0, index * chunkHeight);

            Spawn(posR, parent);
            Spawn(posL, parent);
        }

        public MbGate Spawn(Vector3 position, Transform parent)
        {
            if (_pool.Count == 0)
            {
                var newGate = Instantiate(_prefab, parent);
                newGate.gameObject.SetActive(false);
                _pool.Enqueue(newGate);
            }
            _pool.TryDequeue(out var gate);
            gate.transform.position = position;
            gate.gameObject.SetActive(true);
            gate.tag = tag;
            return gate;
        }

        public void Despawn(MbGate gate)
        {
            gate.transform.SetParent(transform);
            gate.gameObject.SetActive(false);
            _pool.Enqueue(gate);
        }
    }
}

