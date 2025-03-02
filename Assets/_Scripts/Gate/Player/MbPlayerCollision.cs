using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HitAndRun.Gate.Modifier;

namespace HitAndRun.Gate.Player{
    public class MbPlayerCollision : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Modifier"))
            {
                var modifier = other.GetComponent<MbModifierBase>();
                if (modifier)
                {
                    modifier.Modify(this);
                }
            }
        }
    }
}
