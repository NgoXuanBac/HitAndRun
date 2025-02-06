using UnityEngine;

namespace HitAndRun.Character
{
    [RequireComponent(typeof(MbLevel))]
    public class MbCharacter : MonoBehaviour
    {
        [SerializeField] private MbLevel _level;
        public MbLevel Level => _level;
        public void Reset()
        {
            _level ??= GetComponent<MbLevel>();
            _level.Value = 2;
        }
    }
}
