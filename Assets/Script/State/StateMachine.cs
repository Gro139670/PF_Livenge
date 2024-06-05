using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class StateMachine : UnitHelper, IInitializeable
{
    // Start is called before the first frame update
    private IState _CurrState = null;
    private IState _StartState;
    private Dictionary<string,IAllianceState> _AllianceStates = new();
    private Dictionary<string, IState> _States = new Dictionary<string, IState>();

    private void Start()
    {
        Initialize();
        if (_CurrState == null)
        {
            if (ChangeState("Default") == false)
            {
                ChangeState("Idle");
            }
        }
        _StartState = _CurrState;
        GameManager.Instance.GetSystem<StageSystem>().RoundStart += RoundStart;
    }

    protected void FixedUpdate()
    {
        if (GameManager.Instance.GetSystem<StageSystem>().IsBattle == true)
        {
            _CurrState?.FixedLogic();
        }
    }
    void Update()
    {
        if (GameManager.Instance.GetSystem<StageSystem>().IsBattle == true)
        {
            // Debug
            if (_CurrState != null)
            {
                Transition();
                _CurrState.Logic();
            }
        }

    }
    void OnDestroy()
    {
        GameManager.Instance.GetSystem<StageSystem>().RoundStart -= RoundStart;
    }

    private void RoundStart()
    {
        _CurrState = _StartState;
    }
    private void Transition()
    {
        string transitionResult = null;
        foreach (var alliance in _AllianceStates)
        {
            foreach (var state in alliance.Value.GetAllianceStates())
            {
                if (state.Equals(_CurrState.GetType().Name))
                {
                    transitionResult = alliance.Value.CheckAlliance();
                    if(transitionResult != null)
                    {
                        ChangeState(alliance.Value);
                        return;
                    }
                }
            }
        }

        if(_CurrState.IsStateFinish == true)
        {
            transitionResult = _CurrState.CheckTransition();
            if (transitionResult != null)
            {
                ChangeState(transitionResult);
                return;
            }
        }
    }


    public T AddState<T>() where T : IState, new()
    {
        string name = typeof(T).Name;
        if (_States.ContainsKey(name))
        {
            return (T)_States[name];
        }
        T state = new T();
        state.Owner = gameObject;
        state.IsStateFinish = true;
        
        if (_AllianceStates.ContainsKey(typeof(AllianceStateDead).Name) == false)
        {
            var alliance =  AddAlliancetState<AllianceStateDead>();
        }
        _AllianceStates[typeof(AllianceStateDead).Name].AddTransition(name);
        _States.Add(name, state);
        state.Initialize();
        return state;
    }

    public T GetState<T>() where T : class, IState, new()
    {
        string name = typeof(T).Name;
        if(_States.ContainsKey(name) == false)
        {
            return null;
        }
        return (T)_States[name];
    }

    public T AddAlliancetState<T>() where T : IAllianceState, new()
    {
        string name = typeof(T).Name;
        if (_AllianceStates.ContainsKey(name))
        {
            return (T)_AllianceStates[name];
        }

        T alliance = new T();

        alliance.Owner = gameObject;
        alliance.IsStateFinish = true;

        alliance.Initialize();
        _AllianceStates.Add(name, alliance);
        _States.Add(name, alliance);
        return alliance;
    }

    protected bool ChangeState(string name)
    {
        bool result = false;
        _CurrState?.Exit();
        if(_States.ContainsKey(name) == false)
        {
            foreach(var state in _States)  
            {
                if(state.Key.Contains(name) == true)
                {
                    _CurrState = state.Value;
                    result = true;
                    break;
                }
            }
            if(result == false)
            {
                // todo : 찾는 상태가 없다면 기본 상태중에서 맞는것을 찾아서 추가한다.
                return result;
            }
        }
        else
        {
            _CurrState = _States[name];
        }
        _CurrState.Enter();
        return result;
    }

    protected bool ChangeState(IState state)
    {
        _CurrState?.Exit();
        _CurrState = state;
        _CurrState.Enter();
        return true;
    }

    protected bool ChangeState<T>() where T : class, IState
    {
        _CurrState?.Exit();
        _CurrState = _States[typeof(T).Name];
        _CurrState.Enter();
        return true;
    }

    public abstract bool Initialize();
}

public abstract class ProjectileStateMachine : StateMachine
{
    private new void FixedUpdate()
    {
        base.FixedUpdate();
        if (GameManager.Instance.GetSystem<StageSystem>().IsBattle == false)
        {
            gameObject.SetActive(false);
        }
    }
}