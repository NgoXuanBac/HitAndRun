using UnityEngine;

namespace HitAndRun.Bullet
{
    public class MbBullet : MonoBehaviour
    {
        [SerializeField, Range(100, 500)] private float _maxDistance = 300f;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private TrailRenderer _trailRenderer;
        public TrailRenderer TrailRenderer => _trailRenderer;
        private Vector3 start;
        private void Reset()
        {
            _meshRenderer = transform.GetComponentInChildren<MeshRenderer>();
            _trailRenderer = transform.GetComponent<TrailRenderer>();
        }
        private void OnEnable()
        {
            start = transform.position;
        }
        private void Update()
        {
            if (Vector3.Distance(start, transform.position) > _maxDistance)
                MbBulletSpawner.Instance.DespawnBullet(this);
        }
        public void SetColor(Color color)
        {
            _meshRenderer.material.SetColor("_BaseColor", color);
            var gradient = new Gradient();

            gradient.SetKeys(
                new GradientColorKey[] { new(color, 0.0f), new(color, 1.0f) },
                new GradientAlphaKey[] { new(0.4f, 0.0f), new(0.0f, 1.0f) }
            );

            _trailRenderer.colorGradient = gradient;
        }
    }

}
