using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
public abstract class UnitInfo : MonoBehaviour
{

    public enum State
    {
        Defulat,
        Idle,
        Attack,
        Move,
        Chase,
    };

    public enum Direction
    {
        Up = 6,
        Left = 4,
        Right = 5,
        Down = 0,
        None = -1,
    }

    public Tile _CurrTile = null;
    public Tile _NextTile = null;
    public Tile _PrevTile = null;
    protected Tuple<int, int> _CurrIndex;
    protected Tuple<int, int> _StartIndex;
    [SerializeField] protected HashSet<UnitInfo> _AttackUnitList = new HashSet<UnitInfo>();
    [SerializeField] protected HashSet<UnitInfo> _ChaseUnitList = new HashSet<UnitInfo>();

    [SerializeField] protected UnitInfo _ChaseUnit = null;

    [SerializeField] protected State _UnitState = State.Idle;
    [SerializeField] protected Direction _LookDir = Direction.None;
    public Vector3 _Position;

    static protected Func<bool> TrueLambda = () => { return true; };

    private bool _IsDead = false;
    protected bool _IsCompleteMove = true;
    public static float _TileDistance = 10000.0f;
    private float _MoveProgress = 0;
    private bool _IsTakingTile = false;





    #region Status

    [Header("Common")]
    [SerializeField] private string _Name;
    [SerializeField] private int _UnitTear;
    [SerializeField] private int _TeamID;

    [Header("Status")]
    [SerializeField] private float _HP = 100;
    [SerializeField] private float _Damage;
    [SerializeField] private float _Defense;
    [SerializeField] private float _AttackSpeed;
    [SerializeField] private float _MoveSpeed;
    [SerializeField] private float _Size = 2;

    [Header("Cost")]
    [SerializeField] private int _ManaCost;
    [SerializeField] private int _LifeCost;

    [Header("Range")]
    [SerializeField] private float _AttackRange = 1;
    [SerializeField] private float _ChaseRange = 1;
    [SerializeField] private float _SightRange = 1;
    #endregion
    #region StatusFunc
    public int TeamID
    { get { return _TeamID; } }

    public float Size
    { get { return _Size; } }

    public float AttackRange
    { get { return _AttackRange; } }
    public float SightRange
    { get { return _SightRange; } }

    public float ChaseRange
    { get { return _ChaseRange; } }

    #endregion

    protected virtual void Init()
    {
        if (_ChaseRange <= 0)
            _ChaseRange = _AttackRange + _UnitTear;
        if (_SightRange <= 0)
            _SightRange = 1 + (_UnitTear * 1.2f);

        Units.Add(_TeamID, this);
    }



    private void Start()
    {
        Init();
    }

    private void FixedUpdate()
    {
        if (BattleManager.GetInstance().IsBattle == true)
        {
            Action();
        }
    }

    private void Update()
    {
        if (BattleManager.GetInstance().IsBattle == true)
        {
            DoBattle();
        }    
    }

    public State UnitState
    {
        get { return _UnitState; }
        set
        {
            if (value == State.Move)
            {
                _IsCompleteMove = false;
                _MoveProgress = 0;
                _Position = gameObject.transform.localPosition;
            }
            _UnitState = value;
        }
    }

    public Tile GetCurrTile()
    {
        return _CurrTile;
    }
    public void SetCurrTile(Tile tile)
    {
        _CurrTile = tile;
    }

    public bool IsCompleteMove
    {
        get { return _IsCompleteMove; }
    }

    public Tuple<int, int> CurrIndex
    {
        get { return _CurrIndex; }
        set { _CurrIndex = value; }
    }

    public UnitInfo ChaseUnit
    {
        get { return _ChaseUnit; }
    }

    public Tuple<int, int> GetStartIndex
    {
        get { return _StartIndex; }
    }

    public bool IsDead
    {
        get
        {
            if (_HP <= 0)
            {
                _IsDead = true;
                _CurrTile.RemovePassingUnit(this);
            }
            else
            {
                _IsDead = false;
            }
            return _IsDead;
        }
    }

    public bool IsTakingTile
    {
        get
        {
            if (_CurrTile.TakedUnit == this)
            {
                _IsTakingTile = true;
            }
            else
            {
                _IsTakingTile = false;
            }
            return _IsTakingTile;
        }
    }
    protected abstract void StateLogic(int teamID, Func<bool> condition);
    protected abstract void DoBattle();

    protected virtual void IDLELogic(int teamID)
    {


        if (IsTakingTile == true)
        {
            FindEnemy(teamID);

            if (_ChaseUnit != null)
            {
                UnitState = State.Chase;
                return;
            }

            // 여기서 무한 디폴트 상태. 뭔가 해야할듯
            UnitState = State.Defulat;
        }
        else
        {
            {
                int x, y, result, min;
                result = int.MaxValue;
                UnitInfo nextUnit = null;
                foreach (var tile in Tile._FarTile)
                {

                    x = tile.GetComponent<Tile>().GetIndex().Item1 - CurrIndex.Item1;
                    y = tile.GetComponent<Tile>().GetIndex().Item2 - CurrIndex.Item2;
                    min = (x * x) + (y * y);
                    if (result > min)
                    {
                        result = min;
                        //nextUnit = tile.GetComponent<Tile>();
                    }
                }

                _ChaseUnit = nextUnit;
                UnitState = State.Chase;

            }
        }
    }

    protected virtual void ATTACKLogic()
    {
        if (_AttackUnitList.Count <= 0)
        {
            UnitState = State.Idle;
        }
        foreach (var unit in _AttackUnitList)
        {
            if (unit.IsDead == true)
            {
                UnitState = State.Idle;
                break;
            }
        }
    }

    protected virtual void CHASELogic(int teamID)
    {
        if (ChaseUnit == null)
        {
            UnitState = State.Idle;
            return;
        }

        if (IsTakingTile == true)
        {
            FindUnit(_AttackRange, teamID, _AttackUnitList);

            if (_AttackUnitList.Count > 0)
            {
                UnitState = State.Attack;
                return;
            }
        }

        if (IsCanMoveAttackUnit(_AttackRange, _ChaseUnit, TrueLambda) == true)
        {
            UnitState = State.Move;
            return;
        }

        _ChaseUnit = null;
        UnitState = State.Defulat;
    }

    protected virtual void MOVELogic()
    {
        if (_NextTile == null)
        {
            UnitState = State.Idle;
            return;
        }
    }

    protected virtual void IDLEAction()
    { }
    protected virtual void ATTACKAction() { }
    protected virtual void CHASEAction() { }
    protected virtual void MOVEAction()
    {
        float moveSpeed = _MoveSpeed;

        if (_NextTile.TakedUnit != null)
        {
            moveSpeed = _MoveSpeed / _NextTile.TakedUnit.GetComponent<UnitInfo>()._Size;
        }

        _MoveProgress += moveSpeed;
        transform.parent = _NextTile.gameObject.transform;
        if (_MoveProgress >= _TileDistance)
        {
            _IsCompleteMove = true;
            _MoveProgress = _TileDistance;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, _Position, (_MoveProgress / _TileDistance));
    }

    protected void BaseLogic(int teamID)
    {
        
        
        switch (_UnitState)
        {

            case State.Idle:
                {
                    IDLELogic(teamID);
                    break;
                }
            case State.Attack:
                {
                    ATTACKLogic();
                    break;
                }
            case State.Chase:
                {
                    CHASELogic(teamID);
                    break;
                }
            case State.Move:
                {
                    MOVELogic();
                    break;
                }
        }

    }

    private void Action()
    {
        switch (_UnitState)
        {

            case State.Idle:
                {
                    IDLEAction();
                    break;
                }
            case State.Attack:
                {
                    ATTACKAction();
                    break;
                }
            case State.Chase:
                {
                    CHASEAction();
                    break;
                }
            case State.Move:
                {
                    MOVEAction() ;
                    break;
                }
        }
    }
    
    void FindUnit(float range, int teamID, HashSet<UnitInfo> target)
    {

        Units.GetList(teamID,
            unit =>
            {
                if (unit.GetDistance(this) <= range)
                {
                    return unit;
                }
                return null;
            },
            target
            );
    }


    protected bool FindTile(float range, int teamID,Tile tile, Func<bool> condition, List<UnitInfo> target,Direction lookDir, float cost = 0)
    {
        if (target.Count > 0)
        {
            return true;
        }

        if (lookDir != Direction.None)
        {
            Tuple<int, int> index = tile.GetIndex();
            switch (lookDir)
            {

                case Direction.Left:
                    {
                        if (index.Item1 > _CurrIndex.Item1)
                        {
                            return false;
                        }
                        break;
                    }
                case Direction.Right:
                    {
                        if (index.Item1 < _CurrIndex.Item1)
                        {
                            return false;
                        }
                        break;
                    };
                case Direction.Up:
                    {
                        if (index.Item2 < _CurrIndex.Item2)
                        {
                            return false;
                        }
                        break;
                    }
                case Direction.Down:
                    {
                        if (index.Item2 > _CurrIndex.Item2)
                        {
                            return false;
                        }
                        break;
                    }


            }
        }




        bool result = false;

        foreach (int tileIndex in Enum.GetValues(typeof(Direction)))
        {
            if ((Direction)tileIndex == Direction.None)
            {
                continue;
            }
            if (tile.AdjacentTiles[tileIndex] != null)
            {
                Action<UnitInfo> find = unit =>
                {
                    if (cost + Tile._NextTileCost[tileIndex] <= range && unit.TeamID == teamID)
                    {
                        if (condition() == true)
                        {

                            if (target.Find(x => x == unit) == null)
                            {
                                target.Add(unit);
                            }

                            result = true;
                        }
                    }

                };

                if (cost + Tile._NextTileCost[tileIndex] <= range && teamID == 0)
                {
                    if (tile.AdjacentTiles[tileIndex].TakedUnit == null)
                    {
                        result = true;
                        return result;
                    }
                }
                if (tile.AdjacentTiles[tileIndex].TakedUnit != null)
                {
                    find(tile.AdjacentTiles[tileIndex].TakedUnit);
                }

                if (tile.AdjacentTiles[tileIndex].GetPassingUnit().Count > 0)
                {
                    foreach (var unit in tile.AdjacentTiles[tileIndex].GetPassingUnit())
                    {
                        find(unit);
                    }
                }

            }

        }

        if (result == true)
        {
            return result;
        }

        // 비효율적이다.
        // 위쪽 반복문에서 같이 찾으면 되겠지만 가장 가까운 적을 우선 공격하게 하고 싶기에 이렇게 했다.
        for (int tileIndex = 0; tileIndex < tile.AdjacentTiles.Length; tileIndex++)
        {
            if (Tile._NextTileCost[tileIndex] <= 0)
            {
                continue;
            }
            if (tile.AdjacentTiles[tileIndex] != null)
            {
                if (cost + Tile._NextTileCost[tileIndex] <= range)
                {

                    FindTile(range, teamID, tile.AdjacentTiles[tileIndex], condition, target, lookDir, cost + Tile._NextTileCost[tileIndex]);
                }
            }

        }



        return result;
    }

    protected bool IsCanMoveAttackUnit(float range,UnitInfo unit, Func<bool> condition, bool isOrder = true)
    {

        bool result = false;
        List<UnitInfo> _EmptyTile = new List<UnitInfo>(); 

        // 추격 목표를 해당 유닛이 공격 할 수 있다면
        if(FindTile(range, 0, unit.GetCurrTile(), condition, _EmptyTile,Direction.None) == true)
        {
            // A*실행
            result = IsCanMove(unit.GetCurrTile().AStar(_CurrTile,_MoveSpeed,_TeamID));
        }


        return result;
    }

    protected bool IsCanMove(Tile nextTile)
    {
        if (nextTile == _PrevTile || nextTile == null)
        {
            return false;
        }

        _NextTile = nextTile;

        _CurrTile.GetPassingUnit().Remove(this);
        if (IsTakingTile == true)
        {
            _CurrTile.TakedUnit = null;
        }
        _NextTile.SetPassingUnit(this);



        _LookDir = GetMoveDirection(nextTile);

        return true;
    }

    protected void SortTargetList(HashSet<UnitInfo> tiles, bool isOrder, Func<UnitInfo, int> condition)
    {
        if (isOrder == true)
        {
            tiles = tiles.OrderBy(condition).ToHashSet();
        }
        else
        {
            tiles = tiles.OrderByDescending(condition).ToHashSet();
        }
    }

    protected Direction GetMoveDirection(Tile tile)
    {
        Direction result = Direction.None;
        if (_CurrTile.AdjacentTiles[(int)Direction.Up] == tile)
        {
            result = Direction.Up;
        }

        else if (_CurrTile.AdjacentTiles[(int)Direction.Down] == tile)
        {
            result = Direction.Down;
        }

        else if (_CurrTile.AdjacentTiles[(int)Direction.Right] == tile)
        {
            result = Direction.Right;
        }
        else if (_CurrTile.AdjacentTiles[(int)Direction.Left] == tile)
        {
            result = Direction.Left;
        }
        return result;
    }

    public void SetStartIndex()
    {
        _StartIndex = _CurrIndex;
    }
    
    public float GetProgressPerSpeed()
    {
        return (_TileDistance - _MoveProgress) / _MoveSpeed;
    }
    public void DoMoveComplete()
    {
        _PrevTile = _CurrTile;
        _CurrTile = _NextTile;
        _NextTile = null;
        _CurrIndex = _CurrTile.GetIndex();
        _IsCompleteMove = false;
        UnitState = State.Idle;
    }

    protected bool FindEnemy(int teamID)
    {
        //FindUnit(_ChaseRange, teamID, _CurrTile, TrueLambda, _ChaseUnitList, (Direction.None));
        FindUnit(_ChaseRange, teamID, _ChaseUnitList);
        if (_ChaseUnitList.Count > 0)
        {
            SortTargetList(_ChaseUnitList, true, tile =>
            {
                int x, y;
                x = tile.CurrIndex.Item1 - _CurrTile.GetIndex().Item1;
                y = tile.CurrIndex.Item2 - _CurrTile.GetIndex().Item2;
                return (x * x) + (y * y);
            });
            _ChaseUnit = _ChaseUnitList.First();
            _ChaseUnitList.Clear();
            //_LookDir = (Direction)dir;
            return true;
        }
        return false;
    }

    public float GetDistance(UnitInfo other)
    {
        float x, y;
        x = (float)_CurrIndex.Item1 - other._CurrIndex.Item1;
        y = (float)_CurrIndex.Item2 - other._CurrIndex.Item2;

        return (x * x) + (y * y);
    }

}