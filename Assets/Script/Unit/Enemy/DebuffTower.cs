using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class DebuffTower : StateMachine
    {
        public override bool Initialize()
        {
            AddState<CommonStateIdle>();
            AddState<DebuffTowerStateAttack>();

            return true;
        }

        private class DebuffTowerStateAttack : StateAttack
        {
            HashSet<Unit> _DebuffUnit;
            public override string CheckTransition()
            {
                return "CommonStateIdle";
            }

            public override void Exit()
            {
                foreach (Unit unit in _OwnerInfo.AttackUnitList)
                {
                    _DebuffUnit.Add(unit);
                }

                foreach (Unit unit in _DebuffUnit)
                {
                    unit.Status.SpeedDebuff = 20;
                }
            }

            public override bool Initialize()
            {
                _DebuffUnit = new HashSet<Unit>();

                _OwnerInfo.AddDeadEvent += () => {
                    foreach (Unit unit in _DebuffUnit)
                    {
                        unit.Status.SpeedDebuff = 0;
                    }
                };

                return true;
            }

            public override void Logic()
            {
                
            }
        }
    }
}