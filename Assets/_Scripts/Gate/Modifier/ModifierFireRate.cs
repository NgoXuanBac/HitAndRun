using System;
using HitAndRun.Gate.Player;
using HitAndRun.Character;

using UnityEngine;

namespace HitAndRun.Gate.Modifier
{
    public class ModifierFireRate : MbModifierBase
    {
        [SerializeField] private int fireRateToAdd = 2;
        [SerializeField] private MbModifierView modifierView;

        private void Start()
        {
            var isPositive = fireRateToAdd > 0; 
            string displayText = "";

            if (fireRateToAdd == 2)
            {
                displayText = "x2";
            }
            else if (fireRateToAdd == -2)
            {
                displayText = "/2";
            }
            else
            {
                displayText = fireRateToAdd > 0 ? "+" + fireRateToAdd : "" + fireRateToAdd;
            }

            modifierView.SetVisuals(isPositive, fireRateToAdd);
            modifierView.SetAmountText(displayText);
        }

        public override void Modify(MbPlayerCollision playerController)
        {
            var playerCrowd = playerController.GetComponent<MbCharacter>();
            
            if (fireRateToAdd > 0)
            {
                // playerCrowd.UpgradeFireRateToCrowd(fireRateToAdd);
                Debug.Log($"x{fireRateToAdd} firerate.");
            }
            else if (fireRateToAdd < 0)
            {
                // playerCrowd.UpgradeFireRateToCrowd(fireRateToAdd);
                Debug.Log($"/{Mathf.Abs(fireRateToAdd)} firerate.");
            }
        }
    }
}
