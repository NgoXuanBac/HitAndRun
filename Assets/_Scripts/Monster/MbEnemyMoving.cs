using UnityEngine;
namespace HitAndRun.Monster
{
    public class MbEnemyMoving : MbBaseBehavior
    {
        public GameObject target;
        [SerializeField] protected MbEnemyControl mbEnemyControl;

        protected override void LoadComponents()
        {
            this.LoadEnemyControl();
            this.LoadTarget();
        }

        private void FixedUpdate()
        {
            this.Moving();
        }

        protected virtual void LoadEnemyControl()
        {
            if (this.mbEnemyControl != null) return;
            mbEnemyControl = transform.parent.GetComponent<MbEnemyControl>();
        }

        protected virtual void Moving()
        {
            mbEnemyControl.Agent.SetDestination(target.transform.position);
        }

        protected virtual void LoadTarget()
        {
            if (this.target != null) return;
            target = GameObject.Find("CharacterGroup");
        }
    }
}
