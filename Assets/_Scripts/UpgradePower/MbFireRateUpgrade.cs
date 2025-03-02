using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace HitAndRun.UpgradePower
{
    public class MbFireRateUpgrade : MonoBehaviour
    {
        public MbPlayerStats playerStats;
        public TMP_Text currFireRateText; 
        public TMP_Text upFireRateText; 
        public TMP_Text upgradeCostText; 
        public int fireRateCost = 5; 

        void Start()
        {
            UpdateFireRateUI();
        }

        public void UpgradeFireRate()
        {
            if (playerStats.tickets >= fireRateCost)
            {
                playerStats.SpendTicket(fireRateCost);
                playerStats.fireRate++;
                fireRateCost += 2; 
                Debug.Log($"FireRate: {playerStats.fireRate}, Wallet Ticket: {playerStats.tickets}");
                UpdateFireRateUI();
            }
        }

        void UpdateFireRateUI()
        {
            currFireRateText.text = playerStats.fireRate.ToString();
            upFireRateText.text = (playerStats.fireRate + 1).ToString();
            upgradeCostText.text = fireRateCost.ToString();
        }
    }
}
