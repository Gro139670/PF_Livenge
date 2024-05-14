using System.Collections;
using UnityEngine;

namespace Team
{
    public class Summons : StateMachine
    {
        public override bool Initialize()
        {
            AddState<SummonsStateForward>();
            AddState<CommonStateIdle>();
            AddState<CommonStateAttack>();
            AddState<CommonStateSearch>();
            AddState<CommonStateMove>();
            AddState<TeamStateDefault>();

            ChangeState<SummonsStateForward>();
            return true;
        }

        private class SummonsStateForward : StateMove
        {

            public override void Enter()
            {
                base.Enter();
                _OwnerInfo.LookDir = Unit.Direction.Up;
                _IsIgnoreTeamUnit = true;

            }

            public override void Exit()
            {
                base.Exit();
              
            }

            public override string CheckTransition()
            {
                if(_IsIgnoreTeamUnit == true)
                {
                    return "SummonsStateForward";
                }

                return "CommonStateIdle";
            }

            public override void FixedLogic()
            {
                DoMove();
            }

            public override bool Initialize()
            {
                _OwnerInfo.CurrTile.SetTakedUnit(null);

                return true;
            }

            public override void Logic()
            {
                IsMove();
            }
        }

    }
}