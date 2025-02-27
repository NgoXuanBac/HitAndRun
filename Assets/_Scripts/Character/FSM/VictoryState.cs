using UnityEngine;

namespace HitAndRun.Character.FSM
{
    public class VictoryState : BaseState
    {
        public VictoryState(MbCharacter character, Animator animator) : base(character, animator)
        {
        }

        public override void OnEnter()
        {
            _animator.CrossFade(VictoryHash, CrossFadeDuration);
        }
    }

}
