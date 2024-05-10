using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

namespace Team
{
    public class TeamUnit : TeamHelper
    {
        public override void SetEnemyID()
        {
            _OwnerInfo.EnemyTeamID = GetTeamID("Enemy");
        }

        public override void SetTeamID()
        {
            _OwnerInfo.EnemyTeamID = GetTeamID("Our");
        }

        private new void Start()
        {
            base.Start();
            _OwnerInfo.LookDir = Unit.Direction.Up;
        }
    }
}
