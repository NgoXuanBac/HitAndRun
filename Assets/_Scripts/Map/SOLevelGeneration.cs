using System;
using System.Collections.Generic;
using UnityEngine;

namespace HitAndRun.Map
{
    [CreateAssetMenu(fileName = "LevelGeneration", menuName = "ScriptableObjects/Level Generation")]
    public class SOLevelGeneration : ScriptableObject
    {
        [SerializeField]
        private List<PrebuildLevel> _prebuildLevels = new();

        public PrebuildLevel GetPrebuildLevel(int level)
        {
            var index = level - 1;
            if (index < _prebuildLevels.Count)
                return _prebuildLevels[index];

            //TODO: Calculate auto generation map;
            throw new NotImplementedException("Not implemented auto calculate generation map");
        }
    }


    [Serializable]
    public struct PrebuildLevel
    {
        [Range(0, 100)]
        public int MaxCharacters;
        [Range(0, 1)]
        public float CharacterThreshold;
        public int MaxTowerHeath;
        [Range(0, 100)]
        public int MaxObstacles;
        [Range(0, 1)]
        public float ObstacleThreshold;
        [Range(0, 5)]
        public float NoiseScale;
    }
}
