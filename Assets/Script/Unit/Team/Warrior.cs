using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team
{
    public class Warrior : StateMachine
    {
        public override bool Initialize()
        {
            AddState<CommonStateIdle>();
            AddState<CommonStateAttack>();
            AddState<CommonStateSearch>();
            AddState<CommonStateMove>();
            AddState<TeamStateDefault>();
            return true;
        }
    }
}

