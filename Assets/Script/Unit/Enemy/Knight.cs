using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class Knight : StateMachine
    {
        public override bool Initialize()
        {
            AddState<CommonStateIdle>();
            AddState<KightStateAttack>();
            AddState<CommonStateSearch>();
            AddState<CommonStateMove>();
            AddState<EnemyStateDefault>();

            return true;
        }

        private class KightStateAttack : StateAttack
        {
            public override string CheckTransition()
            {
                return "CommonStateIdle";
            }

            public override void Exit()
            {
                if (_OwnerInfo.AttackUnitList.Count <= 0)
                    return;

                float damage = _OwnerInfo.Status.Damage / _OwnerInfo.AttackUnitList.Count;
                foreach (var unit in _OwnerInfo.AttackUnitList)
                {
                    _OwnerInfo.AttackEnemy(unit, damage);
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
    }
}