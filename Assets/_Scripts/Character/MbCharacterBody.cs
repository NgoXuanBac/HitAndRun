using DG.Tweening;
using TMPro;
using UnityEngine;

namespace HitAndRun.Character
{
    public class MbCharacterBody : MonoBehaviour
    {
        [SerializeField, ReadOnly, Min(2)] private int _level;
        [SerializeField] private TextMeshPro _textMeshPro;
        [SerializeField] private SOBodyTypes _bodyTypes;
        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
        [SerializeField, Range(0, 100)] private int _scaleUp;
        [SerializeField, ReadOnly] private float _radius;
        public float Width => _radius * transform.localScale.x * 2;
        public int Level
        {
            get => _level;
            set
            {
                _level = value;
                _textMeshPro.text = FormatNumber(value);

                if (!Application.isPlaying) return;
                _skinnedMeshRenderer.material.SetColor("_BaseColor", _bodyTypes.GetColorByLevel(_level));
                var scale = transform.localScale + (Mathf.Log(_level, 2) - 1) * 0.01f * _scaleUp * Vector3.one;
                transform.DOScale(scale, 0.3f).SetEase(Ease.OutQuad);
            }
        }

        private void Reset()
        {
            _bodyTypes ??= Resources.Load<SOBodyTypes>("Scriptables/BodyTypes");
            _textMeshPro ??= GetComponentInChildren<TextMeshPro>();
            _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            _radius = GetComponent<CapsuleCollider>().radius;
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



