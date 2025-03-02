using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
namespace HitAndRun.DistanceProgressBar
{
    public class DistanceProgressBar : MonoBehaviour
    {
        public Slider progressBar; 
        public Transform player;  
        public float startDistance = 0f; 
        public float endDistance = 999f; 
        public float animationSpeed = 2f; 

        public TMP_Text startText;
        public TMP_Text endText;

        private float initialPositionX;
        private float targetProgress = 0f;

        void Start()
        {
            if (progressBar == null || player == null)
            {
                Debug.LogError("Not found Slide/Player!");
                return;
            }

            initialPositionX = player.position.z;

            progressBar.minValue = 0;
            progressBar.maxValue = 1;
            progressBar.value = 0;
        }

        void Update()
        {
            if (progressBar == null || player == null) return;

            float traveledDistance = player.position.z - initialPositionX;

            targetProgress = Mathf.Clamp01((traveledDistance - startDistance) / (endDistance - startDistance));

            progressBar.value = Mathf.Lerp(progressBar.value, targetProgress, Time.deltaTime * animationSpeed);

            startText.text = startDistance.ToString("0m");
            endText.text = endDistance.ToString("0m");
            Debug.Log($"Road: {traveledDistance}m - Progress: {targetProgress * 100}%");
        }
    }
}

