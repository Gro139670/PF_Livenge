using System.Collections.Generic;
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

    protected void IsMove()
    {
        if (_IsMove == true) return;
        if (_OwnerInfo.NextTile.GetTakedUnit() == null)
        {
            _OwnerInfo.SetLookDirection(_OwnerInfo.NextTile);
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