using System.Collections;
using UnityEngine;

public class CommonStateAttack : UseSpeedState
{
    private float _AttackTime = 0;

    public override string CheckTransition()
    {
        return "Idle";
    }

    public override void Enter()
    {
        IsStateFinish = false;
        _AttackTime = 0;
    }

    public override void Exit()
    {
        if(_OwnerInfo.AttackUnit != null)
        {
            _OwnerInfo.Attack(_OwnerInfo.AttackUnit);
        }
    }

    public override void FixedLogic()
    {
        IsStateFinish = SetTime(ref _AttackTime, _OwnerInfo.Status.AttackSpeed);
    }

    public override bool Initialize()
    {
        return true;
    }

    public override void Logic()
    {
    }
}