using UnityEngine;

namespace HitAndRun.Enemy.State
{
    public class WalkState : BaseState
    {
        public WalkState(Animator animator) : base(animator)
        {
        }

        public override void OnEnter()
        {
            _animator.CrossFade(WalkHash, CrossFadeDuration);
        }
    }
}