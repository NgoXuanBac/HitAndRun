using DG.Tweening;
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

    }

}
