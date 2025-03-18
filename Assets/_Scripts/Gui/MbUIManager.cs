using UnityEngine;

namespace HitAndRun.Gui
{
    public class MbUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _losePopup;
        [SerializeField] private GameObject _winPopup;
        [SerializeField] private GameObject _waitPopup;

        private void Reset()
        {
            _losePopup = GameObject.Find("LosePopup");
            _winPopup = GameObject.Find("WinPopup");
            _waitPopup = GameObject.Find("WaitPopup");

            _losePopup.SetActive(false);
            _winPopup.SetActive(false);
            _waitPopup.SetActive(true);
        }
    }

}
