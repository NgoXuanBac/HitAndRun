using DG.Tweening;
using HitAndRun.Audio;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace HitAndRun.Gui.Popup
{
    public class MbSettingPopup : MbPopup
    {
        [SerializeField] private Button _close;
        [SerializeField] private Button _quit;
        [SerializeField] private Slider _bgmSlider;
        [SerializeField] private Slider _sfxSlider;

        protected override void Reset()
        {
            base.Reset();
            _close = _content.Find("Bg").Find("Close").GetComponent<Button>();
            _quit = _content.Find("Bg").Find("Quit").GetComponent<Button>();
            _bgmSlider = _content.Find("Bg").Find("Music").GetComponentInChildren<Slider>();
            _sfxSlider = _content.Find("Bg").Find("SFX").GetComponentInChildren<Slider>();
        }

        protected override void OnEnable()
        {
            _group.DOFade(1f, 0.3f).SetEase(Ease.OutElastic);
            _content.localScale = Vector3.zero;
            _content.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
            _close.onClick.AddListener(HidePopup);
            _quit.onClick.AddListener(QuitGame);

            _bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
            _sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);


            _bgmSlider.onValueChanged.AddListener(MbAudioManager.Instance.SetBGMVolume);
            _sfxSlider.onValueChanged.AddListener(MbAudioManager.Instance.SetSFXVolume);
        }

        private void QuitGame()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _close.onClick.RemoveListener(HidePopup);
            _quit.onClick.RemoveListener(QuitGame);
        }
    }

}
