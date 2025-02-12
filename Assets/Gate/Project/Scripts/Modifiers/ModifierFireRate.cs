// using System;
// using Thai;
// using UnityEngine;

// namespace Modifiers
// {
//     public class ModifierFireRate : ModifierBase
//     {
//         [SerializeField] private int fireRateToAdd = 2;
//         [SerializeField] private ModifierView modifierView;

//         private void Start()
//         {
//             var isPositive = fireRateToAdd > 1;
//             modifierView.SetVisuals(isPositive, fireRateToAdd);
//         }

//         public override void Modify(PlayerController playerController)
//         {
//             var playerCrowd = playerController.GetComponent<PlayerCrowd>();
//             playerCrowd.UpgradeFireRateToCrowd(fireRateToAdd);
//             Debug.Log($"{(fireRateToAdd >= 1 ? "x" : "/")}{fireRateToAdd} firerate.");
//         }
//     }
// }


using System;
using Thai;
using UnityEngine;

namespace Modifiers
{
    public class ModifierFireRate : ModifierBase
    {
        [SerializeField] private int fireRateToAdd = 2;
        [SerializeField] private ModifierView modifierView;

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

        public override void Modify(PlayerController playerController)
        {
            var playerCrowd = playerController.GetComponent<PlayerCrowd>();
            
            if (fireRateToAdd > 0)
            {
                playerCrowd.UpgradeFireRateToCrowd(fireRateToAdd);
                Debug.Log($"x{fireRateToAdd} firerate.");
            }
            else if (fireRateToAdd < 0)
            {
                playerCrowd.UpgradeFireRateToCrowd(fireRateToAdd);
                Debug.Log($"/{Mathf.Abs(fireRateToAdd)} firerate.");
            }
        }
    }
}
