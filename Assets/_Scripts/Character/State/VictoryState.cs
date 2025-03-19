using UnityEngine;

namespace HitAndRun.Character.State
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

        public override void Update()
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.shortNameHash == VictoryHash && stateInfo.normalizedTime >= 1f)
            {
                _character.IsActive = false;
            }
        }
    }

}
