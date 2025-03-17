using HitAndRun.Character;
using Unity.VisualScripting;
using UnityEngine;

namespace HitAndRun.Gate.Modifier
{
    public abstract class MbModifierBase : MonoBehaviour
    {
        [SerializeField] protected SOModifierTypes _modifierTypes;
        [SerializeField] protected MbModifierView _modifierView;

        public abstract void Modify(MbCharacter character);

        protected virtual void Reset()
        {
            _modifierView = GetComponent<MbModifierView>();
        }
    }
}

