using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace HitAndRun.UpgradePower
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
            if (ticketText != null)
            {
                if (tickets >= 10000) 
                {
                    ticketText.text = (tickets / 1000f).ToString("0.#") + "k";
                }
                else
                {
                    ticketText.text = tickets.ToString();
                }
            }
        }
    }
}