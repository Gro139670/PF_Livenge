using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class DebuffTower : StateMachine
    {
        public override bool Initialize()
        {
            AddState<CommonStateIdle>();
            AddState<CommonStateAttack>();
            AddState<TowerStateSearch>();

            return true;
        }
    }

    public class TowerStateSearch : State
    {
        public override string CheckTransition()
        {
            return "Idle";
        }

        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override void FixedLogic()
        {
        }

        public override bool Initialize()
        {
            return true;
        }

        public override void Logic()
        {
        }
    }
}