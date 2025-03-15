using UnityEngine;

namespace HitAndRun.Enemy.State
{
    public class WalkState : BaseState
    {
        public WalkState(MbEnemy enemy, Animator animator) : base(enemy, animator)
        {
        }

        public override void OnEnter()
        {
            _animator.CrossFade(WalkHash, CrossFadeDuration);
        }
    }
}