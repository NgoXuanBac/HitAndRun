using HitAndRun.FSM;
using UnityEngine;

namespace HitAndRun.Enemy.State
{
    public class BaseState : IState
    {
        protected readonly Animator _animator;
        protected readonly MbEnemy _enemy;

        protected const float CrossFadeDuration = 0.1f;
        protected static readonly int WalkHash = Animator.StringToHash("Walk");
        protected static readonly int IdleHash = Animator.StringToHash("Idle");
        protected static readonly int AttackHash = Animator.StringToHash("Attack");
        protected static readonly int DyingHash = Animator.StringToHash("Dead");
        protected BaseState(MbEnemy enemy, Animator animator)
        {
            _animator = animator;
            _enemy = enemy;
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