using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class CommonStateMove : State
{
    private float dsad;
    private bool _IsMove = false;
    public override string CheckTransition()
    {
        return "Idle";
    }

    public override void Enter()
    {
        IsStateFinish = false;
        _IsMove = false;
        // test
       

    }

    public override void Exit()
    {
        _OwnerInfo.NextTile = null;
        _IsMove = false;
    }

    public override void FixedLogic()
    {
        if (_IsMove == false)
            return;

        _OwnerInfo.NextTile.SetTakedUnit(_OwnerInfo);
        _Owner.transform.SetParent(_OwnerInfo.CurrTile.gameObject.transform);
        _Owner.transform.localPosition = _OwnerInfo._Position;
        IsStateFinish = true;
    }

    public override bool Initialize()
    {
        return true;
    }

    public override void Logic()
    {
        if (_IsMove == true)
            return;

        if(_OwnerInfo.NextTile != null)
        {
            if(_OwnerInfo.NextTile.GetTakedUnit() == null)
            {
                for (int index = 0; index < _OwnerInfo.CurrTile.AdjacentTiles.Length; index++)
                {
                    if (_OwnerInfo.CurrTile.AdjacentTiles[index] == _OwnerInfo.NextTile)
                    {
                        _OwnerInfo.LookDir = ((Unit.Direction)index);
                        break;
                    }
                }
                _IsMove = true;
            }
            else
            {
                IsStateFinish = true;
            }
        }
        else
        {
            if(_OwnerInfo.ChaseUnit == null)
            {
                IsStateFinish = true;
                return;
            }

            if(_OwnerInfo.MovePath?.Count > 0)
            {
                _OwnerInfo.LookDir = _OwnerInfo.MovePath.Pop();
                _OwnerInfo.NextTile = _OwnerInfo.CurrTile.AdjacentTiles[(int)_OwnerInfo.LookDir];
            }
            else
            {
                _OwnerInfo.MovePath = _OwnerInfo.CurrTile.AStar(_OwnerInfo.ChaseUnit.CurrTile);
                if( _OwnerInfo.MovePath == null )
                {
                    _OwnerInfo.IgnoreUnit = _OwnerInfo.ChaseUnit;
                    _OwnerInfo.ChaseUnit = null;
                    _OwnerInfo.MovePath = null;
                    IsStateFinish = true;

                }
            }
        }
    }
}
