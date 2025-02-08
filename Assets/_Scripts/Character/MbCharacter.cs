using UnityEngine;

namespace HitAndRun.Character
{
    [RequireComponent(typeof(MbCharacterBody))]
    public class MbCharacter : MonoBehaviour
    {
        [SerializeField] private MbCharacterBody _body;
        public MbCharacterBody Body => _body;
        public void Reset()
        {
            _body ??= GetComponent<MbCharacterBody>();
            _body.Level = 2;
        }
    }
}
