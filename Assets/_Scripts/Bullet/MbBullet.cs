using UnityEngine;

namespace HitAndRun.Bullet
{
    public class MbBullet : MonoBehaviour
    {
        [SerializeField, Range(100, 500)] private float _maxDistance = 300f;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private TrailRenderer _trailRenderer;
        public TrailRenderer TrailRenderer => _trailRenderer;
        private Vector3 _start;
        private int _damage = 1;
        public int Damage => _damage;
        private void Reset()
        {
            _damage = 1;
            _meshRenderer = transform.GetComponentInChildren<MeshRenderer>();
            _trailRenderer = transform.GetComponent<TrailRenderer>();
        }
        private void OnEnable()
        {
            _start = transform.position;
        }
        private void Update()
        {
            if (Vector3.Distance(_start, transform.position) > _maxDistance)
                MbBulletSpawner.Instance.Despawn(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Tower") && !other.CompareTag("Enemy")) return;
            MbBulletSpawner.Instance.Despawn(this);
        }

        public void SetProperties(Color color, int damage)
        {
            _meshRenderer.material.SetColor("_BaseColor", color);
            var gradient = new Gradient();

            gradient.SetKeys(
                new GradientColorKey[] { new(color, 0.0f), new(color, 1.0f) },
                new GradientAlphaKey[] { new(0.4f, 0.0f), new(0.0f, 1.0f) }
            );

            _trailRenderer.colorGradient = gradient;
            _damage = damage;
        }
    }

}
