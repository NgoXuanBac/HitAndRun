using UnityEngine;
namespace HitAndRun.Monster
{
    public abstract class MbBaseBehavior : MonoBehaviour
    {
        protected virtual void Awake()
        {
            this.LoadComponents();
        }
        protected virtual void Reset()
        {
            this.LoadComponents();
        }
        protected abstract void LoadComponents();
    }

}
