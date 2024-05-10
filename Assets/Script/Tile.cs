using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuadTree<T>
{
    public T _Data;
    public QuadTree<T>[] _Children = new QuadTree<T>[4];
    public QuadTree<T> _Parent;
}

public class Tile : MyButton
{
    private Tile[] _AdjacentTiles = new Tile[9];

    [SerializeField] private Unit _TakedUnit = null;


    private SpriteRenderer _SpriteRenderer = null;
    public static GameObject[] _FarTile;

    [SerializeField]private float _Weight = 1.0f;

    // 현재 타일로부터 각 타일까지의 거리를 나타낸다.
    private static float[] _NextTileCost = { 1, 1.414f, 1.414f,1, 1, 1, 1.414f, 1.414f };

    private static int[,] _TilePriority = { { 0, 1 }, { -1, 1 }, { 1, 1 }, { -1, 0 }, { 1, 0 }, { 0, -1 }, { -1, -1 }, { 1, -1 } };

    private bool _IsShowRange = false;
    private bool _IsShowRangeTogether = false;
    private bool _IsPrevShowState = false;

    #region Property

    public float Weight
    {
        get { return _Weight; }
    }

    public Color BaseColor
    {
        get; set;
    }

    public Unit GetTakedUnit()
    {
        return _TakedUnit;
    }

    public void SetTakedUnit(Unit value)
    {
        if (value == null)
        {
            _TakedUnit = null;
            return;
        }

        Unit unit = value.GetComponent<Unit>();
        if (unit != null)
        {
            unit.CurrTile.SetTakedUnit(null);
            _TakedUnit = value;
            unit.CurrTile = this;
        }
    }


    public Tile[] AdjacentTiles { get { return _AdjacentTiles; } }

    public bool IsShowRangeTogether { set { _IsShowRangeTogether = value; } }

    public Tuple<int,int> Index
    {
        get;set;
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
           
        }
        else
        {
   

            _TakedUnit.CurrTile = this;
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
            if(BaseColor != null)
            {
                _SpriteRenderer.color = BaseColor;
            }
        }
    }

    public void SetAdjacentTile(Tile tile, int index)
    {
        _AdjacentTiles[index] = tile;
    }

    //public void ShowRange(float cost, bool isShow, float distance)
    //{
    //    for (int tileIndex = 0; tileIndex < _AdjacentTiles.Length; tileIndex++)
    //    {

    //        if (cost + 1 <= distance && _AdjacentTiles[tileIndex] != null)
    //        {
    //            _AdjacentTiles[tileIndex].IsShowRangeTogether = isShow;
    //            _AdjacentTiles[tileIndex].ShowRange(cost + 1, isShow, distance);
    //        }
    //    }
    //}

    public float GetDistance(Tile other)
    {
        float x, y;
        x = (float)Index.Item1 - other.Index.Item1;
        y = (float)Index.Item2 - other.Index.Item2;

        return (x * x) + (y * y);
    }

    private class AstarData
    {
        public AstarData(Tile currTile, Tile target)
        {
            Tile = currTile;
            H = Tile.GetDistance(target);
        }

        AstarData Parent = null;
        public Tile Tile;
        public bool IsVisit = false;
        public float F = 0;
        float G = 0;
        float H = 0;



        public void SetParent(AstarData parent, float weight)
        {
            if (Parent == null)
                Parent = parent;
            else
            {
                Parent = (Parent.G < parent.G) ? Parent : parent;
            }

            SetG(weight);
            SetF();
        }

        public AstarData GetParent()
        {
            return Parent;
        }

        void SetF()
        {
            F = G + H;
        }
        void SetG(float weight)
        {
            if (Parent == null)
            {
                G = 0;
            }
            else
            {
                G = Parent.G + weight;
            }
        }
    }

    public Stack<Unit.Direction> AStar(Tile target)
    {
        Stack<Unit.Direction> path = null;
        AstarData goal = null;
        bool find = false;

        Queue<AstarData> queue = new Queue<AstarData>();
        queue.Enqueue(new (this,target));

        HashSet<Tile> visitTile = new HashSet<Tile>();
        visitTile.Add(this);

        Action<int,AstarData> enqueue = (dir,data) =>
        {
            if(visitTile.Contains(data.Tile.AdjacentTiles[dir]))
            {
                return;
            }

            if (data.Tile.AdjacentTiles[dir] != null && find == false)
            {
                // 다음 타일이 목적지일 때
                if(data.Tile.AdjacentTiles[dir] == target)
                {
                    find = true;
                    goal = data;
                    return;
                }

                if (data.Tile.AdjacentTiles[dir]._TakedUnit == null)
                {
                    AstarData child = new AstarData(data.Tile.AdjacentTiles[dir],target);
                    child.SetParent(data, data.Tile.Weight);
                    visitTile.Add(child.Tile);
                    queue.Enqueue(child);
                }
               
            }
               
        };

        

        while (queue.Count > 0)
        {
            if(find == true)
            {
                break;
            }
            var currTile = queue.Dequeue();
            enqueue((int)Unit.Direction.Up, currTile);
            enqueue((int)Unit.Direction.Down, currTile);
            enqueue((int)Unit.Direction.Left, currTile);
            enqueue((int)Unit.Direction.Right, currTile);

            queue.OrderBy(x => x.F);
        }

        if (find == true)
        {
            path = new();
            // set path
            while (goal.GetParent() != null)
            {
                for (int index = 0; index < goal.GetParent().Tile.AdjacentTiles.Length; index++)
                {
                    if (goal.GetParent().Tile.AdjacentTiles[index] == goal.Tile)
                    {
                        path.Push((Unit.Direction)index);
                        goal = goal.GetParent();
                        break;
                    }
                }
                
            }

        }

        return path;
    }
    public void ClearTile()
    {
        _TakedUnit = null;
    }

    public bool SummonUnit(GameObject gameObject)
    {
        if(_TakedUnit == null)
        {
            var unit = Instantiate(gameObject, transform);
            _TakedUnit = unit.GetComponent<Unit>();
            _TakedUnit.CurrTile = this;
            UnitManager.Instance.AddUnit(_TakedUnit.Status.TeamID, _TakedUnit);
            return true;
        }

        return false;
    }
}
