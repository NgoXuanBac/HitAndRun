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

        public bool Enabled
        {
            get => _collider.enabled;
            set => _collider.enabled = value;
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
