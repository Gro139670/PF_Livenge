﻿using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class Archer : StateMachine
    {
        public override bool Initialize()
        {
            AddState<CommonStateIdle>();
            AddState<CommonStateAttack>();
            AddState<CommonStateSearch>();
            AddState<CommonStateMove>();
            AddState<EnemyStateDefault>();

            return true;
        }
    }
}