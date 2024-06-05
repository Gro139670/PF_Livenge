
using System.Linq;
using UnityEngine;
public class CommonStateSearch : State
{
    private Unit.Direction _Start;
    private float _SearchTime = 0;
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
        _OwnerInfo._State = state.Search;
        IsStateFinish = false;
        _Start = _OwnerInfo.LookDir;
        _IsSearched = _IsSearchFailed = false;


    }

    public override void Exit()
    {
    }

    public override void FixedLogic()
    {
        if (_OwnerInfo.ChaseUnit != null || _IsSearchFailed == true)
        {
            IsStateFinish = true;
            return;
        }

        int length = _OwnerInfo.SearchedUnit.Count;
        if (length > 0)
        {
            _OwnerInfo.ChaseUnit = _OwnerInfo.SearchedUnit.ElementAt(Random.Range(0, length));
            return;
        }

        if (TimeManager.Instance.SetTime(ref _SearchTime, _OwnerInfo.Status.SearchSpeed) == true)
        {
            if (_IsSearched == true && _Start == _OwnerInfo.LookDir)
            {
                _OwnerInfo.ChaseUnit = null;
                _IsSearchFailed = true;
                
                return;
            }

            _SearchTime = 0;



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

            _OwnerInfo.SetLookDirection();
            _IsSearched = true;
        }
    }

    public override bool Initialize()
    {
        return true;
    }

    public override void Logic()
    {
       

    }
}
