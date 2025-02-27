using UnityEngine;

namespace HitAndRun.Character.FSM
{
    public class DyingState : BaseState
    {
        public DyingState(MbCharacter character, Animator animator) : base(character, animator)
        {
        }

        public override void OnEnter()
        {
            _animator.CrossFade(DyingHash, CrossFadeDuration);
        }
    }
}