using System.Collections;
using System.Collections.Generic;
using HitAndRun.Gate.Player;
using UnityEngine;

namespace HitAndRun.Gate.Modifier {
    public abstract class MbModifierBase : MonoBehaviour
    {
        public abstract void Modify(MbPlayerCollision playerController);
    }
}

