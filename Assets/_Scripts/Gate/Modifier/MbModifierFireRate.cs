using HitAndRun.Character;
using HitAndRun.Gui;
using UnityEngine;

namespace HitAndRun.Gate.Modifier
{
    public class MbModifierFireRate : MbModifierBase
    {
        protected override void Reset()
        {
            base.Reset();
            _modifierTypes = Resources.Load<SOModifierTypes>("Scriptables/FireRateModifierTypes");
        }
        public override void Modify(MbCharacter character)
        {
            if (_modifierType == null) return;

            MbNotification.Instance.Show($"{(_modifierType.Value.Amount > 0 ? "+" : "") + _modifierType.Value.Amount.ToString()} Fire Rate",
                _modifierType.Value.Category == ModifierCategory.Positive);

            if (character.FireRate <= 2 && _modifierType.Value.Amount < 0) return;
            character.FireRate += _modifierType.Value.Amount;
        }


    }
}
