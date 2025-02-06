using HitAndRun.Attributes;
using TMPro;
using UnityEngine;

namespace HitAndRun.Character
{
    public class MbLevel : MonoBehaviour
    {
        [SerializeField, ReadOnly, Min(2)] private int _value;
        [SerializeField] private TextMeshPro _textMeshPro;
        [SerializeField] private SOBodyTypes _bodyTypes;
        [SerializeField] SkinnedMeshRenderer _skinnedMeshRenderer;
        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                _textMeshPro.text = FormatNumber(value);

                if (!Application.isPlaying) return;
                _skinnedMeshRenderer.material.SetColor("_BaseColor", _bodyTypes.GetColorByLevel(_value));
            }
        }

        private void Reset()
        {
            _bodyTypes ??= Resources.Load<SOBodyTypes>("Scriptables/BodyTypes");
            _textMeshPro ??= GetComponentInChildren<TextMeshPro>();
            _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            Value = 2;
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



