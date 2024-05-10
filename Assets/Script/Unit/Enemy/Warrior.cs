using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class Warrior : StateMachine
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