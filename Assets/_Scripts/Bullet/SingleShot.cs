using UnityEngine;

namespace HitAndRun.Bullet
{
    public class SingleShot : IShootingPattern
    {
        public void Shoot(float speed, Transform fire, Color color, Vector3 scale)
        {
            var bullet = MbBulletSpawner.Instance.SpawnBullet(fire.position, scale, color);
            bullet.GetComponent<Rigidbody>().velocity = fire.forward * speed;
        }
    }

}
