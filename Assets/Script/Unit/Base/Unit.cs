using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public enum state
{
    Attack,
    Idle,
    Search,
    Move,
    Default
}
public class Unit : MonoBehaviour, IInitializeable
{
    public enum Direction : int
    {
        Up = 6,
        Left = 4,
        Right = 5,
        Down = 0,
        None = -1,
    }
    [SerializeField] private Tile _CurrTile = null;
    [SerializeField] private Tile _NextTile = null;
    [SerializeField] private List<Unit> _AttackUnitList = null;
    [SerializeField] private Unit _AttackUnit = null;
    [SerializeField] private Direction _LookDir;
    [SerializeField] private Unit _ChaseUnit;


    #region Debug


    [SerializeField] private Direction _NextLookDir;
    [SerializeField] private Direction _PathDir;

    [SerializeField] private int _NextLookDirNum;
    [SerializeField] public state _State = 0;

    #endregion




    public Tile CurrTile
    {
        get { return _CurrTile; }
        set { _CurrTile = value; }
    }

    public Tile NextTile
    {
        get { return _NextTile; }
        set { _NextTile = value; }
    }
    public Direction LookDir
    {
        get { return _LookDir; }
        set { _LookDir = value; }
    }

    public Vector3 Position
    { get; set; }

    protected int _EnemyTeamID;


    [SerializeField]
    private UnitStatus _Status;

    public UnitStatus Status
    {
        get
        {
            if (_Status == null)
            {
                _Status = new UnitStatus();
            }
            return _Status;
        }
    }

    public int EnemyTeamID
    {
        get { return _EnemyTeamID; }
        set { _EnemyTeamID = value; }
    }

    public Unit ChaseUnit
    { get { return _ChaseUnit; } set { _ChaseUnit = value; } }

    public Unit IgnoreUnit
    { get; set; }

    public Unit AttackUnit
    { get { return _AttackUnit; } set { _AttackUnit = value; } }

    public Stack<Direction> MovePath { get; set; }


    public List<Unit> AttackUnitList
    {
        get { return _AttackUnitList; }
        set { _AttackUnitList = value; }
    }


    /// <summary>
    /// set clockwise
    /// </summary>
    public void SetLookDirection()
    {
        switch (LookDir)
        {
            case Unit.Direction.Up:
                LookDir = Direction.Right;
                return;
            case Unit.Direction.Down:
                LookDir = Direction.Left;
                return;
            case Unit.Direction.Right:
                LookDir = Direction.Down;
                return;
            case Unit.Direction.Left:
                LookDir = Direction.Up;
                return;
        }
    }

    public void SetLookDirection(Tile tile)
    {
        for (int index = 0; index < _CurrTile.AdjacentTiles.Length; index++)
        {
            if (_CurrTile.AdjacentTiles[index] == tile)
            {
                if(index <= 3)
                {
                    _NextLookDir = Unit.Direction.Down;

                }
                else if(index >= 6)
                {
                    _NextLookDir = Unit.Direction.Up;
                }
                else
                {
                    _NextLookDir = (Unit.Direction)index;
                }
                _NextLookDirNum = index;
                LookDir = _NextLookDir;
                break;
            }
        }
    }

    void Start()
    {
        Initialize();
    }

    public bool Initialize()
    {
        _AttackUnitList = new List<Unit>();
        _Status.Initialize();
        _Status.SpeedDebuff = 0;
        Position = transform.localPosition;
        CurrTile = null;
        ChaseUnit = null;
        AttackUnit = null;

        MovePath = new();
        return true;
    }

    public float Attack(Unit target)
    {
        return target.Status.Damaged(_Status.Damage);
    }


}