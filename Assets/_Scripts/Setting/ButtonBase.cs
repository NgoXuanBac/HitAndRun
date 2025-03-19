using UnityEngine;
namespace HitAndRun.Setting
{
    public class ButtonBase : MonoBehaviour
    {
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            if (button != null)
            {
                ClickSound();
            }
        }

        private void ClickSound()
        {

        }
    }

}
