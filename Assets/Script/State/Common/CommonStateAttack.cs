using System.Collections;
using UnityEngine;

public class CommonStateAttack : StateAttack
{


    public override string CheckTransition()
    {
        return "Idle";
    }

   

    public override void Exit()
    {
        if(_OwnerInfo.AttackUnit != null)
        {
            _OwnerInfo.AttackEnemy(_OwnerInfo.AttackUnit);
        }
    }

    

    public override bool Initialize()
    {
        return true;
    }

    public override void Logic()
    {
    }
}