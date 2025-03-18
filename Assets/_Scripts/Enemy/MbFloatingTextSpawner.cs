using System.Collections.Concurrent;
using UnityEngine;

namespace HitAndRun.Enemy
{
    public class MbFloatingTextSpawner : MbSingleton<MbFloatingTextSpawner>
    {
        [SerializeField] private MbFloatingText _prefab;

        private ConcurrentQueue<MbFloatingText> _pools = new();
        private void Reset()
        {
            _prefab = Resources.Load<MbFloatingText>("Prefabs/FloatingText");
        }

        public MbFloatingText Spawn(Vector3 position, Transform parent, string text, float scale = 0.5f)
        {
            if (_pools.Count == 0)
            {
                var newFloatingText = Instantiate(_prefab, transform);
                _pools.Enqueue(newFloatingText);
            }

            _pools.TryDequeue(out var floatingText);
            floatingText.transform.position = position;
            floatingText.transform.SetParent(parent);
            floatingText.gameObject.SetActive(true);
            floatingText.Popup(text, scale);
            return floatingText;

        }

        public void Despawn(MbFloatingText floatingText)
        {
            floatingText.transform.SetParent(transform);
            floatingText.gameObject.SetActive(false);
            _pools.Enqueue(floatingText);
        }
    }
}

