using UnityEngine;

namespace HitAndRun.Enemy.State
{
    public class DyingState : BaseState
    {
        public DyingState(MbEnemy enemy, Animator animator) : base(enemy, animator)
        {
        }

        public override void OnEnter()
        {
            _animator.CrossFade(DyingHash, CrossFadeDuration);
        }
        public override void Update()
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.shortNameHash == DyingHash && stateInfo.normalizedTime >= 1f)
            {
                MbEnemySpawner.Instance.Despawn(_enemy);
            }
        }
    }
}