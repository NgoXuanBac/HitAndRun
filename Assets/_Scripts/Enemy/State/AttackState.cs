using UnityEngine;

namespace HitAndRun.Enemy.State
{
    public class AttackState : BaseState
    {
        public AttackState(Animator animator) : base(animator)
        {
        }

        public override void OnEnter()
        {
            _animator.CrossFade(AttackHash, CrossFadeDuration);
        }
    }
}