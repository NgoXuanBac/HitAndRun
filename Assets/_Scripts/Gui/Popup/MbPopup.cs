using DG.Tweening;
using UnityEngine;

namespace HitAndRun.Gui.Popup
{
    public abstract class MbPopup : MonoBehaviour
    {
        [SerializeField]
        protected Transform _content;
        [SerializeField]
        protected CanvasGroup _group;

        protected virtual void Reset()
        {
            _content = transform.Find("Content");
            _group = GetComponent<CanvasGroup>();
        }

        public virtual void ShowPopup()
        {
            gameObject.SetActive(true);

        }

        public virtual void HidePopup()
        {
            gameObject.SetActive(false);
        }

        protected virtual void OnEnable()
        {
        }
        protected virtual void OnDisable()
        {
            _group?.DOKill();
            _content?.DOKill();
        }
    }

}
