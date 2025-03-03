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
            _character.OnDead?.Invoke(_character);
            var temp = _character.transform.position;
            _character.transform.SetParent(null, true);
            _character.transform.position = temp;
            _animator.CrossFade(DyingHash, CrossFadeDuration);
        }

        public override void Update()
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.shortNameHash == DyingHash && stateInfo.normalizedTime >= 1f)
            {
                MbCharacterSpawner.Instance.Despawn(_character);
            }
        }
    }
}