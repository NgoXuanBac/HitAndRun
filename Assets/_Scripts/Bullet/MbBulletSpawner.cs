using System.Collections.Concurrent;
using UnityEngine;

namespace HitAndRun.Bullet
{
    public class MbBulletSpawner : MbSingleton<MbBulletSpawner>
    {
        [SerializeField] private MbBullet _prefab;
        private ConcurrentQueue<MbBullet> _pool = new();

        private void Reset()
        {
            _prefab = Resources.Load<MbBullet>("Prefabs/Bullet");
        }

        public MbBullet Spawn(Vector3 position, Vector3 scale, Color color, int damage = 1)
        {
            if (_pool.Count == 0)
            {
                var newBullet = Instantiate(_prefab, transform);
                newBullet.gameObject.SetActive(false);
                _pool.Enqueue(newBullet);
            }

            _pool.TryDequeue(out var bullet);
            bullet.transform.localScale = scale;
            bullet.transform.position = position;
            bullet.SetProperties(color, damage);
            bullet.gameObject.SetActive(true);
            return bullet;
        }

        public void Despawn(MbBullet bullet)
        {
            bullet.TrailRenderer.Clear();
            bullet.gameObject.SetActive(false);
            _pool.Enqueue(bullet);
        }
    }
}

