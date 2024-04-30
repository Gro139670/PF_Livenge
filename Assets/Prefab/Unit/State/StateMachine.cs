using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class StateMachine : MonoBehaviour
{
    // Start is called before the first frame update
    private IState _CurrState = null;
    private Dictionary<string,IAllianceState> _AllianceStates = new();
    private Dictionary<string, IState> _States = new Dictionary<string, IState>();

    private void Start()
    {
        AddState<CommonStateIdle>();
        ChangeState(typeof(CommonStateIdle).Name);
    }

    private void FixedUpdate()
    {
        if (BattleManager.GetInstance().IsBattle == true)
        {
            _CurrState?.FixedLogic();
        }
    }
    void Update()
    {

        // Debug
        if(_CurrState != null)
        {
            Transition();
            _CurrState.Logic();
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


    public T AddState<T>() where T : MonoBehaviour, IState, new()
    {
        string name = typeof(T).Name;
        if (_States.ContainsKey(name))
        {
            return (T)_States[name];
        }
        T state = gameObject.AddComponent<T>();
        state.SetUnitInfo(gameObject.GetComponent<UnitInfo>());
        state.Initialize();

        if(_AllianceStates.ContainsKey(typeof(CommonStateDead).Name) == false)
        {
            var alliance =  AddAlliancetState<CommonStateDead>();
            alliance.AddTransition(name);
        }
        _States.Add(name, state);
        return state;
    }

    public T AddAlliancetState<T>() where T : MonoBehaviour, IAllianceState, new()
    {
        string name = typeof(T).Name;
        if (_AllianceStates.ContainsKey(name))
        {
            return (T)_AllianceStates[name];
        }

        T alliance = gameObject.AddComponent<T>();
        alliance.SetUnitInfo(gameObject.GetComponent<UnitInfo>());
        alliance.Initialize();
        _AllianceStates.Add(name, alliance);
        return alliance;
    }

    private void ChangeState(string name)
    {

        _CurrState?.Exit();
        _CurrState = _States[name];

        _CurrState.Enter();
    }

    private void ChangeState(IState state)
    {
        _CurrState.Exit();
        _CurrState = state;
        _CurrState.Enter();
    }
}
