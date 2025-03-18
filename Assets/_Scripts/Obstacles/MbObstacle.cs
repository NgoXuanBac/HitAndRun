using UnityEngine;

namespace HitAndRun.Obstacles
{
    public abstract class MbObstacle : MonoBehaviour
    {

        protected virtual void Start() { }
        protected virtual void Reset() { }
        protected virtual void Awake() { }
        protected virtual void Update() { }
        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }

        protected static class Obstacles
        {
            public const string BlockWall = "BlockWall";
            public const string Saw = "Saw";
            public const string SawCylinder = "SawCylinder";
            public const string MovingSaw = "MovingSaw";
            public const string Hammer = "Hammer";
            public const string Pendulum = "Pendulum";
            public const string Spike = "Spike";
        }
    }
}