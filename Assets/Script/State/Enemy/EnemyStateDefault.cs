using System.Collections;
using UnityEngine;

public class EnemyStateDefault : State
{

    public override string CheckTransition()
    {
        return "Idle";
    }

    public override void Enter()
    {
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