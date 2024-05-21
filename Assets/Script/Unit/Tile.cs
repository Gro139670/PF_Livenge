using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class Tile : MouseHover
{
    private Tile[] _AdjacentTiles = new Tile[9];

    private static readonly Color _HoverColor = Color.cyan;

    [SerializeField] private Unit _TakedUnit = null;
    private SpriteRenderer _SpriteRenderer = null;

    [SerializeField]private readonly float _Weight = 1.0f;
    
    private bool _IsShowRangeTogether = false;



    #region Property
    public Color BaseColor { get; set; }
    public Tile[] AdjacentTiles { get { return _AdjacentTiles; } }
    public Tuple<int,int> Index { get; set; }


    public float Weight {  get { return _Weight; } }
    public bool IsShowRangeTogether { get; set; }
    public bool IsPlayerTile {  get; set; }



    #endregion


    protected override void Awake()
    {
        base.Awake();
        _SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    protected override void Update()
    {
        base.Update();
        if (BaseColor != null)
        {
            _SpriteRenderer.color = BaseColor;
        }
        if (GameManager.Instance.GetSystem<StageSystem>().IsBattle == false)
        {
            if (_IsMouseHover == true)
            {
                Mouse.Instance.HoveredTile = this;
                if(IsPlayerTile == true)
                {
                    _SpriteRenderer.color = _HoverColor;
                }
                else
                {
                    _SpriteRenderer.color = Color.red;
                }
            }

            if (_IsShowRangeTogether == true)
            {
                _SpriteRenderer.color = new Color(1, 0, 0.5f);
            }
        }

    }
    private void LateUpdate()
    {


        //if (_TakedUnit != null)
        //{

        //    if (_IsMouseHover == true)
        //    {
        //        if(Input.GetMouseButtonDown(0))
        //        {
        //            Mouse.Instance.SelectedTile = this;
        //            Mouse.Instance.SelectedUnit = _TakedUnit;

        //        }
        //        _IsShowRange = true;
        //    }
        //    else
        //    {
        //        _IsShowRange = false;
        //    }


        //    if (_IsPrevShowState != _IsShowRange)
        //    {
        //        Unit info = _TakedUnit.GetComponent<Unit>();
        //        ShowRange(0, _IsShowRange, info.Status.AttackRange);
        //        _IsPrevShowState = _IsShowRange;
        //    }

        //}
       
        
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
            //unit.CurrTile.SetTakedUnit(null);
            _TakedUnit = value;
            _TakedUnit.gameObject.transform.SetParent(transform);
            unit.CurrTile = this;
        }
    }

    public void SetUnit(Unit unit)
    {
        unit.CurrTile?.SetTakedUnit(null);
        unit.CurrTile = this;
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

    public float GetDistance(Tile other)
    {
        if (other == null)
            return float.MaxValue;
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

    public GameObject SummonUnit(GameObject gameObject)
    {
        if (gameObject == null)
        {
            throw new System.Exception("GameObject is Null");
        }
        if(_TakedUnit == null)
        {
            var unit = Instantiate(gameObject, transform);
            _TakedUnit = unit.GetComponent<Unit>();
            _TakedUnit.CurrTile = this;
            UnitManager.Instance.AddUnit(_TakedUnit.Status.TeamID, _TakedUnit);
            return unit;
        }

        return null;
    }

    public void SummonProjectile(GameObject gameObject, Unit.Direction direction)
    {

        var info = gameObject.GetComponent<Unit>();
        gameObject.GetComponent<Unit>().CurrTile = this;

        info.LookDir = direction;

        gameObject.transform.parent = this.transform;
        gameObject.SetActive(true);
    }
}
