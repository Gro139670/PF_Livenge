using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


public class CommonStateMove : UseSpeedState
{
    private bool _IsMove = false;
    private float _MoveTime = 0;
    private Vector3 _PrevPosition;
    public override string CheckTransition()
    {
        return "Idle";
    }

    public override void Enter()
    {
        IsStateFinish = false;
        _IsMove = false;
        _MoveTime = 0;
    }

    public override void Exit()
    {
        _OwnerInfo.NextTile = null;
    }

    public override void FixedLogic()
    {
        if (_IsMove == false)
            return;


        IsStateFinish = SetTime(ref _MoveTime, _OwnerInfo.Status.MoveSpeed);

        // debug
        _MoveTime = _OwnerInfo.Status.MoveSpeed;


        _Owner.transform.localPosition = Vector3.Lerp(_PrevPosition, _OwnerInfo._Position, _MoveTime / _OwnerInfo.Status.MoveSpeed);
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
                _OwnerInfo.NextTile.SetTakedUnit(_OwnerInfo);
                _Owner.transform.SetParent(_OwnerInfo.CurrTile.gameObject.transform);
                _PrevPosition = _Owner.transform.localPosition;
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
