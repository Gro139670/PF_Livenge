using System.Collections.Generic;
using System;
using UnityEditor;
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
    public Unit GetUnits(int teamID, Func<Unit, Unit> condition)
    {
        foreach (var item in _Units[teamID])
        {
            if (condition(item) != null)
                return item;
        }
        return null;
    }

    public List<Unit> GetUnits(int teamID)
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
                    break;
                }
            }
        }
    }
}