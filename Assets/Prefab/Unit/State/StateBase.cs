using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    bool SetUnitInfo(UnitInfo info);
    bool Initialize();
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
        get;
    }
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
[RequireComponent(typeof(UnitInfo))]
public abstract class State : MonoBehaviour, IState
{
    protected UnitInfo _UnitInfo = null;
    public abstract string CheckTransition();
    public abstract void Enter();
    public abstract void Exit();
    public abstract void FixedLogic();
    public abstract bool Initialize();
    public bool SetUnitInfo(UnitInfo info)
    {
        if (info == null)
        {
            return false;
        }
        _UnitInfo = info;

        return true;
    }

    public abstract void Logic();

    public bool IsStateFinish
    {
        get;
        protected set;
    }
}

public abstract class AllianceState : State, IAllianceState
{
    public SortedSet<string> _Transition = new();
    public void AddTransition(string state)
    {
        Debug.Log(_Transition);
        _Transition.Add(state);
    }
    public abstract string CheckAlliance();
    public SortedSet<string> GetAllianceStates()
    { return _Transition; }
}
