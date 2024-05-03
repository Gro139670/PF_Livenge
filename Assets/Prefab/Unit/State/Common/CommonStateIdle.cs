using System.Collections;
using UnityEngine;

public class CommonStateIdle : State
{   
    public override string CheckTransition()
    {
        if(_Unit)
        {

        }

        return typeof(CommonStateSearch).Name;
    }

    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void FixedLogic()
    {
        _Unit.Status.Test();
    }

    public override bool Initialize()
    {
        return true;
    }

    public override void Logic()
    {
    }
}