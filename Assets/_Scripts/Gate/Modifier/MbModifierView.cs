using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HitAndRun.Gate.Modifier {
    public class MbModifierView : MonoBehaviour
    {
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Renderer cylinder;
        [SerializeField] private Renderer cylinder2;
        [SerializeField] private Renderer quad;
        [SerializeField] private TMP_Text amountText;
        [SerializeField] private Color positiveColor;
        [SerializeField] private Color negativeColor;
        [SerializeField] private SpriteRenderer positiveStatusImage;
        [SerializeField] private SpriteRenderer negativeStatusImage;
        
        public void SetVisuals(bool isPositive, int amount)
        {
            backgroundImage.color = isPositive ? positiveColor : negativeColor;
            cylinder.material.color = isPositive? positiveColor : negativeColor;
            cylinder2.material.color = isPositive? positiveColor : negativeColor;
            quad.material.color = isPositive? positiveColor : negativeColor;
            amountText.text = isPositive ? "+" + amount : "" + amount;
            if (isPositive) {
                positiveStatusImage.enabled = true;
                negativeStatusImage.enabled = false;
            } else{
                negativeStatusImage.enabled = true;
                positiveStatusImage.enabled = false;
            }
        }
        public void SetAmountText(string text)
        {
            amountText.text = text;
        }
    }
}
