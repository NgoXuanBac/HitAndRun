using UnityEngine;

namespace HitAndRun.Enemy.State
{
    public class AttackState : BaseState
    {
        public AttackState(MbEnemy enemy, Animator animator) : base(enemy, animator)
        {
        }

        public override void OnEnter()
        {
            _animator.CrossFade(AttackHash, CrossFadeDuration);
        }
    }
}