using UnityEngine;
using UnityEngine.EventSystems;

namespace HitAndRun.Gui.Popup
{
    public class MbWaitPopup : MbPopup, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            HidePopup();
            MbGameManager.Instance.StartGame();
        }
    }

}
