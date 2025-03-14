using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
namespace HitAndRun.Gui
{
    public class MbDamageUpgrade : MonoBehaviour
    {
        public MbPlayerStats playerStats;
        public TMP_Text currDamageText;
        public TMP_Text upDDamageText;
        public TMP_Text upgradeCostText;
        public int damageCost = 5;
        public GameObject arrowUp;
        void Start()
        {
            UpdateDamageUI();
            AnimateArrowUpAtStart();
        }

        protected virtual void Reset()
        {
            GameObject ticketsObject = GameObject.Find("Tickets");

            if (ticketsObject != null)
            {
                playerStats = ticketsObject.GetComponent<MbPlayerStats>();
                currDamageText = transform.Find("CurPower")?.GetComponent<TextMeshProUGUI>();
                upDDamageText = transform.Find("UpPower")?.GetComponent<TextMeshProUGUI>();
                upgradeCostText = transform.Find("Ticket")?.GetComponent<TextMeshProUGUI>();
                arrowUp = transform.Find("ArrowUp")?.gameObject;
            }
            else
            {
                Debug.LogWarning("Tickets object not found!");
            }
        }

        public void UpgradeDamage()
        {
            if (playerStats.tickets >= damageCost)
            {
                playerStats.SpendTicket(damageCost);
                playerStats.damage++;
                damageCost += 10;
                Debug.Log($"Damage: {playerStats.fireRate}, Wallet Ticket: {playerStats.tickets}");
                UpdateDamageUI();

                AnimateArrowUp();
            }
        }

        void UpdateDamageUI()
        {
            currDamageText.text = FormatNumber(playerStats.damage);
            upDDamageText.text = FormatNumber(playerStats.damage + 1);
            upgradeCostText.text = FormatNumber(damageCost);

            float baseFontSize = 300f;
            float baseCostFontSize = 150f;
            float minFontSize = 200f;
            float fontSizeReductionRate = 3f;

            float newFontSize = Mathf.Max(baseFontSize - playerStats.damage * fontSizeReductionRate, minFontSize);
            float newCostFontSize = Mathf.Max(baseCostFontSize - playerStats.damage * fontSizeReductionRate, minFontSize);

            currDamageText.fontSize = newFontSize;
            upDDamageText.fontSize = newFontSize;
            upgradeCostText.fontSize = newCostFontSize;
        }

        private string FormatNumber(int number)
        {
            if (number >= 1_000_000_000) return $"{number / 1_000_000_000f:0.#}B";
            if (number >= 1_000_000) return $"{number / 1_000_000f:0.#}M";
            if (number >= 1_000) return $"{number / 1_000f:0.#}K";

            return number.ToString();
        }
        void AnimateArrowUp()
        {
            if (arrowUp != null)
            {
                arrowUp.transform.DOPunchPosition(new Vector3(0, 20, 0), 0.5f, 10, 1f).SetEase(Ease.InOutBounce); 
            }
        }

        void AnimateArrowUpAtStart()
        {
            if (arrowUp != null)
            {
                arrowUp.transform.DOPunchPosition(new Vector3(0, 20, 0), 0.5f, 0, 0f).SetLoops(-1, LoopType.Yoyo); 
            }
        }
    }
}
