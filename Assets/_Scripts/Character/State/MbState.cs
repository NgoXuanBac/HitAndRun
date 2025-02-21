using UnityEngine;

namespace HitAndRun.Character.State
{
    [RequireComponent(typeof(MbStateMachine))]
    public abstract class MbState : MonoBehaviour
    {
        [SerializeField]
        private MbStateMachine _stateMachine;
        protected MbStateMachine StateMachine => _stateMachine;

        protected void OnReset()
        {
            _stateMachine = GetComponent<MbStateMachine>();
        }

        protected void OnEnable()
        {
            OnEnter();
        }

        protected void OnDisable()
        {
            OnExit();
        }
        protected abstract void OnEnter();
        protected abstract void OnExit();
    }
}