using System.Collections;
using UnityEngine;

public class EnemyStateDefault : State
{
    private static bool IsInvade = false;

    public override string CheckTransition()
    {
        return "Idle";
    }

    public override void Enter()
    {
        IsStateFinish = IsInvade;
    }

    public override void Exit()
    {
        IsStateFinish = true;
    }

    public override void FixedLogic()
    {
        if(IsInvade == true)
        {
            IsStateFinish = true;
            return;
        }
        foreach(var unit in UnitManager.Instance.GetAllUnit(_OwnerInfo.EnemyTeamID))
        {
            if(unit.CurrTile.Index.Item2 > GameManager.Instance.GetSystem<TileSystem>().Height / 2)
            {
                IsInvade = IsStateFinish = true;

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
}