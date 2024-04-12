using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Units
{

    static private Dictionary<int, List<UnitInfo>> _Units;
    static public void Initialize()
    {
        if (_Units.Count > 0)
        {
            foreach (var item in _Units.Values)
            {
                item.Clear();
            }
            _Units.Clear();
        }
    }
    static public void Add(int teamID, UnitInfo unit)
    {
        if (_Units == null)
        {
            _Units = new Dictionary<int, List<UnitInfo>>();
        }

        if (_Units.ContainsKey(teamID) == false)
        {
            _Units.Add(teamID, new List<UnitInfo>());
        }
        _Units[teamID].Add(unit);
    }
    static public UnitInfo GetUnits(int teamID, Func<UnitInfo, UnitInfo> condition)
    {
        foreach (var item in _Units[teamID])
        {
            if (condition(item) != null)
                return item;
        }


        return null;
    }

    static public void GetList(int teamID, Func<UnitInfo, UnitInfo> condition, HashSet<UnitInfo> target)
    {
        foreach (var unit in _Units[teamID])
        {
            if (condition(unit) != null)
            {
                target.Add(unit);
            }
        }
    }
}

public class BattleManager : MonoSingleton<BattleManager>
{
    private bool _IsBattle = false;
    private bool _IsBattleStart = false;
    private bool _IsBattleEnd = false;

    private bool _IsCheckBattleResult = false;

    public bool IsBattleStart
    {
        get { return _IsBattleStart; }
        set { _IsBattleStart = value; }
    }

    public bool IsBattle
    {
        get { return _IsBattle; }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
     
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameManager.GetInstance().ChangeScene("Stage1");
            EnemyUnit.EnemyUnitsInit();
        }
        
        if( _IsBattle == true)
        {
            var stage = StageManager.GetInstance();
            stage.DoBattle();
            if(stage.IsBattleEnd == true)
            {
                _IsBattleEnd = true;
                _IsBattle = false;
            }
        }
        else
        {
            if (_IsBattleStart == true)
            {
                _IsBattle = true;
                _IsBattleEnd = false;
                StageManager.GetInstance().StartBattle();
            }
            if (_IsBattleEnd == true)
            {
                if (_IsCheckBattleResult == true)
                {
                    _IsBattleStart = false;
                    StageManager.GetInstance().EndBattle();
                }
            }
        }
        

    }
}
