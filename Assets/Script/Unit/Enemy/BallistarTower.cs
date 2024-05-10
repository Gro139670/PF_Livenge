using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class BallistarTower : StateMachine
    {
        public override bool Initialize()
        {
            AddState<CommonStateIdle>();
            AddState<CommonStateAttack>();
            AddState<TowerStateSearch>();

            return true;
        }
    }
}