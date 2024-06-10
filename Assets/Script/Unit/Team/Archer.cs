using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Team
{
    public class Archer : StateMachine
    {
        public override bool Initialize()
        {
            AddState<CommonStateIdle>();
            AddState<ArcherStateAttack>();
            AddState<CommonStateSearch>();
            AddState<CommonStateMove>();
            AddState<OurStateDefault>();
            return true;
        }
    }
}




