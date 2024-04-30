using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CommonStateDead : AllianceState
{
    public override string CheckAlliance()
    {
        var name = typeof(CommonStateDead).Name;
        if (_UnitInfo._Status.IsDead == true)
        {
            return name;
        }
        if(_UnitInfo.GetCurrTile().GetIndex().Item2 == 5)
        {
            return name;
        }
        return null;
    }

    public override string CheckTransition()
    {
        return null;
    }

    public override void Enter()
    {
        gameObject.SetActive(false);
    }

    public override void Exit()
    {
    }

    public override void FixedLogic()
    {
    }


    public override bool Initialize()
    {
        Debug.Log("init");
        return true;
    }

    public override void Logic()
    {
    }
}