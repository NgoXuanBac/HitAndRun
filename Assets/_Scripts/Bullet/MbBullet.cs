using UnityEngine;

namespace HitAndRun.Bullet
{
    public class MbBullet : MonoBehaviour
    {
        [SerializeField, Range(100, 500)] private float _maxDistance = 100f;
        [SerializeField] private MeshRenderer _meshRenderer;
        private Vector3 start;
        private void Reset()
        {
            _meshRenderer = transform.GetComponentInChildren<MeshRenderer>();
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
        public void SetColor(Color color) => _meshRenderer.material.SetColor("_BaseColor", color);
    }

}
