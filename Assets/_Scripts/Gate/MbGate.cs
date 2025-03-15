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

        public override void SetVisuals(string name, string info, ModifierType type)
        {
            _name.text = name;
            _background.color = new Color(type.Color.r, type.Color.g, type.Color.b, 0.5f);
            _info.text = info;
            _icon.sprite = type.Icon;

            if (Application.isPlaying)
            {
                _cylinderL.material.SetColor("_BaseColor", type.Color);
                _cylinderR.material.SetColor("_BaseColor", type.Color);
                _banner.material.SetColor("_BaseColor", type.Color);
            }
            else
            {
                _cylinderL.sharedMaterial.SetColor("_BaseColor", type.Color);
                _cylinderR.sharedMaterial.SetColor("_BaseColor", type.Color);
                _banner.sharedMaterial.SetColor("_BaseColor", type.Color);
            }
        }
    }
}

