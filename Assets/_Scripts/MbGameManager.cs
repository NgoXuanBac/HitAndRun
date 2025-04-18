using System;
using HitAndRun.Character;
using HitAndRun.Gui;
using HitAndRun.Gui.Popup;
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
        [SerializeField] private GameData _data = new() { Amount = 10, Level = 1, Damage = 1, FireRate = 1 };
        public GameData Data => _data;
        private SaveManager _saveManager = new();

        [SerializeField] private MbEnemyTracker _enemiesTracker;
        [SerializeField] private MbCharacterTracker _charactersTracker;
        [SerializeField] private MbUIManager _uiManager;
        [SerializeField] private MbTeam _team;
        [SerializeField] private MbMapGenerator _generator;
        [SerializeField] private MbSetting _setting;

        public event Action<GameData> OnDataLoaded;
        public void Reset()
        {
            _team = FindObjectOfType<MbTeam>();
            _generator = FindObjectOfType<MbMapGenerator>();
            _setting = FindObjectOfType<MbSetting>();
            _enemiesTracker = MbEnemyTracker.Instance;
            _charactersTracker = MbCharacterTracker.Instance;
            _uiManager = MbUIManager.Instance;
        }

        private void Start()
        {
            Restart();
        }

        public void HandleWin()
        {
            _team.Victory();
            _enemiesTracker.OnEnemiesDied -= HandleWin;
            _charactersTracker.OnCharactersDied -= HandleLose;
            _uiManager.ShowPopup<MbWinPopup>();
        }

        public void HandleLose()
        {
            _enemiesTracker.OnEnemiesDied -= HandleWin;
            _charactersTracker.OnCharactersDied -= HandleLose;
            _uiManager.ShowPopup<MbLosePopup>();
        }

        public void NextLevel()
        {
            _data.Level++;
            _saveManager.Save(_data, "Data");
            Restart();
        }

        public void AddCoin(long amount)
        {
            _data.Amount += amount;
            _saveManager.Save(_data, "Data");
            OnDataLoaded?.Invoke(_data);
        }

        public void Upgrade(GameData data, long amount)
        {
            data.Amount -= amount;
            _data = data;
            _saveManager.Save(_data, "Data");
            OnDataLoaded?.Invoke(_data);
        }

        public void Restart()
        {
            _data = _saveManager.Load("Data", _data);
            OnDataLoaded?.Invoke(_data);

            _setting.ShowSetting();

            _generator.CleanMap();
            _generator.GenerateMap(_data);
            _team.SetUp();

            _enemiesTracker.OnEnemiesDied += HandleWin;
            _charactersTracker.OnCharactersDied += HandleLose;

            _uiManager.ShowPopup<MbWaitPopup>();
        }

        public void StartGame()
        {
            _team.Run(_data);
            _setting.HideSetting();
            _enemiesTracker.Reset();
            _charactersTracker.Reset();
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
    public struct GameData
    {
        public int Level;
        public long Amount;
        public int Damage;
        public int FireRate;
    }
}

