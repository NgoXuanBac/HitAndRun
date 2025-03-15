using System;
using UnityEngine;

namespace HitAndRun.Gate.Modifier
{
    [CreateAssetMenu(fileName = "ModifierType", menuName = "HitAndRun/Modifier Type")]
    public class SOModifierTypes : ScriptableObject
    {
        [SerializeField] private string _name;
        public string Name => _name;

        [SerializeField] private ModifierType _positive;
        public ModifierType Positive => _positive;
        [SerializeField] private ModifierType _negative;
        public ModifierType Negative => _negative;
    }

    [Serializable]
    public struct ModifierType
    {
        public Color Color;
        public Sprite Icon;
    }
}

