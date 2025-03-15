using UnityEngine;
using UnityEngine.UI;

public class ButtonBase : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
        }
    }

    private void ClickSound()
    {
        if (PlayerPrefs.GetInt("SFX", 1) == 1)
        {
            AudioManager.Instance.PlaySFX(SFXType.ButtonClick);
        }
    }
}
