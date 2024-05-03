using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : Button
{

    private static Color[] _TileBaseColor;
    public static Tuple<int, int> _MaxTileSize;
    private Tile[] _AdjacentTiles = new Tile[9];

    [SerializeField] private Unit _TakedUnit = null;
    [SerializeField] private Unit _TakingExpectedUnit = null;
    private List<Unit> _PassingUnit = new List<Unit>();
    private SpriteRenderer _SpriteRenderer = null;
    public static GameObject[] _FarTile;

    private Tuple<int,int> _Index;

    // ���� Ÿ�Ϸκ��� �� Ÿ�ϱ����� �Ÿ��� ��Ÿ����.
    public static float[] _NextTileCost = { 1, 1.414f, 1.414f, -1,1, 1, 1, 1.414f, 1.414f };
    private bool _IsShowRange = false;
    private bool _IsShowRangeTogether = false;
    private bool _IsPrevShowState = false;



    private int _TileNum = 0;

    #region Property

    public Unit TakingExpectedUnit
    {
        get { return _TakingExpectedUnit; }
    }

    public Unit TakedUnit
    {
        get { return _TakedUnit; }
        set
        {
            if(value == null)
            {
                _TakedUnit = null;
                return;
            }

            Unit info = value.GetComponent<Unit>();
            if (info != null)
            {
                _TakedUnit = value;
                info.SetCurrTile(this);
                //info.CurrIndex = _Index;
            }
        }
    }
    public List<Unit> GetPassingUnit()
    {
        return _PassingUnit;

    }
    public void SetPassingUnit(Unit unit)
    {
        _PassingUnit.Add(unit);
    }

    public void RemovePassingUnit(Unit unit)
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
                Unit take = null;
                foreach (var unit in _PassingUnit)
                {
                    //speed = unit.GetComponent<Unit>().GetProgressPerSpeed();
                    //if (min > speed)
                    //{
                    //    min = speed;
                    //    take = unit;
                    //}

                }
                _TakingExpectedUnit = take;
            }
            if (_TakingExpectedUnit != null)
            {
                //if (_TakingExpectedUnit.GetComponent<Unit>().IsCompleteMove == true)
                //{
                //    _TakingExpectedUnit.GetComponent<Unit>().DoMoveComplete();
                //    _TakedUnit = _TakingExpectedUnit;
                //    _TakingExpectedUnit = null;
                //}
            }
        }
        else
        {
            if (_PassingUnit.Count > 0)
            {
             
                foreach (var unit in _PassingUnit)
                {
                    //if(unit.GetComponent<Unit>().IsCompleteMove)
                    //{
                    //    unit.GetComponent<Unit>().DoMoveComplete();
                    //}
                }
            }

            _TakedUnit.SetCurrTile(this);
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
                Unit info = _TakedUnit.GetComponent<Unit>();
                //ShowRange(0, _IsShowRange, info.AttackRange);
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
                // ���� Ÿ���� �������� ��
                if(tuple.Item1.AdjacentTiles[index] == target)
                {
                    // ���� Ÿ���� ��ȯ
                    result = tuple.Item1;
                    return;
                }

                float weight = 0;
                // ������ �������
                if (tuple.Item1.AdjacentTiles[index].TakedUnit != null)
                {
                    Unit info = tuple.Item1.AdjacentTiles[index].TakedUnit.GetComponent<Unit>();

                    // ���� �Ұ� ������ ��
                    //if (info.TeamID != selfTeamID)
                    //{
                    //    return;
                    //}
                    //else
                    //{
                    //    weight = tuple.Item2 + (speed / info.Size);
                    //}

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
            enqueue((int)Unit.Direction.Up, currTile);
            enqueue((int)Unit.Direction.Down, currTile);
            enqueue((int)Unit.Direction.Left, currTile);
            enqueue((int)Unit.Direction.Right, currTile);

            queue.OrderBy(x => x.Item1);



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

    public bool SummonUnit(GameObject gameObject)
    {
        if(_TakedUnit == null)
        {
            Instantiate(gameObject, transform);
            _TakedUnit = gameObject.GetComponent<Unit>();
            _TakedUnit.SetCurrTile(this);
            UnitManager.Instance.AddUnit(_TakedUnit.Status.TeamID, _TakedUnit);
            return true;
        }

        return false;
    }
}
