using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;
using System.Linq;

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
    public Unit GetUnit(int teamID, Func<Unit, bool> condition)
    {
        foreach (var item in _Units[teamID])
        {
            if (condition(item) == true)
                return item;
        }
        return null;
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

    public List<Unit> GetAllUnit(int teamID)
    {
        return _Units[teamID];
    }

    public void Logic()
    {
        foreach (var item in _Units)
        {
            foreach (var unit in item.Value)
            {
                if (unit.gameObject.activeSelf == false)
                {
                    item.Value.Remove(unit);
                    GameObject.Destroy(unit.gameObject);
                    break;
                }
            }
        }
    }
}