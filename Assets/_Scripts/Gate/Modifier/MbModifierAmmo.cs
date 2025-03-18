using UnityEngine;
using HitAndRun.Character;
using HitAndRun.Bullet;
using HitAndRun.Gui;

namespace HitAndRun.Gate.Modifier
{
    public class MbModifierAmmo : MbModifierBase
    {

        protected override void Reset()
        {
            base.Reset();
            _modifierTypes = Resources.Load<SOModifierTypes>("Scriptables/AmmoModifierTypes");
        }
        public override void Modify(MbCharacter character)
        {
            if (character.ShootingPattern is not SpreadShot)
            {
                character.ShootingPattern = new SpreadShot();
            }

            MbNotification.Instance.Show("Spread Shot", _modifierType.Value.Category == ModifierCategory.Positive);
        }


    }
}
