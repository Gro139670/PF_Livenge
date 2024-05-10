using System;
using System.Collections.Generic;
using UnityEngine;


public class Unit : MonoBehaviour, IInitializeable
{

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
    { get; set; }

    public Unit IgnoreUnit
    { get; set; }

    public Unit AttackUnit
    { get; set; }

    public Stack<Direction> MovePath { get; set; }

    [SerializeField] List<Unit> _AttackUnitList = new();
    public List<Unit> AttackUnitList
    {
        get { return _AttackUnitList; }
        set { _AttackUnitList = value; }
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

    void Start()
    {
        Initialize();
    }

    public bool Initialize()
    {
        _Status.SpeedDebuff = 0;
        _Position = transform.localPosition;
        CurrTile = null;
        ChaseUnit = null;
        AttackUnit = null;

        MovePath = new();
        return true;
    }

    public void Attack(Unit target)
    {
        target.Status.Damaged(_Status.Damage);
        //Debug.Log(gameObject.name + " Attack : " + target.gameObject.name);
    }
}