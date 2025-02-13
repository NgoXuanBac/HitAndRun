using HitAndRun.Inspector;
using UnityEngine;
using UnityEngine.Events;

namespace HitAndRun
{
    [RequireComponent(typeof(BoxCollider))]
    public class MbTrigger : MonoBehaviour
    {
        [SerializeField, TagSelector]
        private string _tag = "Player";
        public UnityEvent OnEnter;
        public UnityEvent OnExit;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(_tag)) return;
            OnEnter?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(_tag)) return;
            OnExit?.Invoke();
        }
    }
}
