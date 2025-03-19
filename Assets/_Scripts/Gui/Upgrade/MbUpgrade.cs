using DG.Tweening;
using TMPro;
using UnityEngine;

namespace HitAndRun.Gui.Upgrade
{
    public abstract class MbUpgrade : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _arrowUp;
        [SerializeField]
        protected TMP_Text _curText;
        [SerializeField]
        protected TMP_Text _nextText;
        protected virtual void Awake()
        {
            MbGameManager.Instance.OnDataLoaded += UpdateUI;
        }
        protected abstract void UpdateUI(GameData data);
        protected virtual void Start()
        {
            if (_arrowUp) _arrowUp.DOLocalMoveY(_arrowUp.localPosition.y + 10f, 0.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutBounce);
        }
        protected virtual void Reset()
        {
            _arrowUp = transform.Find("ArrowUp")?.GetComponent<RectTransform>();
            _curText = transform.Find("Cur")?.GetComponent<TextMeshProUGUI>();
            _nextText = transform.Find("Next")?.GetComponent<TextMeshProUGUI>();
        }
    }
}

