using HitAndRun.Bullet;
using UnityEngine;

namespace HitAndRun.Bullet
{
    public class SpreadShot : IShootingPattern
    {
        public void Shoot(float speed, Transform fire, Color color, Vector3 scale, int damage = 1)
        {
            float[] angles = { -10f, 0, 10f };
            foreach (var angle in angles)
            {
                var bullet = MbBulletSpawner.Instance.Spawn(fire.position, scale, color, damage);
                Vector3 direction = Quaternion.AngleAxis(angle, Vector3.up) * fire.forward;
                bullet.GetComponent<Rigidbody>().velocity = direction * speed;
            }
        }
    }
}
