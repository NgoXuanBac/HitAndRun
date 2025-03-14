using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using HitAndRun;
namespace HitAndRun.Gui
{
    public class MbHandMovement : MonoBehaviour
    {
        public Button button;
        public GameObject tap;
        public float moveDuration = 1f;
        public float moveDistance = 100f;
        private RectTransform buttonRectTransform;
        private bool _hasStarted = false;
        void Start()
        {
            DOTween.SetTweensCapacity(2000, 50);
            buttonRectTransform = button.GetComponent<RectTransform>();
            MoveHand();
        }

        protected virtual void Reset()
        {
            Transform buttonTransform = transform.Find("Button01_Demo_Green");
            if (buttonTransform != null)
            {
                button = buttonTransform.GetComponent<Button>();
            }

            Transform tapTransform = transform.Find("Button01_Demo_Green/Tap");
            if (tapTransform != null)
            {
                tap = tapTransform.gameObject;
            }
            _hasStarted = false;
        }
        private void Update()
        {
            var touches = InputHelper.GetTouches();
            if (_hasStarted)
                button.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).OnKill(() => button.gameObject.SetActive(false));

            if (touches.Count == 0) return;
            var touch = touches[0];

            if (!_hasStarted && touch.phase == TouchPhase.Began)
                _hasStarted = true;
        }

        void MoveHand()
        {
            float startPos = -buttonRectTransform.rect.width / 2;
            float endPos = buttonRectTransform.rect.width / 2;

            tap.transform.DOLocalMoveX(endPos, moveDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.Linear);
        }

        public void HideButtonOnTap()
        {
            button.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).OnKill(() => button.gameObject.SetActive(false));
        }
    }
}