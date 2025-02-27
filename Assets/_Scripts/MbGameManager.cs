using UnityEngine;

namespace HitAndRun
{
    public class MbGameManager : MbSingleton<MbGameManager>
    {
        [SerializeField] private int _currentLevel = 1;
        public int CurrentLevel => _currentLevel;
    }
}

