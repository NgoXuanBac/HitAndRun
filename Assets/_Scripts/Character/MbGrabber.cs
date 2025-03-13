using System;
using UnityEngine;

namespace HitAndRun.Character
{
    public class MbGrabber : MonoBehaviour
    {
        public event Action<MbCharacter, MbCharacter, bool> OnGrab;
        [SerializeField] private MbCharacter _character;
        private void Reset()
        {
            _character = transform.parent.GetComponent<MbCharacter>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(_character.INACTIVE_TAG)) return;
            var dir = (other.transform.position - _character.transform.position).normalized;
            var side = Vector3.Dot(dir, _character.transform.right);
            OnGrab?.Invoke(_character, other.GetComponent<MbCharacter>(), side > 0);
        }
    }
}

