using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace HitAndRun.Gui
{
    public class MbMenuManager : MonoBehaviour
    {
        public float fadeTime = 1f;
        public CanvasGroup canvasGroup;
        public RectTransform rectTransform;

        [SerializeField] private GameObject Setting;
        [SerializeField] private GameObject MainMenu;

        [SerializeField] private float settingTweenTime = 0.3f;
        // [SerializeField] private float waitTimeBeforeShow = 0.1f;

        private bool isSettingActive = false;
        private bool isTouchBlocked = false;
        private bool _hasStarted = false;
        private void Start()
        {
            if (Setting != null)
            {
                Setting.SetActive(false);
            }
        }

        protected virtual void Reset()
        {
            fadeTime = 1f;

            Transform settingTransform = transform.Find("Setting");
            if (settingTransform != null)
            {
                canvasGroup = settingTransform.GetComponent<CanvasGroup>();
                rectTransform = settingTransform.GetComponent<RectTransform>();
                Setting = settingTransform.gameObject;
                Debug.Log("Successfully assigned the 'Setting' object.");
            }
            else
            {
                Debug.LogWarning("Could not find a child object named 'Setting'. Make sure it's a child of the object this script is attached to.");
            }

            Transform mainMenuTransform = transform.Find("MainMenu");
            if (mainMenuTransform != null)
            {
                MainMenu = mainMenuTransform.gameObject;
                Debug.Log("Successfully assigned the 'MainMenu' object.");
            }
            else
            {
                Debug.LogWarning("Could not find a child object named 'MainMenu'. Make sure it's a child of the object this script is attached to.");
            }
        }

        public void ToggleSetting()
        {
            if (isSettingActive)
            {
                HideSetting();
            }
            else
            {
                ShowSetting();
            }
        }
        public void ShowSetting()
        {
            Time.timeScale = 0f;
            StartCoroutine(TweenShowPanel(Setting));
            isSettingActive = true;
            isTouchBlocked = true;
        }

        public void HideSetting()
        {
            Time.timeScale = 1f;
            isTouchBlocked = false;
            StartCoroutine(TweenHidePanel(Setting));
            isSettingActive = false;
        }

        private IEnumerator TweenShowPanel(GameObject targetPanel)
        {
            targetPanel.SetActive(true);
            targetPanel.transform.localScale = Vector3.zero;

            targetPanel.transform.DOScale(Vector3.one, settingTweenTime)
                .SetEase(Ease.OutElastic)
                .SetUpdate(true);  

            yield return null;
        }

        private IEnumerator TweenHidePanel(GameObject targetPanel)
        {
            targetPanel.transform.DOScale(Vector3.zero, settingTweenTime)
                .SetEase(Ease.InBack)
                .OnComplete(() => targetPanel.SetActive(false))
                .SetUpdate(true);  
            yield return null;
        }

        private void Update()
        {
            if (isTouchBlocked)
            {
                return;
            }

            var touches = InputHelper.GetTouches();
            if (touches.Count == 0) return;

            var touch = touches[0];

            if (!_hasStarted && touch.phase == TouchPhase.Began)
            {
                _hasStarted = true;
            }
        }

    }
}
