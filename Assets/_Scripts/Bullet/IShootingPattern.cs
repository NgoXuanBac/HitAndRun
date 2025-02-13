using UnityEngine;

namespace HitAndRun.Bullet
{
    public interface IShootingPattern
    {
        void Shoot(float bulletSpeed, Transform firePoint);
    }
}