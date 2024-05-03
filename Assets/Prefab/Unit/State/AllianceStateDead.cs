using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AllianceStateDead : AllianceState
{
    public override string CheckAlliance()
    {
        var name = typeof(AllianceStateDead).Name;
        if (_Unit?.Status.IsDead == true)
        {
            return name;
        }
        //if (_Unit.GetCurrTile().GetIndex().Item2 == 5)
        //{
        //    return name;
        //}
        return null;
    }

    public override string CheckTransition()
    {
        return null;
    }

    public override void Enter()
    {
        Owner.SetActive(false);
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