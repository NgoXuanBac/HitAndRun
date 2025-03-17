using UnityEngine;
using UnityEngine.EventSystems;

namespace HitAndRun.Gui
{
    public class MbWaitPopup : MonoBehaviour, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            transform.gameObject.SetActive(false);
            MbGameManager.Instance.StartGame();
        }
    }

}
