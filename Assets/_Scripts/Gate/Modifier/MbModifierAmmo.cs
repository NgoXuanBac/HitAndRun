using UnityEngine;
using HitAndRun.Character;
using HitAndRun.Bullet;

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
            character.ShootingPattern = new SpreadShot();
        }


    }
}
