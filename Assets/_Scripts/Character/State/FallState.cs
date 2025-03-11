using UnityEngine;

namespace HitAndRun.Character.State
{
    public class FallState : BaseState
    {
        public FallState(MbCharacter character, Animator animator) : base(character, animator)
        {
        }

        public override void OnEnter()
        {
            _character.OnDead?.Invoke(_character);
            _character.transform.SetParent(null, true);
            _animator.CrossFade(FallHash, CrossFadeDuration);
        }

        public override void Update()
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.shortNameHash == FallHash && stateInfo.normalizedTime >= 1f)
            {
                MbCharacterSpawner.Instance.Despawn(_character);
            }
        }
    }
}

