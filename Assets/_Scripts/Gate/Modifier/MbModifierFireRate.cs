using HitAndRun.Character;

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
        private void Start()
        {
            var type = _modifierTypes.Types[Random.Range(0, _modifierTypes.Types.Count)];
            _modifierView.SetVisuals(_modifierTypes.Name, type.Color, type.Amount == 0 ? null : type.Amount.ToString(), type.Icon);
        }
        public override void Modify(MbCharacter character)
        {
        }


    }
}
