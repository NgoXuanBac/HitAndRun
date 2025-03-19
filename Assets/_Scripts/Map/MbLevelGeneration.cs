using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HitAndRun.Map
{
    public class MbLevelGeneration : MonoBehaviour
    {
        [Header("Chunk")]

        [SerializeField] private List<LevelMilestone> _milestones;
        [SerializeField, Range(3.5f, 5)] private float _coverage = 3.5f;
        private void Reset()
        {
            var ratio = new List<TypeRatio>();
            foreach (SpawnType type in Enum.GetValues(typeof(SpawnType)))
            {
                if (type == SpawnType.Character) continue;
                ratio.Add(new TypeRatio { Type = type, Weight = 1 });
            }

            _milestones = new List<LevelMilestone>
            {
                new() { LevelThreshold = 0, TypeRatios = ratio },
            };
        }

        public List<SpawnType> Generate(int level, int characterNum, int min = 3)
        {
            var count = min + Mathf.CeilToInt(characterNum * _coverage);
            var chunks = new List<SpawnType>();

            var availables = new List<int>();
            for (int i = min; i < count; i++) availables.Add(i);


            var positions = new HashSet<int>();
            while (positions.Count < characterNum)
            {
                var random = UnityEngine.Random.Range(0, availables.Count);
                var position = availables[random];

                positions.Add(position);
                availables.Remove(position);

                availables.Remove(position - 1);
                availables.Remove(position + 1);
            }
            var ratio = GetTypeRatiosForLevel(level);
            for (int i = 0; i < count; i++)
            {
                if (positions.Contains(i))
                {
                    chunks.Add(SpawnType.Character);
                }
                else if (i >= min)
                {
                    if (ratio.Count == 0) continue;
                    var type = GetRandomType(ratio.Where(v => v.Type != chunks[i - 1]).ToList());
                    chunks.Add(type);
                }
                else
                {
                    chunks.Add(SpawnType.Empty);
                }
            }

            while (chunks.Count > min && chunks[min] == SpawnType.Empty)
            {
                chunks.RemoveAt(min);
            }

            while (chunks.Count > min && chunks[^1] == SpawnType.Empty)
            {
                chunks.RemoveAt(chunks.Count - 1);
            }

            return chunks;
        }

        private SpawnType GetRandomType(List<TypeRatio> types)
        {
            int totalWeight = types.Sum(t => t.Weight);
            int randomValue = UnityEngine.Random.Range(0, totalWeight);

            foreach (var type in types)
            {
                if (randomValue < type.Weight)
                    return type.Type;
                randomValue -= type.Weight;
            }

            return SpawnType.Empty;
        }

        private List<TypeRatio> GetTypeRatiosForLevel(int level)
        {
            var milestone = _milestones.FirstOrDefault(m => level >= m.LevelThreshold);
            return milestone.TypeRatios.Count > 0 ? milestone.TypeRatios : new List<TypeRatio>();
        }

        [Serializable]
        private struct LevelMilestone
        {
            public int LevelThreshold;
            public List<TypeRatio> TypeRatios;
        }

        [Serializable]
        private struct TypeRatio
        {
            public SpawnType Type;
            public int Weight;
        }
    }
}
