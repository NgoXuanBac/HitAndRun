using UnityEngine;
using UnityEngine.AI;
namespace HitAndRun.Monster
{

    public class MbEnemyControl : MbBaseBehavior
    {
        [SerializeField]
        protected NavMeshAgent agent;
        public NavMeshAgent Agent => agent;
        protected override void LoadComponents()
        {
            this.LoadNavMeshAgent();
        }
        protected virtual void LoadNavMeshAgent()
        {
            if (agent != null)
            {
                return;
            }
            agent = GetComponent<NavMeshAgent>();

        }
    }
}
