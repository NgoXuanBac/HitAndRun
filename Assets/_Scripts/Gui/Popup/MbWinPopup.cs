using DG.Tweening;
using HitAndRun.Ads;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HitAndRun.Gui.Popup
{
    public class MbWinPopup : MbPopup
    {
        [SerializeField] private Button _claimBtn;
        [SerializeField] private Button _claimX2Btn;
        [SerializeField] private TMP_Text _total;
        [SerializeField] private TMP_Text _level;
        [SerializeField, Range(10, 100)] private long _coinNum = 10;
        [SerializeField, Range(0.1f, 2)] private float _coinScale = 1f;


        protected override void Reset()
        {
            base.Reset();
            var buttons = GetComponentsInChildren<Button>(true);

            foreach (Button btn in buttons)
            {
                if (btn.name == "Claim")
                    _claimBtn = btn;
                else if (btn.name == "ClaimX2")
                    _claimX2Btn = btn;

                if (_claimBtn != null && _claimX2Btn != null)
                    break;
            }

            _total = _content.Find("Total").GetComponent<TMP_Text>();
            _level = _content.Find("Level").GetComponent<TMP_Text>();
        }

        protected override void OnEnable()
        {
            _total.text = "x" + FormatNumber(_coinNum * (int)(1 + MbGameManager.Instance.Data.Level * _coinScale));
            _claimBtn.onClick.AddListener(HandleClaim);
            _claimX2Btn.onClick.AddListener(HandleX2Claim);
            _level.text = MbGameManager.Instance.Data.Level.ToString();

            _group.DOFade(1f, 0.3f).SetEase(Ease.OutElastic);
            _content.localScale = Vector3.zero;
            _content.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
        }

        protected override void OnDisable()
        {
            _claimBtn.onClick.RemoveListener(HandleClaim);
            _claimX2Btn.onClick.RemoveListener(HandleX2Claim);
        }

        private string FormatNumber(long number)
        {
            if (number >= 1_000_000_000) return $"{number / 1_000_000_000f:0.#}B";
            if (number >= 1_000_000) return $"{number / 1_000_000f:0.#}M";
            if (number >= 1_000) return $"{number / 1_000f:0.#}K";

            return number.ToString();
        }

        private void HandleClaim()
        {
            MbGameManager.Instance.AddCoin(_coinNum);
            MbGameManager.Instance.NextLevel();
            HidePopup();
        }

        private void HandleX2Claim()
        {
            MbRewardAds.Instance.ShowRewardedAd(() => MbGameManager.Instance.AddCoin(_coinNum * 2),
                () =>
                {
                    HidePopup();
                    MbGameManager.Instance.NextLevel();
                }
            );
        }

    }

}
