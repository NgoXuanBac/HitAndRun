using System;
using UnityEngine;
using HitAndRun.Gate.Player;
using HitAndRun.Character;

namespace HitAndRun.Gate.Modifier
{
    public class ModifierAmmo : MbModifierBase
    {
        [SerializeField] private int ammoMultiplier = 2; // Số lần nhân số lượng đạn (2 hoặc 3)
        [SerializeField] private MbModifierView modifierView;

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

        public override void Modify(MbPlayerCollision playerController)
        {
            var playerCrowd = playerController.GetComponent<MbCharacter>();
            
            // foreach (var shooter in playerCrowd.Shooters)
            // {
            //     shooter.UpgradeAmmo(ammoMultiplier); 
            // }

            // In ra log
            if (ammoMultiplier > 1)
            {
                Debug.Log($"x{ammoMultiplier} ammo.");
            }
        }
    }
}
