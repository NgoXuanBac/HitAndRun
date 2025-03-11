using UnityEngine;

namespace HitAndRun.Enemy
{
    public class IdleState : BaseState
    {
        public IdleState(Animator animator) : base(animator)
        {
        }

        public override void OnEnter()
        {
            _animator.CrossFade(IdleHash, CrossFadeDuration);
        }
    }
}