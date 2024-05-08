using System;
using System.Collections.Generic;
using UnityEngine;


public class Unit : MonoBehaviour, IInitializeable
{
    public Unit()
    {
        Initialize();
    }

    public enum Direction
    {
        Up = 6,
        Left = 4,
        Right = 5,
        Down = 0,
        None = -1,
    }


    [SerializeField] private Tile _CurrTile = null;
    [SerializeField] private Tile _NextTile = null;

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
    { get; set; }


    public Vector3 _Position;

    [SerializeField]    
    protected int _EnemTeamID;


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

    public int EnenmyTeamID
    {
        get { return _EnemTeamID; }
    }

    [SerializeField] public Unit ChaseUnit
    { get; set; }

    public Unit IgnoreUnit
    { get; set; }

    public Stack<Direction> MovePath { get; set; }

    public List<Unit> AttackUnitList
    {
        get; set;
    }

    public bool Initialize()
    {
        CurrTile = null;
        ChaseUnit = null;
        LookDir = Direction.Down;
        MovePath = new();
        return true;
    }

    public static Direction GetNextDirection(Direction dir)
    {
        switch (dir)
        {
            case Unit.Direction.Up:
                return Direction.Right;

            case Unit.Direction.Down:
                return Direction.Left;

            case Unit.Direction.Right:
                return Direction.Down;

            case Unit.Direction.Left:
                return Direction.Up;
        }

        return dir;
    }

}