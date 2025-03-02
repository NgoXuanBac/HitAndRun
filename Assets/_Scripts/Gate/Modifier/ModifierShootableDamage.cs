using System;
using HitAndRun.Gate.Player;
using HitAndRun.Character;
using UnityEngine;

namespace HitAndRun.Gate.Modifier
{
    public class ModifierShootableDamage : MbModifierBase
    {
        [SerializeField] private int damageToAdd = 2;
        [SerializeField] private MbModifierView modifierView;

        private void Start()
        {
            var isPositive = damageToAdd > 0;
            modifierView.SetVisuals(isPositive, damageToAdd);
        }

        public override void Modify(MbPlayerCollision playerController)
        {
            var playerCrowd = playerController.GetComponent<MbCharacter>();
            // playerCrowd.UpgradeDamageToCrowd(damageToAdd);
            Debug.Log($"{(damageToAdd >= 0 ? "+" : "-")}{damageToAdd} damages.");
        }
    }
}