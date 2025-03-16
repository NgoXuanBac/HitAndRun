using System;
using HitAndRun.Bullet;
using HitAndRun.Character;
using HitAndRun.Enemy;
using HitAndRun.Map;
using HitAndRun.Tower;
using UnityEditor;
using UnityEngine;

namespace HitAndRun
{
    public enum GameState
    {
        Wait, Play, Win, Lose
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

        // => Wait => Play => Win => Lose

        private void Start()
        {
            Restart();
        }

        public void HandleWin()
        {
            _enemiesTracker.OnEnemiesDied -= HandleWin;
            _team.OnCharactersDied -= HandleLose;
            NextLevel();
        }

        public void HandleLose(bool isFinish)
        {
            _enemiesTracker.OnEnemiesDied -= HandleWin;
            _team.OnCharactersDied -= HandleLose;
            Restart();
        }

        private void NextLevel()
        {
            _currentLevel++;
            _saveManager.Save(_currentLevel, "Level");
            Restart();
        }

        public void Restart()
        {
            _generator.CleanMap();
            _generator.GenerateMap();
            _enemiesTracker.Reset();
            _team.Init();

            _enemiesTracker.OnEnemiesDied += HandleWin;
            _team.OnCharactersDied += HandleLose;
        }
    }



#if UNITY_EDITOR
    [CustomEditor(typeof(MbGameManager))]
    public class EGameManagerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            var manager = (MbGameManager)target;
            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button("Reset"))
            {
                manager.Restart();
            }
            GUI.enabled = true;
            EditorGUILayout.Space();
            DrawDefaultInspector();
        }
    }
#endif

    [Serializable]
    public struct Specifications
    {
        public int Damage;
        public float FireRate;
    }
}

