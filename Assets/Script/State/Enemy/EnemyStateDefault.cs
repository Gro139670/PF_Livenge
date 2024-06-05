using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemyStateDefault : State
{
    private bool IsInvade = false;

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
        IsStateFinish = true;
    }

    public override void FixedLogic()
    {
        if (IsInvade == true)
        {
            if (_OwnerInfo.CurrTile != null)
            {
                _OwnerInfo.NextTile = _OwnerInfo.CurrTile.AdjacentTiles[(int)Unit.Direction.Down];
                IsStateFinish = true;
            }
            return;
        }
        else
        {
            foreach (var unit in UnitManager.Instance.GetAllTeamUnit(_OwnerInfo.EnemyTeamID))
            {
                if (unit.CurrTile.Index.Item2 > GameManager.Instance.GetSystem<TileSystem>().HeightIndex / 2)
                {
                    IsInvade = IsStateFinish = true;

                }
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