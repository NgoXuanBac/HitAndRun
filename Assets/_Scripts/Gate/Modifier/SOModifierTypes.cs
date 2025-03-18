using System;
using System.Collections.Generic;
using UnityEngine;

namespace HitAndRun.Gate.Modifier
{
    [CreateAssetMenu(fileName = "ModifierType", menuName = "HitAndRun/Modifier Type")]
    public class SOModifierTypes : ScriptableObject
    {
        [SerializeField] private string _name;
        public string Name => _name;

        [SerializeField] private List<ModifierType> _types;
        public List<ModifierType> Types => _types;

        public List<ModifierType> GetTypes(ModifierCategory category)
        {
            return _types.FindAll(x => x.Category == category);
        }
    }

    public enum ModifierCategory
    {
        Positive,
        Negative,
    }

    [Serializable]
    public struct ModifierType
    {
        public Color Color;
        public Sprite Icon;
        public int Amount;
        public ModifierCategory Category;

    }

}

