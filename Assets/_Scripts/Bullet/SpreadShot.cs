using HitAndRun.Bullet;
using UnityEngine;

public class SpreadShot : IShootingPattern
{
    public void Shoot(float bulletSpeed, Transform firePoint)
    {
        float[] angles = { -10f, 0, 10f };
        foreach (var angle in angles)
        {
            var bullet = MbBulletSpawner.Instance.SpawnBullet(firePoint.position);
            Vector3 direction = Quaternion.AngleAxis(angle, Vector3.up) * firePoint.forward;
            bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
        }
    }
}

