using UnityEngine;

namespace HitAndRun.Obstacles
{
    public abstract class MbObstacle : MonoBehaviour
    {

        protected abstract void Start();
        protected abstract void Reset();
        protected virtual void Awake() { }
        protected virtual void Update() { }
    }
}


