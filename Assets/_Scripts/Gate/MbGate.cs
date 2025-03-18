using HitAndRun.Gate.Modifier;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HitAndRun.Gate
{
    public class MbGate : MbModifierView
    {
        [SerializeField] private Renderer _cylinderL;
        [SerializeField] private Renderer _cylinderR;
        [SerializeField] private Renderer _banner;
        [SerializeField] private Image _background;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private Image _icon;

        private void Reset()
        {
            _cylinderL = transform.Find("Cylinder_L").GetComponent<Renderer>();
            _cylinderR = transform.Find("Cylinder_R").GetComponent<Renderer>();
            _banner = transform.Find("Banner").GetComponent<Renderer>();
            var canvas = transform.Find("Canvas");
            _background = canvas.Find("Background").GetComponent<Image>();
            _name = canvas.Find("Name").GetComponent<TMP_Text>();
            _icon = canvas.Find("Background").Find("Icon").GetComponent<Image>();
            _text = canvas.Find("Background").Find("Text").GetComponent<TMP_Text>();
        }

        public override void SetVisuals(string name, Color color, string text = null, Sprite icon = null)
        {
            _name.text = name;
            _text.text = text;
            _icon.sprite = icon;

            _icon.gameObject.SetActive(icon != null);
            _text.gameObject.SetActive(text != null);

            _background.color = new Color(color.r, color.g, color.b, 0.7f);
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

