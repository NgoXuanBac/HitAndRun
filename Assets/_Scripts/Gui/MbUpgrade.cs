using DG.Tweening;
using TMPro;
using UnityEngine;

namespace HitAndRun.Gui
{
    public class MbUpgrade : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _arrowUp;
        [SerializeField]
        private TMP_Text _curText;
        [SerializeField]
        private TMP_Text _nextText;

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

