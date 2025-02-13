using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HitAndRun.Assets.TruongHi.Assets._Data._Script

{
    public class MbBaseMonobehavior : MonoBehaviour
    {
        protected virtual void Awake(){
            this.LoadComponents();
        }
        protected virtual void Reset(){
            this.LoadComponents();
        }
        protected virtual void LoadComponents(){
            // Add your component loading logic here
        }
    }

}
