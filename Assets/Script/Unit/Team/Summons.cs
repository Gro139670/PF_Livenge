﻿using System.Collections;
using UnityEngine;

namespace Team
{
    public class Summons : StateMachine
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