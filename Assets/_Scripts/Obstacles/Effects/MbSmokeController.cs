using UnityEngine;
using UnityEngine.VFX;

namespace HitAndRun.Obstacles.Effects
{
    public class MbSmokeController : MonoBehaviour
    {
        void OnTriggerEnter(Collider trig)
        {
            if (trig.gameObject.tag == "Ground")
            {
                GetComponent<VisualEffect>().Play();
            }
        }
    }

}
