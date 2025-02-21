using HitAndRun.Inspector;
using UnityEngine;

namespace HitAndRun.Character.State
{
    public class MbStateMachine : MonoBehaviour
    {
        [SerializeField, ReadOnly]
        private MbState _currentState;

        private void Reset()
        {
        }

        public void ChangeState(MbState newState)
        {
            if (newState == null) return;
            _currentState.enabled = false;
            _currentState = newState;
            _currentState.enabled = true;
        }
    }
}