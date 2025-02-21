using System.Collections.Concurrent;
using UnityEngine;

namespace HitAndRun.Bullet
{
    public class MbBulletSpawner : MonoBehaviour
    {
        [SerializeField] private MbBullet _prefab;
        private ConcurrentQueue<MbBullet> _pool = new();
        private static MbBulletSpawner _instance;
        public static MbBulletSpawner Instance => _instance;

        private void Awake()
        {
            _instance ??= this;
        }

        private void Reset()
        {
            _prefab = Resources.Load<MbBullet>("Prefabs/Bullet");
        }

        public MbBullet SpawnBullet(Vector3 position, Vector3 scale, Color color)
        {
            if (_pool.Count == 0)
            {
                var newBullet = Instantiate(_prefab, transform);
                _pool.Enqueue(newBullet);
            }

            _pool.TryDequeue(out var bullet);
            bullet.transform.localScale = scale;
            bullet.transform.position = position;
            bullet.SetColor(color);
            bullet.gameObject.SetActive(true);
            return bullet;
        }

        public void DespawnBullet(MbBullet bullet)
        {
            bullet.gameObject.SetActive(false);
            _pool.Enqueue(bullet);
        }
    }
}

