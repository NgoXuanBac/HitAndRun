using System;
using HitAndRun.Character;
using HitAndRun.Map;
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
        [SerializeField] private SaveData _data = new() { Amount = 0, Level = 1, Damage = 2, FireRate = 0.2f };
        public SaveData Data => _data;
        private SaveManager _saveManager = new();

        [SerializeField] private MbEnemyTracker _enemiesTracker;
        [SerializeField] private MbCharacterTracker _charactersTracker;
        [SerializeField] private MbTeam _team;
        [SerializeField] private MbMapGenerator _generator;

        public event Action<SaveData> OnDataLoaded;
        public void Reset()
        {
            _team = FindObjectOfType<MbTeam>();
            _generator = FindObjectOfType<MbMapGenerator>();
            _enemiesTracker = MbEnemyTracker.Instance;
            _charactersTracker = MbCharacterTracker.Instance;
        }

        private void Start()
        {
            Restart();
        }

        public void HandleWin()
        {
            _enemiesTracker.OnEnemiesDied -= HandleWin;
            _charactersTracker.OnCharactersDied -= HandleLose;
            NextLevel();
        }

        public void HandleLose()
        {
            _enemiesTracker.OnEnemiesDied -= HandleWin;
            _charactersTracker.OnCharactersDied -= HandleLose;
            Restart();
        }

        private void NextLevel()
        {
            _data.Level++;
            _saveManager.Save(_data, "Data");
            Restart();
        }

        public void Restart()
        {
            _data = _saveManager.Load("Data", _data);
            OnDataLoaded?.Invoke(_data);

            _generator.CleanMap();
            _generator.GenerateMap();
            _enemiesTracker.Reset();
            _team.Init();

            _enemiesTracker.OnEnemiesDied += HandleWin;
            _charactersTracker.OnCharactersDied += HandleLose;
        }

        public void StartGame()
        {
            _team.ActiveCharacters();
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
    public struct SaveData
    {
        public int Level;
        public long Amount;
        public int Damage;
        public float FireRate;
    }
}

