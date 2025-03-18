using UnityEngine;

namespace HitAndRun.Enemy.State
{
    public class IdleState : BaseState
    {
        public IdleState(MbEnemy enemy, Animator animator) : base(enemy, animator)
        {
        }

        public override void OnEnter()
        {
            _animator.CrossFade(IdleHash, CrossFadeDuration);
        }
    }
}