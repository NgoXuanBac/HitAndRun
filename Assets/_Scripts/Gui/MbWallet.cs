using TMPro;
using UnityEngine;


namespace HitAndRun.Gui
{
    public class MbWallet : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private void Reset()
        {
            _text = GetComponentInChildren<TMP_Text>();
        }

        private void Start()
        {
            MbGameManager.Instance.OnDataLoaded += UpdateUI;
        }

        private void UpdateUI(SaveData data)
        {
            _text.text = FormatNumber(data.Amount);
        }

        private string FormatNumber(long number)
        {
            if (number >= 1_000_000_000) return $"{number / 1_000_000_000f:0.#}B";
            if (number >= 1_000_000) return $"{number / 1_000_000f:0.#}M";
            if (number >= 1_000) return $"{number / 1_000f:0.#}K";

            return number.ToString();
        }
    }
}

