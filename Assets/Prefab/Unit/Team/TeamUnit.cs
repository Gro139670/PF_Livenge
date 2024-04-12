using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TeamUnit : UnitInfo
{

    private bool _IsInvadeLimitLine = false;

    protected override void Init()
    {
        _LookDir = Direction.Up;
        base.Init();
    }

    protected void TeamLogic(int teamID, Func<bool> condition)
    {
        if (EnemyUnit.IsInvade == false)
        {
            if (_CurrIndex.Item2 >= Tile._MaxTileSize.Item2 / 2)
            {
                EnemyUnit.IsInvade = true;
            }
        }
        if (_IsInvadeLimitLine == false)
        {
            if (_CurrIndex.Item2 >= (Tile._MaxTileSize.Item2 / 4) * 3)
            {
                _IsInvadeLimitLine = true;
                EnemyUnit.AddChaseUnitAllias(this);
            }
        }

        switch (_UnitState)
        {
            case State.Defulat:
                {
                    UnitState = State.Idle;
                    if (_CurrTile.AdjacentTiles[(int)Direction.Up] != null)
                    {
                        if(IsCanMove(_CurrTile.AdjacentTiles[(int)Direction.Up]) == true)
                        {
                            UnitState = State.Move;
                        } 
                    }
                    break;
                }
        }

    }
}
