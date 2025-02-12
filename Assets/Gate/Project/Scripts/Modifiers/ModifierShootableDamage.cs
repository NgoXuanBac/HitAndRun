using System;
using Thai;
using UnityEngine;

namespace Modifiers
{
    public class ModifierShootableDamage : ModifierBase
    {
        [SerializeField] private int damageToAdd = 2;
        [SerializeField] private ModifierView modifierView;

        private void Start()
        {
            var isPositive = damageToAdd > 0;
            modifierView.SetVisuals(isPositive, damageToAdd);
        }

        public override void Modify(PlayerController playerController)
        {
            var playerCrowd = playerController.GetComponent<PlayerCrowd>();
            playerCrowd.UpgradeDamageToCrowd(damageToAdd);
            Debug.Log($"{(damageToAdd >= 0 ? "+" : "-")}{damageToAdd} damages.");
        }
    }
}