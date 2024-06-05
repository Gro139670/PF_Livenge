using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OurStateDefault : State
{
    public override string CheckTransition()
    {
        return "Move";
    }

    public override void Enter()
    {
        _OwnerInfo._State = state.Default;
        IsStateFinish = false;
    }

    public override void Exit()
    {
    }

    public override void FixedLogic()
    {
        if (IsStateFinish == true)
            return;

        if(_OwnerInfo.CurrTile != null)
        {
            _OwnerInfo.NextTile = _OwnerInfo.CurrTile.AdjacentTiles[(int)Unit.Direction.Up];
            IsStateFinish = true;
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
