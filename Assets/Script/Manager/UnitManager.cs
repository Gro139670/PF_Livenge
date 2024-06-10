using System.Collections.Generic;
using System;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{

    private Dictionary<int, List<Unit>> _Units;
    public override bool Initialize()
    {
        _Units = new();
        ClearUnits();
        return true;
    }
    public void ClearUnits()
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
    public void ClearTeamUnits(int num)
    {
        foreach (var item in _Units[num])
        {
            item.gameObject.SetActive(false);
        }
        Debug.Log(_Units[num]); 
    }
    public void AddTeam(int teamID)
    {
        if (_Units.ContainsKey(teamID) == false)
        {
            _Units.Add(teamID, new List<Unit>());
        }
    }
    public void AddUnit(int teamID, Unit unit)
    {
        if (_Units == null)
        {
            _Units = new Dictionary<int, List<Unit>>();
        }

        if (_Units.ContainsKey(teamID) == false)
        {
            _Units.Add(teamID, new List<Unit>());
        }
        _Units[teamID].Add(unit);
    }

    public List<Unit> GetUnitList(int teamID, Func<Unit, bool> condition)
    {
        // todo new로 생성 말고 리스트 레퍼런스를 받아서 쓰자.
        List<Unit> list = null;
        if (_Units[teamID] != null)
        {
            list = new List<Unit>();
            foreach (var item in _Units[teamID])
            {
                if (condition(item) == true)
                    list.Add(item);
            }
            if (list.Count <= 0)
                list = null;
        }
        return list;
    }

    public List<Unit> GetAllTeamUnit(int teamID)
    {
        return _Units[teamID];
    }

    public Dictionary<int, List<Unit>> GetAllUnit()
    { return _Units; }

    public void Logic()
    {
        foreach (var item in _Units)
        {
            item.Value.RemoveAll(unit =>
            {
                if(unit.gameObject.activeSelf == false)
                {
                    GameObject.Destroy(unit.gameObject);
                    return true;
                }
                return false;
            });
        }
    }
}