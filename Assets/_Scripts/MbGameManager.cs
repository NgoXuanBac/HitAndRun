using System;
using UnityEngine;

namespace HitAndRun
{
    public class MbGameManager : MbSingleton<MbGameManager>
    {
        [SerializeField] private int _currentLevel = 1;
        public int CurrentLevel => _currentLevel;

        [SerializeField] private Specifications _specifications = new() { Damage = 2, FireRate = 0.2f };
        public Specifications Specifications => _specifications;

        private SaveManager _saveManager = new();

        public void Reset()
        {
            _currentLevel = _saveManager.Load("Level", _currentLevel);
            _specifications = _saveManager.Load("Specifications", _specifications);
        }

    }

    [Serializable]
    public struct Specifications
    {
        public int Damage;
        public float FireRate;
    }
}

