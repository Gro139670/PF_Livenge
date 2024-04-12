using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tWarrior : TeamUnit
{
    protected override void DoBattle()
    {
        StateLogic(2, TrueLambda);
    }


    protected override void StateLogic(int teamID, Func<bool> condition)
    {
        switch (_UnitState)
        {
            
        }



        TeamLogic(2, TrueLambda);
        BaseLogic(2);
    }

}
