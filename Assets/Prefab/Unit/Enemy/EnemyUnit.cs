using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : UnitInfo
{
    private static List<UnitInfo> _ChaseUnitAllias = new List<UnitInfo>();

    private static bool _IsInvade = false;


    public static void EnemyUnitsInit()
    {
        _ChaseUnitAllias.Clear();
    }

    protected override void Init()
    {
        _LookDir = Direction.Down;
        base.Init();
    }

    public static void AddChaseUnitAllias(UnitInfo unit)
    {
        _ChaseUnitAllias.Add(unit);
    }

    public static bool IsInvade
    {
        get
        {
            if(BattleManager.GetInstance().IsBattleStart == false)
            {
                _IsInvade = false;
            }
            return _IsInvade;
        }
        set
        {
            _IsInvade = value;
        }
    }


    protected void TeamLogic(int teamID, Func<bool> condition)
    {
        

        //switch (_UnitState)
        //{
        //    case State.Defulat:
        //        {
        //            //_ChaseUnit = null;
        //            if(_ChaseUnit != null)
        //            {
        //                _UnitState = State.Chase;
        //                break;
        //            }


        //           if (_ChaseUnitAllias.Count <= 0)
        //           {
        //               _UnitState = State.Idle;
        //           }
        //           else
        //           {
        //               foreach(var unit in _ChaseUnitAllias)
        //               {
        //                   if(unit.GetComponent<UnitInfo>().IsDead == true)
        //                   {
        //                       _ChaseUnitAllias.Remove(unit);
        //                       continue;
        //                   }
        //                   else
        //                   {
        //                       _ChaseUnit = unit;
        //                        break;
        //                   }
        //               }
        //               _UnitState = State.Chase;
        //           }
        //            break;
        //        }

        //}

    }

}
