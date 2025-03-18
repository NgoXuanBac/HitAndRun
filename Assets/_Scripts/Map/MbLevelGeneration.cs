using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HitAndRun.Map
{
    public class MbLevelGeneration : MonoBehaviour
    {
        [Header("Chunk")]
        [SerializeField] private int _chunkCount = 30;
        [SerializeField] private List<TypeRatio> _ratio;

        private void Reset()
        {
            _ratio = new List<TypeRatio>();
            foreach (SpawnType type in Enum.GetValues(typeof(SpawnType)))
            {
                _ratio.Add(new TypeRatio { Type = type, Weight = 1 });
            }
        }

        public SpawnType[] Generate(int currentLevel)
        {
            var chunks = new SpawnType[_chunkCount];

            for (int index = 3; index < _chunkCount; index++)
            {
                var types = _ratio
                    .Where(v => v.Type != chunks[index - 1])
                    .ToList();
                // chunks[index] = types[UnityEngine.Random.Range(0, types.Count)].Type;
                chunks[index] = SpawnType.Gate;
            }

            return chunks;
        }

        [Serializable]
        private struct TypeRatio
        {
            public SpawnType Type;
            public int Weight;
        }
    }
}
