using UnityEngine;

namespace HitAndRun.Gui.Popup
{
    public abstract class MbPopup : MonoBehaviour
    {
        [SerializeField]
        protected Transform _content;

        protected virtual void Reset()
        {
            _content = transform.Find("Content");
        }

        public virtual void ShowPopup()
        {
            gameObject.SetActive(true);
        }

        public virtual void HidePopup()
        {
            gameObject.SetActive(false);
        }

        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }
    }

}
