using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CommonStateSearch : State
{
    private Unit.Direction _Start;
    private float _SearchCount = 0;
    private bool _IsSearched = true;
    private bool _IsSearchFailed = false;
    public override string CheckTransition()
    {
        if(_OwnerInfo.ChaseUnit != null)
        {
            return "Move";
        }

        return "Default";
    }

    public override void Enter()
    {
        IsStateFinish = false;
        _Start = _OwnerInfo.LookDir;
        _SearchCount = _OwnerInfo.Status.SearchSpeed;
        _IsSearched = _IsSearchFailed = false;


    }

    public override void Exit()
    {
    }

    public override void FixedLogic()
    {

        _SearchCount--;
        
    }

    public override bool Initialize()
    {
        return true;
    }

    public override void Logic()
    {
        if (_OwnerInfo.ChaseUnit != null || _IsSearchFailed == true)
        {
            IsStateFinish = true;
            return;
        }

        if (_SearchCount <= 0)
        {
            if (_IsSearched == true && _Start == _OwnerInfo.LookDir)
            {
                _IsSearchFailed = true;
                return;
            }

            _SearchCount = _OwnerInfo.Status.SearchSpeed;



            var list = UnitManager.Instance.GetUnitList(_OwnerInfo.EnemyTeamID, unit =>
            {
                if (_OwnerInfo.CurrTile.GetDistance(unit.CurrTile) <= _OwnerInfo.Status.ChaseRange * _OwnerInfo.Status.ChaseRange)
                {
                    return true;
                }

                return false;
            });

            if (list != null)
            {
                foreach (var unit in list)
                {
                    if (unit == _OwnerInfo.IgnoreUnit)
                        continue;
                    switch (_OwnerInfo.LookDir)
                    {
                        case Unit.Direction.Up:
                            if (unit.CurrTile.Index.Item2 <= _OwnerInfo.CurrTile.Index.Item2)
                            {
                                continue;
                            }
                            break;
                        case Unit.Direction.Down:
                            if (unit.CurrTile.Index.Item2 >= _OwnerInfo.CurrTile.Index.Item2)
                            {
                                continue;
                            }
                            break;
                        case Unit.Direction.Right:
                            if (unit.CurrTile.Index.Item1 <= _OwnerInfo.CurrTile.Index.Item1)
                            {
                                continue;
                            }
                            break;
                        case Unit.Direction.Left:
                            if (unit.CurrTile.Index.Item1 >= _OwnerInfo.CurrTile.Index.Item1)
                            {
                                continue;
                            }
                            break;
                    }
                    _OwnerInfo.ChaseUnit = unit;
                    _OwnerInfo.IgnoreUnit = null;
                    break;
                }
            }

            _OwnerInfo.LookDir = Unit.GetNextDirection(_OwnerInfo.LookDir);
            _IsSearched = true;
        }

    }
}
