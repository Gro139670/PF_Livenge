using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemyStateDefault : State
{
    private static bool IsInvade = false;

    public override string CheckTransition()
    {
        return "Move";
    }

    public override void Enter()
    {
        _OwnerInfo._State = state.Default;
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
            if (_OwnerInfo.CurrTile != null)
            {
                _OwnerInfo.NextTile = _OwnerInfo.CurrTile.AdjacentTiles[(int)Unit.Direction.Down];
                if(_OwnerInfo.NextTile == null)
                {
                    _OwnerInfo.ChaseUnit = _OwnerInfo.SearchedUnit?.First();
                }
                IsStateFinish = true;
            }
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