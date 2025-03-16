using System;
using HitAndRun.Character;
using HitAndRun.Map;
using Unity.VisualScripting;
using UnityEngine;

namespace HitAndRun
{
    public enum GameState
    {
        Play, Run,
    }
    public class MbGameManager : MbSingleton<MbGameManager>
    {
        [SerializeField] private int _currentLevel = 1;
        public int CurrentLevel => _currentLevel;

        [SerializeField] private Specifications _specifications = new() { Damage = 2, FireRate = 0.2f };
        public Specifications Specifications => _specifications;
        private SaveManager _saveManager = new();

        [SerializeField] private MbEnemyTracker _enemiesTracker;
        [SerializeField] private MbTeam _team;
        [SerializeField] private MbMapGenerator _generator;

        public void Reset()
        {
            _currentLevel = _saveManager.Load("Level", _currentLevel);
            _specifications = _saveManager.Load("Specifications", _specifications);
            _team = FindObjectOfType<MbTeam>();
            _generator = FindObjectOfType<MbMapGenerator>();
            _enemiesTracker = MbEnemyTracker.Instance;
        }

        private void Start()
        {
            _generator.GenerateMap();
            _enemiesTracker.Reset();
            _enemiesTracker.OnEnemiesDied += HandleWin;
            _team.OnCharactersDied += HandleLose;
        }

        public void HandleWin()
        {
            _enemiesTracker.OnEnemiesDied -= HandleWin;
            _team.OnCharactersDied -= HandleLose;
            Debug.Log("Win");
        }

        public void HandleLose(bool isFinish)
        {
            _enemiesTracker.OnEnemiesDied -= HandleWin;
            _team.OnCharactersDied -= HandleLose;
            Debug.Log("Lose");
        }

    }

    [Serializable]
    public struct Specifications
    {
        public int Damage;
        public float FireRate;
    }
}

