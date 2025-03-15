using UnityEngine;

namespace HitAndRun.Bullet
{
    public class SingleShot : IShootingPattern
    {
        public void Shoot(float speed, Transform fire, Color color, Vector3 scale, int damage = 1)
        {
            var bullet = MbBulletSpawner.Instance.Spawn(fire.position, scale, color, damage);
            bullet.GetComponent<Rigidbody>().velocity = fire.forward * speed;
        }
    }

}
