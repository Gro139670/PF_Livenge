using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class StateMachine : UnitHelper, IInitializeable
{
    // Start is called before the first frame update
    private IState _CurrState = null;
    private Dictionary<string,IAllianceState> _AllianceStates = new();
    private Dictionary<string, IState> _States = new Dictionary<string, IState>();

    private void Start()
    {
        Initialize();
        if(ChangeState("Default") == false)
        {
            ChangeState("Idle");
        }
    }

    private void FixedUpdate()
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
        state.Initialize();
        state.Owner = gameObject;
        state.IsStateFinish = true;
        if (_AllianceStates.ContainsKey(typeof(AllianceStateDead).Name) == false)
        {
            var alliance =  AddAlliancetState<AllianceStateDead>();
        }
        _AllianceStates[typeof(AllianceStateDead).Name].AddTransition(name);
        _States.Add(name, state);
        return state;
    }

    public T AddAlliancetState<T>() where T : IAllianceState, new()
    {
        string name = typeof(T).Name;
        if (_AllianceStates.ContainsKey(name))
        {
            return (T)_AllianceStates[name];
        }

        T alliance = new T();

        alliance.Initialize();
        alliance.Owner = gameObject;
        alliance.IsStateFinish = true;

        _AllianceStates.Add(name, alliance);
        return alliance;
    }

    private bool ChangeState(string name)
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

    private bool ChangeState(IState state)
    {
        _CurrState?.Exit();
        _CurrState = state;
        _CurrState.Enter();
        return true;
    }

    public abstract bool Initialize();
}
