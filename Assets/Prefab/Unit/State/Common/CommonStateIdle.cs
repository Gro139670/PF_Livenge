using System.Collections;
using UnityEngine;

public class CommonStateIdle : State
{   
    public override string CheckTransition()
    {
        return null;
    }

    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void FixedLogic()
    {
        _UnitInfo._Status.Test();
    }

    public override bool Initialize()
    {
        return true;
    }

    public override void Logic()
    {
    }
}