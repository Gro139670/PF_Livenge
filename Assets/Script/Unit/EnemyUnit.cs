using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyUnit : TeamHelper
    {
        public override void SetEnemyID()
        {
            _OwnerInfo.EnemyTeamID = GetTeamID("Our");
        }

        public override void SetTeamID()
        {
            _OwnerInfo.EnemyTeamID = GetTeamID("Enemy");
        }

        private new void Start()
        {
            base.Start();
            _OwnerInfo.LookDir = Unit.Direction.Down;
        }
    }
}


