using UnityEngine;

namespace HitAndRun.Character.FSM
{
    public class IdleState : BaseState
    {
        public IdleState(MbCharacter character, Animator animator) : base(character, animator)
        {
        }

        public override void OnEnter()
        {
            _animator.CrossFade(IdleHash, CrossFadeDuration);
        }
    }
}