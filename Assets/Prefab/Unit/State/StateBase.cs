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

    bool SetUnit(Unit info);

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
    protected Unit _Unit = null;

    public GameObject Owner
    {
        protected get; set;
    }
    public abstract string CheckTransition();
    public abstract void Enter();
    public abstract void Exit();
    public abstract void FixedLogic();
    public abstract bool Initialize();
    public bool SetUnit(Unit info)
    {
        if (info == null)
        {
            return false;
        }
        _Unit = info;

        return true;
    }

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
        return null;
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

public class CommonStateAttack : State
{
    public override string CheckTransition()
    {
        return null;
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