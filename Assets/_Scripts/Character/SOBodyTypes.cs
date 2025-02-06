using UnityEngine;
using System.Collections.Generic;

namespace HitAndRun.Character
{
    [CreateAssetMenu(fileName = "BodyTypes", menuName = "HitAndRun/Body Types")]
    public class SOBodyTypes : ScriptableObject
    {
        [SerializeField]
        private List<Color> _bodyTypes;
        public Color GetColorByLevel(int level)
        {
            var index = (int)Mathf.Log(level, 2);
            index = (index < 0 || index >= _bodyTypes.Count) ? 0 : index;
            return _bodyTypes[index];
        }
    }

}

