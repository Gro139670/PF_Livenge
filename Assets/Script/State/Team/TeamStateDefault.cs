using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamStateDefault : State
{
    public override string CheckTransition()
    {
        return "Move";
    }

    public override void Enter()
    {
        _OwnerInfo.NextTile = _OwnerInfo.CurrTile.AdjacentTiles[(int)Unit.Direction.Up];
        IsStateFinish = true;
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
