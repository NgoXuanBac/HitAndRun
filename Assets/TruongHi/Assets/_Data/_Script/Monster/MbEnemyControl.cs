using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace HitAndRun.Assets.TruongHi.Assets._Data._Script

{

    public class MbEnemyControl : MbBaseMonobehavior
    {
        [SerializeField]
        protected NavMeshAgent agent;
        public NavMeshAgent Agent => agent;
        protected override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadNavMeshAgent();
        }
        protected virtual void LoadNavMeshAgent(){
            if(agent != null){
                return;
            }
            agent = GetComponent<NavMeshAgent>();

        }
    }
}
