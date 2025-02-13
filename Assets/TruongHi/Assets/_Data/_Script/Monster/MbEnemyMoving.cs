 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace HitAndRun.Assets.TruongHi.Assets._Data._Script
{
    public class MbEnemyMoving : MbBaseMonobehavior
    {
        public GameObject target;
        [SerializeField] protected MbEnemyControl mbEnemyControl;

        protected override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadEnemyControl();
            this.LoadTarget();
        }

        void FixedUpdate()
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
            // if (mbEnemyControl.IsDead) return;
            mbEnemyControl.Agent.SetDestination(target.transform.position);
        }

        protected virtual void LoadTarget()
        {
            if (this.target!= null) return;
            target = GameObject.Find("CharacterGroup");
        }
    }
}
