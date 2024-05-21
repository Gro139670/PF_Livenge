
using UnityEngine;

namespace Team
{
    public class Reaper : StateMachine
    {
        public override bool Initialize()
        {
            AddState<CommonStateIdle>();
            AddState<ReaperStateAttack>();
            AddState<CommonStateSearch>();
            AddState<CommonStateMove>();
            AddState<OurStateDefault>();

            ChangeState<ReaperStateAttack>();
            return true;
        }

        private class ReaperStateAttack : StateAttack
        {
            GameObject[] _Projectile;

            int _ProjectileNum = 0;

            public override string CheckTransition()
            {
                return "Default";
            }

            public override void Enter()
            {
            }

            public override void Exit()
            {
                _OwnerInfo.CurrTile.AdjacentTiles[(int)_OwnerInfo.LookDir]?.SummonProjectile(_Projectile[_ProjectileNum++], _OwnerInfo.LookDir);
                _ProjectileNum %= _Projectile.Length;
            }

            public override void FixedLogic()
            {
            }

            public override bool Initialize()
            {
                // todo 시간이 되면 동적으로 갯수 늘릴것
                _Projectile = new GameObject[5];
                for (int i = 0; i < _Projectile.Length; i++)
                {
                    _Projectile[i] = GameObject.Instantiate(_OwnerInfo.SummonUnit[0]);
                    var unit = _Projectile[i].GetComponent<ProjectileUnit>();
                    unit.Status.Damage = _OwnerInfo.Status.Damage;
                    unit.EnemyTeamID = _OwnerInfo.EnemyTeamID;
                    unit.Status.TeamID = _OwnerInfo.Status.TeamID;
                    unit.Owner = _Owner;
                    _Projectile[i].SetActive(false);
                }
                return true;
            }

            public override void Logic()
            {
            }
        }
    }

    
}