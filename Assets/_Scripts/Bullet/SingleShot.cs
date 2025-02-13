using UnityEngine;

namespace HitAndRun.Bullet
{
    public class SingleShot : IShootingPattern
    {
        public void Shoot(float bulletSpeed, Transform firePoint)
        {
            var bullet = MbBulletSpawner.Instance.SpawnBullet(firePoint.position);
            bullet.GetComponent<Rigidbody>().velocity = firePoint.forward * bulletSpeed;
        }
    }

}
