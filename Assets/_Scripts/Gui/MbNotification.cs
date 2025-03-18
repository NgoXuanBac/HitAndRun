using DG.Tweening;
using TMPro;
using UnityEngine;

namespace HitAndRun.Gui
{
    public class MbNotification : MbSingleton<MbNotification>
    {
        [SerializeField] private Color _positive;
        [SerializeField] private Color _negative;
        [SerializeField] private TMP_Text _content;
        [SerializeField] private float _duration = 1f;
        private void Reset()
        {
            _content = GetComponentInChildren<TMP_Text>();
            _content.gameObject.SetActive(false);
        }

        public void Show(string message, bool positive = true)
        {
            if (_content.IsActive()) _content.DOKill();

            _content.gameObject.SetActive(true);

            _content.color = positive ? _positive : _negative;
            _content.text = message;

            _content.DOFade(0f, _duration).From(1).SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                _content.gameObject.SetActive(false);
            });

        }
    }
}

