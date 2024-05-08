using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team
{
    public class Archer : StateMachine
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

