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
            private bool _IsIgnoreTeamUnit = true;
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
                _OwnerInfo.CurrTile.SetTakedUnit(null);
                //_OwnerInfo.Status.SpeedDebuff = -100;
                return true;
            }

            public override void Logic()
            {
                IsMove();
            }

            protected override void IsMove()
            {
                if(_IsMove == true)
                    return;

                if (_IsIgnoreTeamUnit == true)
                {
                    var unit = _OwnerInfo.CurrTile.AdjacentTiles[(int)_OwnerInfo.LookDir]?.GetTakedUnit();
                    if (unit?.Status.TeamID == _OwnerInfo.EnemyTeamID ||
                        unit?.Status.UnitID == _OwnerInfo.Status.UnitID)
                    {
                        IsStateFinish = true;
                        _IsIgnoreTeamUnit = false;
                        return;
                    }



                    if (_OwnerInfo.CurrTile.AdjacentTiles[(int)_OwnerInfo.LookDir] != null)
                    {
                        if (_OwnerInfo.CurrTile.GetTakedUnit() == _OwnerInfo)
                        {
                            _OwnerInfo.CurrTile.SetTakedUnit(null);
                        }

                        _OwnerInfo.CurrTile = _OwnerInfo.CurrTile.AdjacentTiles[(int)_OwnerInfo.LookDir];
                        if (_OwnerInfo.CurrTile.GetTakedUnit() == null)
                        {
                            _OwnerInfo.CurrTile.SetTakedUnit(_OwnerInfo);
                        }
                    }
                    else
                    {
                        IsStateFinish = true;
                        _IsIgnoreTeamUnit = false;
                        return;
                    }


                    _Owner.transform.SetParent(_OwnerInfo.CurrTile.gameObject.transform);
                    _PrevPosition = _Owner.transform.localPosition;
                    _IsMove = true;
                }
            }
        }

    }
}