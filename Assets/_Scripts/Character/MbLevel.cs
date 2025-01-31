using HitAndRun.Attributes;
using TMPro;
using UnityEngine;

namespace HitAndRun.Character
{
    public class MbLevel : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _textMeshPro;
        [SerializeField, ReadOnly, Min(2)] private int _level;
        public int Level
        {
            get => _level;
            set
            {
                _level = value;
                _textMeshPro.text = FormatNumber(value);
            }
        }

        private void Reset()
        {
            _textMeshPro = GetComponentInChildren<TextMeshPro>();
            Level = 2;
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



