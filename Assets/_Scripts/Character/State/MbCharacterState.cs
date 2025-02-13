using UnityEngine;

namespace HitAndRun.Character.State
{
    [RequireComponent(typeof(MbStateMachine))]
    public abstract class MbCharacterState : MonoBehaviour
    {
        [SerializeField]
        private MbStateMachine _stateMachine;
        protected MbStateMachine StateMachine => _stateMachine;

        protected void OnReset()
        {
            _stateMachine = GetComponent<MbStateMachine>();
            enabled = false;
        }
    }
}