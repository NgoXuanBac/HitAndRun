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

        [SerializeField] private MbTeam _team;
        [SerializeField] private MbMapGenerator _generator;
        [SerializeField] private MbEnemyTracker _enemyTracker;

        public void Reset()
        {
            _currentLevel = _saveManager.Load("Level", _currentLevel);
            _specifications = _saveManager.Load("Specifications", _specifications);
            _team = FindObjectOfType<MbTeam>();
            _generator = FindObjectOfType<MbMapGenerator>();
            _enemyTracker = FindObjectOfType<MbEnemyTracker>();
        }

        private void Awake()
        {
            _team.Movement.OnFinish += OnFinish;
        }

        private void Start()
        {
            _generator.GenerateMap();
            _enemyTracker.Reset();
        }

        public void OnWin()
        {
            Debug.Log("Win");
        }

        public void OnFinish(Vector3 position)
        {
            _enemyTracker.Enemies.ForEach(e => e.Target = position);
        }

    }

    [Serializable]
    public struct Specifications
    {
        public int Damage;
        public float FireRate;
    }
}

