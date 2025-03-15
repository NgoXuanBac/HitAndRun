using System.Linq;
using HitAndRun.Inspector;
using UnityEngine;

namespace HitAndRun
{
    public class MbAutoTarget : MonoBehaviour
    {
        [SerializeField] private LayerMask _targetLM;
        [SerializeField, TagSelector] private string _targetTag = "";
        [SerializeField] private float _range = 35f;
        [SerializeField] private float _targetRate = 0.5f;

        private Transform _target;
        public Transform Target => _target;

        private float _nextTargetTime;
        private void Update()
        {
            if (Time.time >= _nextTargetTime)
            {
                FindTarget();
                _nextTargetTime = Time.time + _targetRate;
            }
        }
        private void FindTarget()
        {
            var enemies = Physics.OverlapSphere(transform.position, _range, _targetLM)
                .Where(t => t.CompareTag(_targetTag))
                .ToArray();
            var min = Mathf.Infinity;

            Transform closest = null;

            foreach (var enemy in enemies)
            {
                var dist = Vector3.Distance(transform.position, enemy.transform.position);
                if (dist < min)
                {
                    min = dist;
                    closest = enemy.transform;
                }
            }
            _target = closest;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _range);
        }
#endif
    }

}
