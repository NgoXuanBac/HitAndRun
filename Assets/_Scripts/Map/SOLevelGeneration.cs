using System;
using System.Collections.Generic;
using UnityEngine;

namespace HitAndRun.Map
{
    [CreateAssetMenu(fileName = "LevelGeneration", menuName = "HitAndRun/Map/Level Generation")]
    public class SOLevelGeneration : ScriptableObject
    {
        [SerializeField]
        private List<LevelData> _prebuildLevels = new();

        public LevelData GetPrebuildLevel(int level)
        {
            var index = level - 1;
            if (index < _prebuildLevels.Count)
                return _prebuildLevels[index];

            //TODO: Calculate auto generation map;
            throw new NotImplementedException("Not implemented auto calculate generation map");
        }
    }


    [Serializable]
    public struct LevelData
    {
        [Range(0, 1)]
        public float TowerThreshold;
        [Range(0, 1)]
        public float CharacterThreshold;
        [Range(0, 5)]
        public float NoiseScale;
    }
}
