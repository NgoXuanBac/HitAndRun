using UnityEngine;

namespace HitAndRun.Enemy
{
    public abstract class MbDamageable : MonoBehaviour
    {
        public abstract void TakeDamage(int damage);
    }

}
