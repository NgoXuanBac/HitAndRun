using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HitAndRun.Map
{
    [CreateAssetMenu(fileName = "SpawnRule", menuName = "HitAndRun/Map/Spawn Rule")]
    public class SOSpawnRule : ScriptableObject
    {
        [SerializeField] private SpawnType _spawnType;
        public SpawnType SpawnType => _spawnType;
        [SerializeField]
        private List<SpawnPattern> _spawnPatterns;

        public IEnumerable<float> GetSpawnRatios()
        {
            var index = UnityEngine.Random.Range(0, _spawnPatterns.Count);
            var pattern = _spawnPatterns[index];
            if (pattern.Ratios == null || pattern.Ratios.Count == 0)
                return new List<float>();
            return GetRandomRatios(pattern.Ratios, pattern.Count);
        }

        private List<float> GetRandomRatios(List<float> ratios, int count)
        {
            if (ratios.Count == count) return ratios;
            var random = new System.Random();
            return ratios.OrderBy(x => random.Next()).Take(count).ToList();
        }

        [Serializable]
        private struct SpawnPattern
        {
            public string Name;
            public int Count;
            public List<float> Ratios;
        }
    }


}
