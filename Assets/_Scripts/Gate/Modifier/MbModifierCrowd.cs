using HitAndRun.Character;
using UnityEngine;

namespace HitAndRun.Gate.Modifier
{
    public class MbModifierCrowd : MbModifierBase
    {
        [SerializeField] private int _characterAmount = 2;
        protected override void Reset()
        {
            base.Reset();
            _modifierType = Resources.Load<SOModifierTypes>("Scriptables/CrowdModifierTypes");
            _isPositive = false;
            ApplyVisuals(_characterAmount.ToString());
        }

        public override void Modify(MbCharacter character)
        {
            Debug.Log($"{(_characterAmount >= 0 ? "+" : "")}{_characterAmount} crowds.");
        }

    }
}

