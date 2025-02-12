using System;
using Thai;
using UnityEngine;

namespace Modifiers
{
    public class ModifierAmmo : ModifierBase
    {
        [SerializeField] private int ammoMultiplier = 2; // Số lần nhân số lượng đạn (2 hoặc 3)
        [SerializeField] private ModifierView modifierView;

        private void Start()
        {
            var isPositive = ammoMultiplier > 1; // Kiểm tra nếu là nhân đạn (x2, x3)
            string displayText = "";

            if (ammoMultiplier == 2)
            {
                displayText = "x2";
            }
            else if (ammoMultiplier == 3)
            {
                displayText = "x3";
            }
            else
            {
                displayText = ammoMultiplier > 0 ? "+" + ammoMultiplier : "" + ammoMultiplier;
            }

            modifierView.SetVisuals(isPositive, ammoMultiplier);
            modifierView.SetAmountText(displayText);
        }

        public override void Modify(PlayerController playerController)
        {
            var playerCrowd = playerController.GetComponent<PlayerCrowd>();
            
            foreach (var shooter in playerCrowd.Shooters)
            {
                shooter.UpgradeAmmo(ammoMultiplier); 
            }

            // In ra log
            if (ammoMultiplier > 1)
            {
                Debug.Log($"x{ammoMultiplier} ammo.");
            }
        }
    }
}
