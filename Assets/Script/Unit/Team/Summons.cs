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
            AddState<OurStateDefault>();

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
                _OwnerInfo.Status.SpeedDebuff = 0;
                return "CommonStateIdle";
            }

            public override void FixedLogic()
            {
                if (_IsMove == false)
                    return;

                IsStateFinish = TimeManager.Instance.SetTime(ref _MoveTime, _OwnerInfo.Status.MoveSpeed);


                _Owner.transform.localPosition = Vector3.Lerp(_PrevPosition, _OwnerInfo.Position, _MoveTime / _OwnerInfo.Status.MoveSpeed);
            }

            public override bool Initialize()
            {
                _OwnerInfo.Status.SpeedDebuff = -100;
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