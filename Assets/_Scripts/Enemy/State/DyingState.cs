using UnityEngine;

namespace HitAndRun.Enemy
{
    public class DyingState : BaseState
    {
        public DyingState(Animator animator) : base(animator)
        {
        }

        public override void OnEnter()
        {
            _animator.CrossFade(DyingHash, CrossFadeDuration);
        }
    }
}