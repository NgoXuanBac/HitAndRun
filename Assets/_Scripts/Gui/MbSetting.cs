using HitAndRun.Gui.Popup;
using UnityEngine;
using UnityEngine.UI;

namespace HitAndRun.Gui
{
    public class MbSetting : MonoBehaviour
    {
        [SerializeField] private Button _btn;
        [SerializeField] private Image _icon;
        [SerializeField] private Sprite _setting;
        [SerializeField] private Sprite _restart;
        private void Reset()
        {
            _btn = GetComponentInChildren<Button>();
            _icon = _btn.GetComponent<Image>();
            _setting = Resources.Load<Sprite>("Textures/UI/setting");
            _restart = Resources.Load<Sprite>("Textures/UI/restart");
        }

        private void Awake()
        {
            _btn.onClick.AddListener(ToggleSetting);
        }

        private void OnDestroy()
        {
            _btn.onClick.RemoveListener(ToggleSetting);
        }

        public void ShowSetting() => _icon.sprite = _setting;
        public void HideSetting() => _icon.sprite = _restart;

        public void ToggleSetting()
        {
            if (_icon.sprite == _setting)
            {
                MbUIManager.Instance.ShowPopup<MbSettingPopup>();
            }
            else
            {
                MbGameManager.Instance.Restart();
            }
        }

    }
}
