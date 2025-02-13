using UnityEngine;

namespace HitAndRun.Bullet
{
    public class MbBullet : MonoBehaviour
    {
        [SerializeField, Range(100, 500)] private float _maxDistance = 100f;
        private Vector3 start;
        void OnEnable()
        {
            start = transform.position;
        }
        private void Update()
        {
            if (Vector3.Distance(start, transform.position) > _maxDistance)
                MbBulletSpawner.Instance.DespawnBullet(this);
        }

    }

}
