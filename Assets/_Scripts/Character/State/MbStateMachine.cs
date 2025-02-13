using UnityEngine;

namespace HitAndRun.Character.State
{
    public class MbStateMachine : MonoBehaviour
    {
        [SerializeField] private MbDeadState _deadState;
        [SerializeField] private MbShootingState _shootingState;
        [SerializeField] private MbIdleState _idleState;

        private MbCharacterState _currentState;

        private void Reset()
        {
            _deadState = GetComponent<MbDeadState>();
            _shootingState = GetComponent<MbShootingState>();
            _idleState = GetComponent<MbIdleState>();

            _idleState.enabled = false;
            _shootingState.enabled = false;
            _deadState.enabled = false;
        }

        public void ChangeState(MbCharacterState newState)
        {
            if (_currentState != null)
                _currentState.enabled = false;
            _currentState = newState;
            _currentState.enabled = true;
        }
    }
}

