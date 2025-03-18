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
            ClickSound();
        }
    }

    private void ClickSound()
    {
        
    }
}
