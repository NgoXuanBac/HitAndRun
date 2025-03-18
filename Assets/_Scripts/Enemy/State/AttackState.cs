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

        public override void Update()
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.shortNameHash == AttackHash && stateInfo.normalizedTime >= 1f)
            {
                _enemy.IsAttacking = false;
            }
        }
    }
}