using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HitAndRun.Gate.Modifier
{
    public class MbModifierView : MonoBehaviour
    {
        [SerializeField] private Renderer _cylinderL;
        [SerializeField] private Renderer _cylinderR;
        [SerializeField] private Renderer _banner;
        [SerializeField] private Image _background;
        [SerializeField] private TMP_Text _info;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private Image _icon;

        private void Reset()
        {
            _cylinderL = transform.Find("Cylinder_L").GetComponent<Renderer>();
            _cylinderR = transform.Find("Cylinder_R").GetComponent<Renderer>();
            _banner = transform.Find("Banner").GetComponent<Renderer>();
            _background = transform.Find("Canvas").Find("Background").GetComponent<Image>();
            _icon = transform.Find("Canvas").Find("Icon").GetComponent<Image>();
            _name = transform.Find("Canvas").Find("Name").GetComponent<TMP_Text>();
            _info = transform.Find("Canvas").Find("Info").GetComponent<TMP_Text>();
        }

        public void SetVisuals(string name, Color color, Sprite icon, string info)
        {
            _name.text = name;
            _background.color = new Color(color.r, color.g, color.b, 0.5f);
            _info.text = info;
            _icon.sprite = icon;

            if (Application.isPlaying)
            {
                _cylinderL.material.SetColor("_BaseColor", color);
                _cylinderR.material.SetColor("_BaseColor", color);
                _banner.material.SetColor("_BaseColor", color);
            }
            else
            {
                _cylinderL.sharedMaterial.SetColor("_BaseColor", color);
                _cylinderR.sharedMaterial.SetColor("_BaseColor", color);
                _banner.sharedMaterial.SetColor("_BaseColor", color);
            }
        }
    }
}
