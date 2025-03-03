using HitAndRun.Character;

using UnityEngine;

namespace HitAndRun.Gate.Modifier
{
    public class MbModifierFireRate : MbModifierBase
    {
        [SerializeField] private int _fireRate = 2;

        protected override void Reset()
        {
            base.Reset();
            _modifierType = Resources.Load<SOModifierTypes>("Scriptables/FireRateModifierTypes");
            _isPositive = true;
            ApplyVisuals(_fireRate.ToString());
        }
        public override void Modify(MbCharacter character)
        {
            if (_fireRate > 0)
            {
                Debug.Log($"x{_fireRate} firerate.");
            }
            else if (_fireRate < 0)
            {
                Debug.Log($"/{Mathf.Abs(_fireRate)} firerate.");
            }
        }


    }
}
