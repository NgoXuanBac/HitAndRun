using HitAndRun.Character;
using UnityEngine;

namespace HitAndRun.Gate.Modifier
{
    public class MbModifierDamage : MbModifierBase
    {
        [SerializeField] private int _damageAmount = 2;

        protected override void Reset()
        {
            base.Reset();
            _modifierType = Resources.Load<SOModifierTypes>("Scriptables/DamageModifierTypes");
            _isPositive = true;
        }

        private void Start()
        {
            ApplyVisuals($"{(_damageAmount >= 0 ? "+" : "")}{_damageAmount}");
        }

        public override void Modify(MbCharacter character)
        {
            Debug.Log($"{(_damageAmount >= 0 ? "+" : "")}{_damageAmount} damages.");
        }
    }
}