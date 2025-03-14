using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace HitAndRun.Gui
{
    public class MbPlayerStats : MonoBehaviour
    {
        public int tickets = 0;
        public int fireRate = 1;
        public int damage = 1;

        public TMP_Text ticketText;

        void Start()
        {
            UpdateTicketUI();
        }

        protected virtual void Reset()
        {

            Transform settingTransform = transform.Find("Ticket_Count");
            if (settingTransform != null)
            {
                ticketText = settingTransform.GetComponent<TMP_Text>();
                Debug.Log("Successfully assigned the 'Setting' object.");
            }
            else
            {
                Debug.LogWarning("Could not find a child object named 'Setting'. Make sure it's a child of the object this script is attached to.");
            }
        }
        public void CollectTicket(int amount)
        {
            tickets += amount;
            UpdateTicketUI();
        }

        public void SpendTicket(int amount)
        {
            if (tickets >= amount)
            {
                tickets -= amount;
                UpdateTicketUI();
            }
        }

        void UpdateTicketUI()
        {
            ticketText.text = FormatNumber(tickets);

            float baseFontSize = 100f; 
            float minFontSize = 100f;   
            float fontSizeReductionRate = 3f; 

            float newFontSize = Mathf.Max(baseFontSize - tickets * fontSizeReductionRate, minFontSize);

            ticketText.fontSize = newFontSize;
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