using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HitAndRun.Map
{
    [CreateAssetMenu(fileName = "SpawnRule", menuName = "HitAndRun/Map/Spawn Rule")]
    public class SOSpawnRule : ScriptableObject
    {
        [SerializeField, Range(1, 5)] private int _grid = 3;
        public int Grid => _grid;
    }
}
