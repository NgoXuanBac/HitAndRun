using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace HitAndRun.UpgradePower
{
    public class MbFreeUpgradeADS : MonoBehaviour
    {
        public MbPlayerStats playerStats;
        public TMP_Text curr;
        public TMP_Text up;
        void Start()
        {
            UpdateDamageUI();
            UpdateFireRateUI();
        }

        public void FreeUpgradeDamage()
        {
            playerStats.damage++;
            Debug.Log($"Run ADS complete! Damage: {playerStats.damage}");
            UpdateDamageUI();
        }

        public void FreeUpgradeFireRate()
        {
            playerStats.fireRate++;
            Debug.Log($"Run ADS complete! FireRate: {playerStats.fireRate}");
            UpdateFireRateUI();
        }

        void UpdateDamageUI()
        {
            curr.text = playerStats.damage.ToString();
            up.text = (playerStats.damage + 1).ToString();
        }
        void UpdateFireRateUI()
        {
            curr.text = playerStats.fireRate.ToString();
            up.text = (playerStats.fireRate + 1).ToString();
        }
    }
}
