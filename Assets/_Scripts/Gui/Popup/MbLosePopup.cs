using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HitAndRun.Gui.Popup
{
    public class MbLosePopup : MbPopup, IPointerClickHandler
    {
        [SerializeField] private Transform _retry;
        protected override void Reset()
        {
            base.Reset();
            _retry = _content.Find("Retry");
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            HidePopup();
            MbGameManager.Instance.Restart();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _retry.DOScale(1.2f, 0.5f).SetLoops(-1, LoopType.Yoyo).Play();

            _group.DOFade(1f, 0.3f).SetEase(Ease.OutElastic);
            _content.localScale = Vector3.zero;
            _content.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _retry.localScale = Vector3.one;
            _retry?.DOKill();
        }
    }

}
