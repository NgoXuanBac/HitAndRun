using UnityEngine;

namespace HitAndRun.Character.State
{
    public class AttackState : BaseState
    {
        public AttackState(MbCharacter character, Animator animator) : base(character, animator)
        {
        }

        public override void OnEnter()
        {
            _animator.CrossFade(AttackHash, CrossFadeDuration);
        }
    }
}