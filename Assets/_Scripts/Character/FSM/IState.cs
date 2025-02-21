using UnityEngine;

namespace HitAndRun.Character.FSM
{
    public interface IState
    {
        void OnEnter();
        void OnExit();
        void Update();
        void FixedUpdate();
    }
    public abstract class BaseState : IState
    {
        protected readonly Animator _animator;
        protected BaseState(Animator animator)
        {
            _animator = animator;
        }
        public virtual void OnEnter()
        {
        }

        public virtual void OnExit()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void FixedUpdate()
        {
        }
    }
}
