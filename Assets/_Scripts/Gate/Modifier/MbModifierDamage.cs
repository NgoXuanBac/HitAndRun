using HitAndRun.Character;
using UnityEngine;

namespace HitAndRun.Gate.Modifier
{
    public class MbModifierDamage : MbModifierBase
    {
        protected override void Reset()
        {
            base.Reset();
            _modifierTypes = Resources.Load<SOModifierTypes>("Scriptables/DamageModifierTypes");
        }
        public override void Modify(MbCharacter character)
        {
            if (_modifierType == null) return;
            if (character.Damage <= 2 && _modifierType.Value.Amount < 0) return;
            character.Damage += _modifierType.Value.Amount;
        }
    }
}