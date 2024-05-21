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
            AddState<ArcherStateAttack>();
            AddState<CommonStateSearch>();
            AddState<CommonStateMove>();
            AddState<OurStateDefault>();
            return true;
        }
    }
}

public class ArcherStateAttack : StateAttack
{
    GameObject[] _Projectile;

    int _ProjectileNum = 0;
    public override string CheckTransition()
    {
        return "Idle";
    }

    public override void Exit()
    {
        var projectile = _Projectile[_ProjectileNum++];
        _OwnerInfo.CurrTile.SummonProjectile(projectile, _OwnerInfo.LookDir);
        projectile.GetComponent<ProjectileUnit>().AttackUnit = _OwnerInfo.AttackUnit;
        _ProjectileNum %= _Projectile.Length;
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


