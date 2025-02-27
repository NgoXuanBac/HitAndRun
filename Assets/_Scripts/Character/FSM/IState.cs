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
        protected readonly MbCharacter _character;
        protected static readonly int RunHash = Animator.StringToHash("Run");
        protected static readonly int IdleHash = Animator.StringToHash("Idle");
        protected static readonly int VictoryHash = Animator.StringToHash("Victory");
        protected static readonly int DyingHash = Animator.StringToHash("Dead");
        protected static readonly int FallHash = Animator.StringToHash("Fall");
        protected const float CrossFadeDuration = 0.1f;
        protected BaseState(MbCharacter character, Animator animator)
        {
            _animator = animator;
            _character = character;
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
