using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : Button
{

    private static Color[] _TileBaseColor;
    public static Tuple<int, int> _MaxTileSize;
    private Tile[] _AdjacentTiles = new Tile[9];

    [SerializeField] private UnitInfo _TakedUnit = null;
    [SerializeField] private UnitInfo _TakingExpectedUnit = null;
    private List<UnitInfo> _PassingUnit = new List<UnitInfo>();
    private SpriteRenderer _SpriteRenderer = null;
    public static GameObject[] _FarTile;

    private Tuple<int,int> _Index;

    // 현재 타일로부터 각 타일까지의 거리를 나타낸다.
    public static float[] _NextTileCost = { 1, 1.414f, 1.414f, -1,1, 1, 1, 1.414f, 1.414f };
    private bool _IsShowRange = false;
    private bool _IsShowRangeTogether = false;
    private bool _IsPrevShowState = false;



    private int _TileNum = 0;

    #region Property

    public UnitInfo TakingExpectedUnit
    {
        get { return _TakingExpectedUnit; }
    }

    public UnitInfo TakedUnit
    {
        get { return _TakedUnit; }
        set
        {
            if(value == null)
            {
                _TakedUnit = null;
                return;
            }

            UnitInfo info = value.GetComponent<UnitInfo>();
            if (info != null)
            {
                _TakedUnit = value;
                info.SetCurrTile(this);
                info.CurrIndex = _Index;
            }
        }
    }
    public List<UnitInfo> GetPassingUnit()
    {
        return _PassingUnit;

    }
    public void SetPassingUnit(UnitInfo unit)
    {
        _PassingUnit.Add(unit);
    }

    public void RemovePassingUnit(UnitInfo unit)
    {
        _PassingUnit.Remove(unit);
    }


    public Tile[] AdjacentTiles { get { return _AdjacentTiles; } }

    public bool IsShowRangeTogether { set { _IsShowRangeTogether = value; } }
    public int TileNum
    {
        set { _TileNum = value; }
        get { return _TileNum; }
    }

    public void SetIndex(int width, int height)
    {
        _Index = new Tuple<int, int>(width, height);
    }

    public Tuple<int, int> GetIndex()
    {
        return _Index;
    }

    #endregion


    protected override void Awake()
    {
        _SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    protected override void Update()
    {

        if(_TakedUnit == null)
        {
            if(_PassingUnit.Count > 0)
            {
                float speed;
                float min = float.MaxValue;
                UnitInfo take = null;
                foreach (var unit in _PassingUnit)
                {
                    speed = unit.GetComponent<UnitInfo>().GetProgressPerSpeed();
                    if (min > speed)
                    {
                        min = speed;
                        take = unit;
                    }

                }
                _TakingExpectedUnit = take;
            }
            if (_TakingExpectedUnit != null)
            {
                if (_TakingExpectedUnit.GetComponent<UnitInfo>().IsCompleteMove == true)
                {
                    _TakingExpectedUnit.GetComponent<UnitInfo>().DoMoveComplete();
                    _TakedUnit = _TakingExpectedUnit;
                    _TakingExpectedUnit = null;
                }
            }
        }
        else
        {
            if (_PassingUnit.Count > 0)
            {
             
                foreach (var unit in _PassingUnit)
                {
                    if(unit.GetComponent<UnitInfo>().IsCompleteMove)
                    {
                        unit.GetComponent<UnitInfo>().DoMoveComplete();
                    }
                }
            }
        }
        base.Update();

    }
    private void LateUpdate()
    {
        if(_TakedUnit != null)
        {
         
            if (IsMouseHover == true)
            {
                _IsShowRange = true;
            }
            else
            {
                _IsShowRange = false;
            }


            if (_IsPrevShowState != _IsShowRange)
            {
                UnitInfo info = _TakedUnit.GetComponent<UnitInfo>();
                ShowRange(0, _IsShowRange, info.AttackRange);
                _IsPrevShowState = _IsShowRange;
            }
            
        }

        if (_IsShowRangeTogether == true)
        {
            _SpriteRenderer.color = new Color(1, 0, 0.5f);
        }
        else
        {
            if(_TileBaseColor[_TileNum] != null)
            {
                _SpriteRenderer.color = _TileBaseColor[_TileNum];
            }
        }


    }

    public void SetAdjacentTile(Tile tile, int index)
    {
        _AdjacentTiles[index] = tile;
    }

    public void ShowRange(float cost, bool isShow, float distance)
    {
        for (int tileIndex = 0; tileIndex < _AdjacentTiles.Length; tileIndex++)
        {

            if (cost + 1 <= distance && _AdjacentTiles[tileIndex] != null)
            {
                _AdjacentTiles[tileIndex].IsShowRangeTogether = isShow;
                _AdjacentTiles[tileIndex].ShowRange(cost + 1, isShow, distance);
            }
        }
    }

    public Tile AStar(Tile target, float speed, int selfTeamID)
    {

        Tile result = null;
        Queue<Tuple<Tile, float>> queue = new Queue<Tuple<Tile, float>>();

        queue.Enqueue(new Tuple<Tile,float>(this,0));

        Action<int, Tuple<Tile, float>> enqueue = (index,tuple) =>
        {
            if (tuple.Item1.AdjacentTiles[index] != null && result == null)
            {
                // 다음 타일이 목적지일 때
                if(tuple.Item1.AdjacentTiles[index] == target)
                {
                    // 현재 타일을 반환
                    result = tuple.Item1;
                    return;
                }

                float weight = 0;
                // 유닛이 있을경우
                if (tuple.Item1.AdjacentTiles[index].TakedUnit != null)
                {
                    UnitInfo info = tuple.Item1.AdjacentTiles[index].TakedUnit.GetComponent<UnitInfo>();

                    // 통행 불가 유닛일 때
                    if (info.TeamID != selfTeamID)
                    {
                        return;
                    }
                    else
                    {
                        weight = tuple.Item2 + (speed / info.Size);
                    }

                }
                else
                {
                    weight = tuple.Item2 + speed;
                }
                queue.Enqueue(new Tuple<Tile, float>(tuple.Item1.AdjacentTiles[index],weight));
            }
               
        };

        Tuple<Tile, float> currTile;

        while (queue.Count > 0)
        {
            if(result != null)
            {
                return result;
            }
            currTile = queue.Dequeue();
            enqueue((int)UnitInfo.Direction.Up, currTile);
            enqueue((int)UnitInfo.Direction.Down, currTile);
            enqueue((int)UnitInfo.Direction.Left, currTile);
            enqueue((int)UnitInfo.Direction.Right, currTile);

            queue.OrderByDescending(x => x.Item1);



        }

        return result;
    }



    public static void SetTileBaseColor(Color[] colors)
    {
        _TileBaseColor = colors;
    }
    public static void SetMaxTileSize(int width, int height)
    {
        _MaxTileSize = new Tuple<int, int>(width, height);
    }
    public void ClearTile()
    {
        _TakedUnit = null;
        _PassingUnit.Clear();
    }
}
