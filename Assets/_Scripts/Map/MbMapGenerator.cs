using HitAndRun.Character;
using HitAndRun.Inspector;
using HitAndRun.Tower;
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

        private void Reset()
        {
            _ground = GetComponentInChildren<MbGround>();
            _ground.ChunkCount = _chunkCount;
            _chunkHeight = _ground.Length / _chunkCount;
            _chunkWidth = _ground.Width;
        }

        private void Start()
        {
            GenerateMap();
        }

        public void GenerateMap()
        {
            var level = MbGameManager.Instance.CurrentLevel;

            for (int index = 3; index < _chunkCount; index++)
            {
                var noise = Mathf.PerlinNoise(level, index * 0.3f);
                if (noise < 0.5f)
                {
                    MbCharacterSpawner.Instance.Spawn(new Vector3(0, 8, index * _chunkHeight), transform, MbCharacter.INACTIVE_TAG);
                    MbTowerSpawner.Instance.Spawn(new Vector3(0, 0, index * _chunkHeight), transform, 100);
                }
            }
        }
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
