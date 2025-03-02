using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace HitAndRun.UpgradePower
{
    public class MbDamageUpgrade : MonoBehaviour
    {
        public MbPlayerStats playerStats;
        public TMP_Text currDamageText; 
        public TMP_Text upDDamageText; 
        public TMP_Text upgradeCostText; 
        public int damageCost = 5; 

        void Start()
        {
            UpdateDamageUI();
        }

        public void UpgradeDamage()
        {
            if (playerStats.tickets >= damageCost)
            {
                playerStats.SpendTicket(damageCost);
                playerStats.damage++;
                damageCost += 3; 
                Debug.Log($"Damage: {playerStats.fireRate}, Wallet Ticket: {playerStats.tickets}");
                UpdateDamageUI();
            }
        }

        void UpdateDamageUI()
        {
            currDamageText.text = playerStats.damage.ToString();
            upDDamageText.text = (playerStats.damage + 1).ToString();
            upgradeCostText.text = damageCost.ToString();
        }
    }
}
