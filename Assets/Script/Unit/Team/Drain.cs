using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team
{
    public class Drain : StateMachine
    {
        public override bool Initialize()
        {
            AddState<CommonStateIdle>();
            AddState<DrainStateAttack>();
            AddState<CommonStateSearch>();
            AddState<DrainStateMove>();
            AddState<TeamStateDefault>();
            return true;
        }
    }

    public class DrainStateMove : StateMove
    {
        public override string CheckTransition()
        {
            return "Idle";
        }

        public override void FixedLogic()
        {
            DoMove();
        }

        public override bool Initialize()
        {
            return true;
        }

        public override void Logic()
        {
            _OwnerInfo.NextTile = _OwnerInfo.CurrTile.AdjacentTiles[(int)Unit.Direction.Up];
            if (_OwnerInfo.NextTile.GetTakedUnit() == null)
            {
                IsMove();
            }

        }
    }

    public class DrainStateAttack : StateAttack
    {
        public override string CheckTransition()
        {
            return "Idle";
        }


        public override void Exit()
        {
            if(_OwnerInfo.AttackUnit != null)
            {
                foreach (var unit in _OwnerInfo.AttackUnitList)
                {
                    switch (_OwnerInfo.LookDir)
                    {
                        case Unit.Direction.Up:
                            DrainAttack(unit.CurrTile.Index.Item2 ,_OwnerInfo.CurrTile.Index.Item2,unit);
                            break;
                        case Unit.Direction.Down:
                            DrainAttack(unit.CurrTile.Index.Item2 , _OwnerInfo.CurrTile.Index.Item2, unit);
                            break;
                        case Unit.Direction.Right:
                            DrainAttack(unit.CurrTile.Index.Item1 , _OwnerInfo.CurrTile.Index.Item1, unit);
                            break;
                        case Unit.Direction.Left:
                            DrainAttack(unit.CurrTile.Index.Item1 , _OwnerInfo.CurrTile.Index.Item1, unit);
                            break;
                    }
                    break;
                }
                
            }
        }


        public override bool Initialize()
        {
            return true;
        }

        public override void Logic()
        {
        }

        private void DrainAttack(int position1, int position2, Unit unit)
        {
            if (position1 < position2)
            {
                _OwnerInfo.Status.AddHP(_OwnerInfo.Attack(unit) / 10);
                
            }
        }
    }
}

/*
if (list != null)
            {
                foreach (var unit in list)
                {
                    if (unit == _OwnerInfo.IgnoreUnit)
                        continue;
                    switch (_OwnerInfo.LookDir)
                    {
                        case Unit.Direction.Up:
                            if (unit.CurrTile.Index.Item2 <= _OwnerInfo.CurrTile.Index.Item2)
                            {
                                continue;
                            }
                            break;
                        case Unit.Direction.Down:
                            if (unit.CurrTile.Index.Item2 >= _OwnerInfo.CurrTile.Index.Item2)
                            {
                                continue;
                            }
                            break;
                        case Unit.Direction.Right:
                            if (unit.CurrTile.Index.Item1 <= _OwnerInfo.CurrTile.Index.Item1)
                            {
                                continue;
                            }
                            break;
                        case Unit.Direction.Left:
                            if (unit.CurrTile.Index.Item1 >= _OwnerInfo.CurrTile.Index.Item1)
                            {
                                continue;
                            }
                            break;
                    }
                    _OwnerInfo.ChaseUnit = unit;
                    _OwnerInfo.IgnoreUnit = null;
                    break;
                }
            }
*/