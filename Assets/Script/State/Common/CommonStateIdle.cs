using System.Collections;
using System.Linq;
using UnityEngine;

public class CommonStateIdle : State
{   
    public override string CheckTransition()
    {
       if(IsCanAttack() == true)
        {
            _OwnerInfo.SetLookDirection(_OwnerInfo.AttackUnit.CurrTile);
            return "Attack";
        }

        return "Search";
    }

    public override void Enter()
    {
        _OwnerInfo._State = state.Idle;
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

    private bool IsCanAttack()
    {
        if (_OwnerInfo.AttackUnitList?.Count > 0)
        {
            if (_OwnerInfo.AttackUnitList != null)
            {
                _OwnerInfo.AttackUnitList.OrderBy(
                unit =>
                {
                    return _OwnerInfo.CurrTile.GetDistance(unit?.CurrTile);
                });
                _OwnerInfo.AttackUnit = _OwnerInfo.AttackUnitList.First();
            }

            {
                if (_OwnerInfo.AttackUnitList?.Count > 0)
                {
                    foreach (var unit in _OwnerInfo.AttackUnitList)
                    {
                        if (unit == null)
                        {
                            _OwnerInfo.AttackUnitList.Remove(unit);
                            break;

                        }

                        if (_OwnerInfo.CurrTile.GetDistance(unit.CurrTile) > _OwnerInfo.Status.AttackRange * _OwnerInfo.Status.AttackRange)
                        {
                            _OwnerInfo.AttackUnitList.Remove(unit);
                            break;

                        }
                    }
                }
            }

            if (_OwnerInfo.AttackUnit == null)
            {
                _OwnerInfo.AttackUnitList = null;
                return false;
            }

            return true;
        }

        _OwnerInfo.AttackUnitList = UnitManager.Instance.GetUnitList(_OwnerInfo.EnemyTeamID,
           unit =>
           {
               if (_OwnerInfo.CurrTile.GetDistance(unit.CurrTile) <= _OwnerInfo.Status.AttackRange * _OwnerInfo.Status.AttackRange)
               {
                   return true;
               }
               return false;
           });

        

        return false;
    }
}