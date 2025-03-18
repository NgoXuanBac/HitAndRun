using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HitAndRun.Map
{
    public class MbLevelGeneration : MonoBehaviour
    {
        [Header("Chunk")]
        [SerializeField] private List<TypeRatio> _ratio;
        [SerializeField, Range(3.5f, 5)] private float _coverage = 3.5f;
        private void Reset()
        {
            _ratio = new List<TypeRatio>();
            foreach (SpawnType type in Enum.GetValues(typeof(SpawnType)))
            {
                if (type == SpawnType.Character) continue;
                _ratio.Add(new TypeRatio { Type = type, Weight = 1 });
            }
        }

        public SpawnType[] Generate(int currentLevel)
        {
            var chunks = new SpawnType[30];

            for (int index = 3; index < 30; index++)
            {
                var types = _ratio
                    .Where(v => v.Type != chunks[index - 1])
                    .ToList();
                chunks[index] = types[UnityEngine.Random.Range(0, types.Count)].Type;
            }

            return chunks;
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

            for (int i = 0; i < count; i++)
            {
                if (positions.Contains(i))
                {
                    chunks.Add(SpawnType.Character);
                }
                else if (i >= min)
                {
                    var types = _ratio.Where(v => v.Type != chunks[i - 1]).ToList();
                    chunks.Add(types[UnityEngine.Random.Range(0, types.Count)].Type);
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
            chunks.Add(SpawnType.Empty);

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
