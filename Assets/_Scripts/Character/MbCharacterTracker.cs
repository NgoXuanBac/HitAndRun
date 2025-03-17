using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HitAndRun.Character
{
    public class MbCharacterTracker : MbSingleton<MbCharacterTracker>
    {
        [SerializeField] private List<MbCharacter> _characters = new();
        public List<MbCharacter> Characters => _characters;
        public event Action OnCharactersDied;

        public void Reset()
        {
            _characters?.Clear();
            _characters = FindObjectsOfType<MbCharacter>().ToList();
        }

        public void RemoveCharacter(MbCharacter character)
        {
            if (_characters.Contains(character)) _characters.Remove(character);
            if (_characters.Count == 0)
            {
                OnCharactersDied?.Invoke();
            }
        }
    }
}

