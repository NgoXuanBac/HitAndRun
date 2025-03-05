using UnityEngine;

namespace HitAndRun.Gate.Modifier
{
    public abstract class MbModifierView : MonoBehaviour
    {
        public abstract void SetVisuals(string name, string info, ModifierType type);
    }
}
