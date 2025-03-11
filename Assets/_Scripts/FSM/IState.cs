using UnityEngine;

namespace HitAndRun.FSM
{
    public interface IState
    {
        void OnEnter();
        void OnExit();
        void Update();
        void FixedUpdate();
    }
}
