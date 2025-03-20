using DG.Tweening;
using UnityEngine;

namespace HitAndRun.Gui.Popup
{
    public class MbSettingPopup : MbPopup
    {
        protected override void OnEnable()
        {
            _group.DOFade(1f, 0.3f).SetEase(Ease.OutElastic);
            _content.localScale = Vector3.zero;
            _content.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }
    }

}
