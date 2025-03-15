using UnityEngine;

namespace HitAndRun.Bullet
{
    public interface IShootingPattern
    {
        void Shoot(float speed, Transform fire, Color color, Vector3 scale, int damage = 1);
    }
}