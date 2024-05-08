using System.Collections;
using UnityEngine;

public class CommonStateIdle : State
{   
    public override string CheckTransition()
    {
       if(IsCanAttack() == true)
        {
            return "Attack";
        }

        return "Search";
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

    private bool IsCanAttack()
    {
        var attackList = UnitManager.Instance.GetUnitList(_OwnerInfo.EnenmyTeamID,
           unit =>
           {
               if (_OwnerInfo.CurrTile.GetDistance(unit.CurrTile) <= _OwnerInfo.Status.AttackRange)
               {
                   return true;
               }
               return false;
           }
       );

        if (attackList != null)
        {
            _OwnerInfo.AttackUnitList = attackList;
            return true;
        }
        return false;
    }
}