using System.Collections;
using System.Collections.Generic;
using HitAndRun.Gate.Player;
using HitAndRun.Character;
using UnityEngine;

namespace HitAndRun.Gate.Modifier{
    public class ModifierCrowd : MbModifierBase
    {
        [SerializeField] private MbModifierView modifierView;
        [SerializeField] private int crowdModifyAmount = 2;
        private bool _isPositive;

        private void Start()
        {
            _isPositive = crowdModifyAmount > 0;
            modifierView.SetVisuals(_isPositive, crowdModifyAmount);
        }


        public override void Modify(MbPlayerCollision playerController)
        {
            var playerCrowd = playerController.GetComponent<MbCharacter>();
            // for (int i = 0; i < Mathf.Abs(crowdModifyAmount); i++)
            // {
            //     if(_isPositive) playerCrowd.AddShooter();
            //     else playerCrowd.RemoveShooter();
            // }
            Debug.Log($"{(crowdModifyAmount >= 0 ? "+" : "")}{crowdModifyAmount} crowds.");
        }
    }
}

