using UnityEngine;

namespace HitAndRun.Character.State
{
    public class RunState : BaseState
    {
        public RunState(MbCharacter character, Animator animator) : base(character, animator)
        {
        }

        public override void OnEnter()
        {
            _animator.CrossFade(RunHash, CrossFadeDuration);
        }
    }
}