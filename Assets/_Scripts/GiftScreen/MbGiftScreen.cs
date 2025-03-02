using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace HitAndRun.GiftScreen
{
    public class MbGiftScreen : MonoBehaviour
    {
        [Header("UI Elements")]
        public GameObject giftScreen;
        public Slider progressBar;
        public TMP_Text ticketText;

        [Header("Game Data")]
        public Transform player;
        public float startDistance = 0f;
        public float endDistance = 50f;
        public int collectedTickets = 0;
        public int collectedCharacters = 0;
        public bool isWin = false;

        private float initialPositionX;
        private int totalTickets = 0;
        private bool gameEnded = false;

        void Start()
        {
            if (giftScreen != null)
                giftScreen.SetActive(false);

            if (player != null)
                initialPositionX = player.position.z;
        }

        void Update()
        {
            if (!gameEnded && (CheckGameOver() || isWin))
            {
                gameEnded = true;
                StartCoroutine(DisplayGiftScreen());
            }
            
            UpdateProgressBar();
        }

        void UpdateProgressBar()
        {
            if (progressBar == null || player == null) return;

            float traveledDistance = player.position.z - initialPositionX;
            float progress = Mathf.Clamp01(traveledDistance / endDistance);
            progressBar.value = progress;
        }

        IEnumerator DisplayGiftScreen()
        {
            yield return new WaitForSeconds(0.5f);
            ShowGiftScreen();
        }

        void ShowGiftScreen()
        {
            if (giftScreen != null)
            {
                giftScreen.SetActive(true);
                CalculateTickets();
            } 
        }

        void CalculateTickets()
        {
            float traveledDistance = player.position.z - initialPositionX;

            totalTickets = Mathf.FloorToInt(traveledDistance * 20)
                        + collectedTickets
                        + (collectedCharacters * 20);

            if (isWin)
            {
                totalTickets += 500;
            }

            if (ticketText != null)
            {
                ticketText.text = FormatTickets(totalTickets);
            }
        }

        bool CheckGameOver()
        {
            float traveledDistance = player.position.z - initialPositionX;
            return traveledDistance >= endDistance;
        }

        string FormatTickets(int tickets)
        {
            if (tickets >= 1000)
                return (tickets / 1000f).ToString("0.#") + "k";
            return tickets.ToString();
        }
    }
}
