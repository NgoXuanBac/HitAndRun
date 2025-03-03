using HitAndRun.Character;
using UnityEditor;
using UnityEngine;

namespace HitAndRun.Gate.Modifier
{
    public abstract class MbModifierBase : MonoBehaviour
    {
        [SerializeField] protected SOModifierTypes _modifierType;
        [SerializeField] protected MbModifierView _modifierView;
        [SerializeField] protected bool _isPositive;

        public abstract void Modify(MbCharacter character);

        protected virtual void Reset()
        {
            _modifierView = GetComponent<MbModifierView>();
        }

        protected void ApplyVisuals(string info)
        {
            var type = _isPositive ? _modifierType.Positive : _modifierType.Negative;
            _modifierView.SetVisuals(_modifierType.Name, type.Color, type.Icon, info);
        }
    }
}

