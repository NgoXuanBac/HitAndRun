using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HitAndRun.Gui.Upgrade
{
    public abstract class MbUpgrade : MonoBehaviour
    {
        [SerializeField] protected Sprite _coinIcon;
        [SerializeField] protected Sprite _ytIcon;
        [SerializeField] private RectTransform _arrowUp;
        [SerializeField] protected TMP_Text _curText;
        [SerializeField] protected TMP_Text _nextText;
        [SerializeField] protected Image _icon;
        [SerializeField] protected TMP_Text _price;
        [SerializeField] protected Button _button;
        [SerializeField, Range(0f, 1f)] protected float _priceScale = 0.5f;
        [SerializeField, Range(10, 100)] protected int _priceBase = 10;
        protected virtual void Awake()
        {
            MbGameManager.Instance.OnDataLoaded += UpdateUI;
            _button.onClick.AddListener(HandleClick);
        }

        protected virtual void OnDestroy()
        {
            _button.onClick.RemoveListener(HandleClick);
        }

        protected abstract void UpdateUI(GameData data);
        protected abstract void HandleClick();
        protected virtual void Start()
        {
            if (_arrowUp) _arrowUp.DOLocalMoveY(_arrowUp.localPosition.y + 20f, 0.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutBounce);
        }
        protected virtual void Reset()
        {
            _button = GetComponent<Button>();
            _arrowUp = transform.Find("ArrowUp")?.GetComponent<RectTransform>();
            _curText = transform.Find("Cur")?.GetComponent<TextMeshProUGUI>();
            _nextText = transform.Find("Next")?.GetComponent<TextMeshProUGUI>();
            _icon = transform.Find("Icon")?.GetComponent<Image>();
            _price = transform.Find("Price")?.GetComponent<TextMeshProUGUI>();
            _coinIcon = Resources.Load<Sprite>("Textures/UI/Coin");
            _ytIcon = Resources.Load<Sprite>("Textures/UI/Youtube");
        }

        protected string FormatNumber(int number)
        {
            if (number >= 1_000_000_000) return $"{number / 1_000_000_000f:0.#}B";
            if (number >= 1_000_000) return $"{number / 1_000_000f:0.#}M";
            if (number >= 1_000) return $"{number / 1_000f:0.#}K";

            return number.ToString();
        }
    }
}

