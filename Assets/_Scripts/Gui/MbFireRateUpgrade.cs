using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace HitAndRun.Gui
{
    public class MbFireRateUpgrade : MonoBehaviour
    {
        public MbPlayerStats playerStats;
        public TMP_Text currFireRateText;
        public TMP_Text upFireRateText;
        public TMP_Text upgradeCostText;
        public int fireRateCost = 5;
        public GameObject arrowUp;
        void Start()
        {
            UpdateFireRateUI();
            AnimateArrowUpAtStart();
        }

        protected virtual void Reset()
        {
            GameObject ticketsObject = GameObject.Find("Tickets");

            if (ticketsObject != null)
            {
                playerStats = ticketsObject.GetComponent<MbPlayerStats>();
                currFireRateText = transform.Find("CurPower")?.GetComponent<TextMeshProUGUI>();
                upFireRateText = transform.Find("UpPower")?.GetComponent<TextMeshProUGUI>();
                upgradeCostText = transform.Find("Ticket")?.GetComponent<TextMeshProUGUI>();
                arrowUp = transform.Find("ArrowUp")?.gameObject;
            }
            else
            {
                Debug.LogWarning("Tickets object not found!");
            }
        }

        public void UpgradeFireRate()
        {
            if (playerStats.tickets >= fireRateCost)
            {
                playerStats.SpendTicket(fireRateCost);
                playerStats.fireRate++;
                fireRateCost += 10;
                Debug.Log($"FireRate: {playerStats.fireRate}, Wallet Ticket: {playerStats.tickets}");
                UpdateFireRateUI();
                AnimateArrowUp();
            }
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

        void UpdateFireRateUI()
        {
            currFireRateText.text = FormatNumber(playerStats.fireRate);
            upFireRateText.text = FormatNumber(playerStats.fireRate + 1);
            upgradeCostText.text = FormatNumber(fireRateCost);

            float baseFontSize = 300f;
            float baseCostFontSize = 150f;
            float minFontSize = 200f;
            float fontSizeReductionRate = 3f;

            float newFontSize = Mathf.Max(baseFontSize - playerStats.damage * fontSizeReductionRate, minFontSize);
            float newCostFontSize = Mathf.Max(baseCostFontSize - playerStats.damage * fontSizeReductionRate, minFontSize);

            currFireRateText.fontSize = newFontSize;
            upFireRateText.fontSize = newFontSize;
            upgradeCostText.fontSize = newCostFontSize;
        }

        private string FormatNumber(int number)
        {
            if (number >= 1_000_000_000) return $"{number / 1_000_000_000f:0.#}B";
            if (number >= 1_000_000) return $"{number / 1_000_000f:0.#}M";
            if (number >= 1_000) return $"{number / 1_000f:0.#}K";

            return number.ToString();
        }
    }
}
