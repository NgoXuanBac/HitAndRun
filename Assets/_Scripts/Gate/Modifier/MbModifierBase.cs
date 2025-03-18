using HitAndRun.Character;
using UnityEngine;

namespace HitAndRun.Gate.Modifier
{
    public abstract class MbModifierBase : MonoBehaviour
    {
        [SerializeField] protected SOModifierTypes _modifierTypes;
        [SerializeField] protected MbModifierView _modifierView;

        protected ModifierType? _modifierType;

        public abstract void Modify(MbCharacter character);

        protected virtual void Reset()
        {
            _modifierView = GetComponent<MbModifierView>();
        }

        public virtual bool HasCategory(ModifierCategory category)
        {
            return _modifierTypes.GetTypes(category).Count > 0;
        }

        public virtual void SetCategory(ModifierCategory category)
        {
            var types = _modifierTypes.GetTypes(category);
            _modifierType = types[Random.Range(0, types.Count)];

            if (_modifierType == null) return;
            _modifierView.SetVisuals(_modifierTypes.Name, _modifierType.Value.Color,
                _modifierType.Value.Amount == 0 ? null : ((_modifierType.Value.Amount > 0 ? "+" : "") + _modifierType.Value.Amount.ToString()),
                _modifierType.Value.Icon
            );
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out MbCharacter character)) return;
            Modify(character);
        }
    }
}

