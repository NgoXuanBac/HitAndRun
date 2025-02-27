using HitAndRun.Character;
using HitAndRun.Inspector;
using UnityEditor;
using UnityEngine;

namespace HitAndRun.Map
{
    public class MbMapGenerator : MonoBehaviour
    {
        [SerializeField] private MbGround _ground;

        [Header("Chunk")]
        [SerializeField] private int _chunkCount = 60;
        [SerializeField, ReadOnly] private float _chunkHeight;
        [SerializeField, ReadOnly] private float _chunkWidth;

        [Header("Generation")]
        [SerializeField] private SOLevelGeneration _generation;

        private int _currentCharacterCount = 0;
        private SpawnPattern[] _spawnPatterns = new[] { new SingleSpawnPattern() };
        // private int _currentObstacleCount = 0;

        private void Reset()
        {
            _generation = Resources.Load<SOLevelGeneration>("Scriptables/LevelGeneration");
            _ground = GetComponentInChildren<MbGround>();
            _ground.ChunkCount = _chunkCount;
            _chunkHeight = _ground.Length / _chunkCount;
            _chunkWidth = _ground.Width;
            _currentCharacterCount = 0;
            // _currentObstacleCount = 0;
        }

        public void GenerateMap()
        {
            var level = MbGameManager.Instance.CurrentLevel;
            var config = _generation.GetPrebuildLevel(level);

            for (int index = 3; index < _chunkCount; index++)
            {
                var noise = Mathf.PerlinNoise(MbGameManager.Instance.CurrentLevel, index * 0.3f);
                GenerateByNoise(index, noise, config);

            }
        }

        private void GenerateByNoise(int index, float value, PrebuildLevel config)
        {
            if (value < config.CharacterThreshold && _currentCharacterCount < config.MaxCharacters)
            {
                var offset = _spawnPatterns[0].GetSpawnPosX();
                var position = new Vector3(offset[Random.Range(0, offset.Length)] * (_chunkWidth / 3), 0, index * _chunkHeight);
                MbCharacterSpawner.Instance.SpawnCharacter(position, transform, MbCharacter.INACTIVE_TAG);
            }
        }
    }

    public enum SpawnObjectType
    {
        Character,
        Tower,
        Gate,
        Obstacle
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MbMapGenerator))]
    public class EMapGeneratorInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            var generator = (MbMapGenerator)target;
            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button($"Generate"))
            {
                generator.GenerateMap();
            }
            GUI.enabled = true;
            EditorGUILayout.Space();
            DrawDefaultInspector();
        }
    }
#endif

}
