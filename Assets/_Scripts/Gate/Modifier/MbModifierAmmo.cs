using UnityEngine;
using HitAndRun.Character;

namespace HitAndRun.Gate.Modifier
{
    public class MbModifierAmmo : MbModifierBase
    {
        [SerializeField] private int _ammoMultiplier = 2;


        public override void Modify(MbCharacter character)
        {
            if (_ammoMultiplier > 1)
            {
                Debug.Log($"x{_ammoMultiplier} ammo.");
            }
        }
    }
}
