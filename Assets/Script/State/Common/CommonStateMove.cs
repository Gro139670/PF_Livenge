using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


public class CommonStateMove : StateMove
{

    public override string CheckTransition()
    {
        return "Idle";
    }

   

    

    public override void FixedLogic()
    {
        DoMove();
    }

    public override bool Initialize()
    {
        return true;
    }

    public override void Logic()
    {
        if (_IsMove == true)
            return;

        if (_OwnerInfo.NextTile != null)
        {
            IsMove();
        }
        else
        {

            if (_OwnerInfo.ChaseUnit == null)
            {

                IsStateFinish = true;
                return;
            }
            if (_OwnerInfo.MovePath?.Count > 0)
            {

                _OwnerInfo.LookDir = _OwnerInfo.MovePath.Pop();
                _OwnerInfo.NextTile = _OwnerInfo.CurrTile.AdjacentTiles[(int)_OwnerInfo.LookDir];
            }
            else
            {
                _OwnerInfo.MovePath = _OwnerInfo.CurrTile.AStar(_OwnerInfo.ChaseUnit.CurrTile);
                if( _OwnerInfo.MovePath == null || _OwnerInfo.MovePath.Count <= 0)
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
