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
            if (_OwnerInfo.NextTile?.GetTakedUnit() != null)
            {
                _OwnerInfo.NextTile = _OwnerInfo.CurrTile.AdjacentTiles[(int)Unit.Direction.Left];
                if (_OwnerInfo.NextTile?.GetTakedUnit() != null)
                {
                    _OwnerInfo.NextTile = _OwnerInfo.CurrTile.AdjacentTiles[(int)Unit.Direction.Right];
                }
            }
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
