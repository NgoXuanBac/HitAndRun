using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
namespace HitAndRun.Gui
{
    public class MbFreeUpgradeADS : MonoBehaviour
    {
        public MbPlayerStats playerStats;
        public TMP_Text curr;
        public TMP_Text up;

        public GameObject arrowUp;
        void Start()
        {
            UpdateDamageUI();
            UpdateFireRateUI();
            AnimateArrowUpAtStart();
        }

        protected virtual void Reset()
        {
            GameObject ticketsObject = GameObject.Find("Tickets");

            if (ticketsObject != null)
            {
                playerStats = ticketsObject.GetComponent<MbPlayerStats>();
                curr = transform.Find("CurPower")?.GetComponent<TextMeshProUGUI>();
                up = transform.Find("UpPower")?.GetComponent<TextMeshProUGUI>();
                arrowUp = transform.Find("ArrowUp")?.gameObject;
            }
            else
            {
                Debug.LogWarning("Tickets object not found!");
            }
        }


        public void FreeUpgradeDamage()
        {
            playerStats.damage++;
            Debug.Log($"Run ADS complete! Damage: {playerStats.damage}");
            UpdateDamageUI();
            AnimateArrowUp();
        }

        public void FreeUpgradeFireRate()
        {
            playerStats.fireRate++;
            Debug.Log($"Run ADS complete! FireRate: {playerStats.fireRate}");
            UpdateFireRateUI();
            AnimateArrowUp();
        }

        void UpdateDamageUI()
        {
            curr.text = FormatNumber(playerStats.damage);
            up.text = FormatNumber(playerStats.damage + 1);

            // Tính toán cỡ chữ
            float baseFontSize = 300f; 
            float minFontSize = 200f;   
            float fontSizeReductionRate = 3f; 

            float newFontSize = Mathf.Max(baseFontSize - playerStats.damage * fontSizeReductionRate, minFontSize);

            // Cập nhật cỡ chữ
            curr.fontSize = newFontSize;
            up.fontSize = newFontSize;
        }

        void UpdateFireRateUI()
        {
            curr.text = FormatNumber(playerStats.fireRate);
            up.text = FormatNumber(playerStats.fireRate + 1);

            // Tính toán cỡ chữ
            float baseFontSize = 300f; 
            float minFontSize = 200f;   
            float fontSizeReductionRate = 3f; 

            float newFontSize = Mathf.Max(baseFontSize - playerStats.fireRate * fontSizeReductionRate, minFontSize);

            curr.fontSize = newFontSize;
            up.fontSize = newFontSize;
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
