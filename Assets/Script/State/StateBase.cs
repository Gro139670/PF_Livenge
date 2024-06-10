using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public interface IState : IInitializeable
{
    void Enter();
    void FixedLogic();
    void Logic();
    void Exit();

    /// <summary>
    /// 현재 상태에서 다음상태로 가는 전이 검사
    /// </summary>
    /// <returns></returns>
    string CheckTransition();

    bool IsStateFinish
    {
        get; set;
    }

    GameObject Owner { set; }
}


public interface IAllianceState : IState
{
    SortedSet<string> GetAllianceStates();
    
    /// <summary>
    /// 이 공통 상태로 오는 전이 추가
    /// </summary>
    /// <param name="state"></param>
    void AddTransition(string state);

    /// <summary>
    /// 이전 상태들에서 현재 공통 상태로 오는 전이 검사.
    /// </summary>
    /// <returns></returns>
    string CheckAlliance();
}
public abstract class State : IState
{
    protected Unit _OwnerInfo;
    protected GameObject _Owner;

    public GameObject Owner
    {
        set
        {
            if (value != null)
            {
                _Owner = value;
                _OwnerInfo = _Owner.GetComponent<Unit>();
            }
        }
    }
    public abstract string CheckTransition();
    public abstract void Enter();
    public abstract void Exit();
    public abstract void FixedLogic();
    public abstract bool Initialize();

    public abstract void Logic();

    public bool IsStateFinish
    {
        get;
        set;
    }
}

public abstract class AllianceState : State, IAllianceState
{
    public SortedSet<string> _Transition = new();
    public void AddTransition(string state)
    {
        _Transition.Add(state);
    }
    public abstract string CheckAlliance();
    public SortedSet<string> GetAllianceStates()
    { return _Transition; }
}

public class CommonStateDefault : State
{
    public override string CheckTransition()
    {
        return "Idle";
    }

    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void FixedLogic()
    {
    }

    public override bool Initialize()
    {
        return true;
    }

    public override void Logic()
    {
    }
}

public abstract class StateMove : State
{
    protected bool _IsMove = false;
    protected float _MoveTime = 0;
    protected Vector3 _PrevPosition;

    public override void Enter()
    {
        _OwnerInfo._State = state.Move;
        IsStateFinish = false;
        _IsMove = false;
        _MoveTime = 0;

    }

    public override void Exit()
    {
        _OwnerInfo.NextTile = null;
    }

    protected void DoMove()
    {
        if (_IsMove == false)
            return;

        IsStateFinish = TimeManager.Instance.SetTime(ref _MoveTime, _OwnerInfo.Status.MoveSpeed);


        _Owner.transform.localPosition = Vector3.Lerp(_PrevPosition, _OwnerInfo.Position, _MoveTime / _OwnerInfo.Status.MoveSpeed);
    }

    protected virtual void IsMove()
    {
        // todo 다형성을 이용해 소환수에서 함수를 오버라이딩 하자.
        if (_IsMove == true) return;

        _OwnerInfo.SetLookDirection(_OwnerInfo.NextTile);
       
        {
            if(_OwnerInfo.NextTile == null)
            {
                IsStateFinish = true;
                _OwnerInfo.MovePath = null;
                return;
            }

            if (_OwnerInfo.NextTile?.GetTakedUnit() == null)
            {
                _OwnerInfo.CurrTile.SetTakedUnit(null);
                _OwnerInfo.NextTile.SetTakedUnit(_OwnerInfo);
                _Owner.transform.SetParent(_OwnerInfo.CurrTile.gameObject.transform);
                _PrevPosition = _Owner.transform.localPosition;
                _IsMove = true;

            }
            else
            {
                IsStateFinish = true;
                _OwnerInfo.MovePath = null;
            }
        }
    }
}

public abstract class StateAttack : State
{
    protected float _AttackTime = 0;
    public override void Enter()
    {
        _OwnerInfo._State = state.Attack;
        IsStateFinish = false;
        _AttackTime = 0;
    }

    public override void FixedLogic()
    {
        IsStateFinish = TimeManager.Instance.SetTime(ref _AttackTime, _OwnerInfo.Status.AttackSpeed);
    }
}

public class ArcherStateAttack : StateAttack
{
    GameObject[] _Projectile;

    int _ProjectileNum = 0;
    public override string CheckTransition()
    {
        return "CommonStateIdle";
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
        if (_Owner.gameObject.IsDestroyed() == true)
        {
            foreach (var item in _Projectile)
            {
                GameObject.Destroy(item.gameObject);
            }
        }

    }
}

public abstract class ProjectileStateMove : StateMove
{
    public override void Enter()
    {
        base.Enter();
        if (_OwnerInfo.LookDir == Unit.Direction.Up)
        {
            _Owner.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (_OwnerInfo.LookDir == Unit.Direction.Left)
        {
            _Owner.transform.rotation = Quaternion.Euler(0, 0, 90);
        }

            if (_OwnerInfo.LookDir == Unit.Direction.Down)
        {
            _Owner.transform.rotation = Quaternion.Euler(0, 0, 180);
        }


        if (_OwnerInfo.LookDir == Unit.Direction.Right)
        {
            _Owner.transform.rotation = Quaternion.Euler(0, 0, 270);
        }
    }

    protected override void IsMove()
    {
        if (_IsMove == true)
            return;
        
        if (_OwnerInfo.CurrTile.AdjacentTiles[(int)_OwnerInfo.LookDir] != null)
        {
            _OwnerInfo.CurrTile = _OwnerInfo.CurrTile.AdjacentTiles[(int)_OwnerInfo.LookDir];
            _Owner.transform.SetParent(_OwnerInfo.CurrTile.transform);
            _PrevPosition = _Owner.transform.localPosition;
            _IsMove = true;
        }
        else
        {
            IsStateFinish = true;
            _Owner.SetActive(false);
        }
    }

    protected bool IsWidth(Unit.Direction dir)
    {
        if (dir == Unit.Direction.Left || dir == Unit.Direction.Right)
            return false;
        return false;
    }
}

public class TowerStateSearch : State
{
    public override string CheckTransition()
    {
        return "Idle";
    }

    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void FixedLogic()
    {
    }

    public override bool Initialize()
    {
        return true;
    }

    public override void Logic()
    {
    }
}