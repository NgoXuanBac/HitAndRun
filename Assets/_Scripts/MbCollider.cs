using System;
using UnityEngine;

namespace HitAndRun
{
    public class MbCollider : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        public Action<GameObject> TriggerEnter;
        public Action<GameObject> TriggerExit;
        public Action<GameObject> CollisionEnter;
        public Action<GameObject> CollisionExit;

        private void OnEnable()
        {
            if (_collider == null) return;
            _collider.enabled = true;
        }
        private void OnDisable()
        {
            if (_collider == null) return;
            _collider.enabled = false;
        }

        private void Reset()
        {
            _collider = GetComponent<Collider>();
        }

        private void OnCollisionEnter(Collision other)
        {
            CollisionEnter?.Invoke(other.gameObject);
        }

        private void OnCollisionExit(Collision other)
        {
            CollisionExit?.Invoke(other.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            TriggerEnter?.Invoke(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            TriggerExit?.Invoke(other.gameObject);
        }
    }

}
