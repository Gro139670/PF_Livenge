using System;
using UnityEngine;

public class TileSystem : MonoSystem
{
    [SerializeField] private GameObject _Tile;
    [SerializeField] private Color[] _TileColor;
    [SerializeField] private GameObject _TileGeneratePoint;
    
    private Tile[] _TileContainer;

    [SerializeField] private int _Width = 0;
    [SerializeField] private int _Height = 0;

    [SerializeField] private int _UnitNum = 0;

    public int Width
    { get { return _Width; } }
    public int WidthIndex
    {
        get { return _Width - 1; }
    }
    public int Height
    { get { return _Height; } }
    public int HeightIndex
    { 
        get { return _Height - 1; } 
    }


    public int GetTileNum(int widthIndex, int heightIndex)
    {
        return (heightIndex * _Width) + widthIndex;
    }

    private void Update()
    {
        _UnitNum = 0;
        foreach (var unit in UnitManager.Instance.GetAllUnit()) {
            _UnitNum += unit.Value.Count;
                }
    }
    private void Awake()
    {
        GameManager.Instance.RegistSystem(this);
        #region TileGenerator
        Vector3 tilePoint = new();
        if(_TileGeneratePoint != null)
        {
            tilePoint = _TileGeneratePoint.transform.position;
        }
        _TileContainer = new Tile[_Height * _Width];
        int tileNum = 0;
        GameObject tile;
        for (int HeightIndex = 0; HeightIndex < _Height; HeightIndex++)
        {
            int startNum = tileNum;
            for (int WidthIndex = 0; WidthIndex < _Width; WidthIndex++)
            {


                tile = Instantiate(_Tile, transform);
                Tile tileComponent = tile.GetComponent<Tile>();
                tileNum++;
                tileNum %= _TileColor.Length;




                tileComponent.Index = new(WidthIndex, HeightIndex);

                tile.transform.localPosition = new Vector3(tile.transform.localPosition.x + (tile.transform.localScale.x * WidthIndex) + tilePoint.x,
                    tile.transform.localPosition.y + (_Tile.transform.localScale.y * HeightIndex) + tilePoint.y);

                _TileContainer[GetTileNum(WidthIndex,HeightIndex)] = tileComponent;

                // 타일 색을 지정한다.
                tileComponent.BaseColor = _TileColor[(WidthIndex + (HeightIndex & 1)) % _TileColor.Length];

                if(HeightIndex < 5)
                {
                    tileComponent.IsPlayerTile = true;
                }
            }

            
        }




        Action<int, int, int, Tile> SetTile = (widthIndex, heightIndex, line, tile) =>
        {
            Tile tileComponent;
            // left
            if (widthIndex - 1 >= 0)
            {
                tileComponent = _TileContainer[GetTileNum(widthIndex, heightIndex) - 1];
                tile.SetAdjacentTile(tileComponent, (1 + (line * 3)));
            }

            // mid
            tileComponent = _TileContainer[GetTileNum(widthIndex, heightIndex)];
            tile.SetAdjacentTile(tileComponent, (0 + (line * 3)));

            // right
            if (widthIndex + 1 < _Width)
            {
                tileComponent = _TileContainer[GetTileNum(widthIndex, heightIndex) + 1] ;
                tile.SetAdjacentTile(tileComponent, (2 + (line * 3)));
            }
        };


        for (int HeightIndex = 0; HeightIndex < _Height; HeightIndex++)
        {
            for (int WidthIndex = 0; WidthIndex < _Width; WidthIndex++)
            {
                // top
                if (HeightIndex - 1 >= 0)
                {
                    SetTile(WidthIndex, HeightIndex - 1, 0, _TileContainer[GetTileNum(WidthIndex, HeightIndex)]);
                }

                // mid
                SetTile(WidthIndex, HeightIndex, 1, _TileContainer[GetTileNum(WidthIndex, HeightIndex)]);

                // bottom
                if (HeightIndex + 1 < _Height)
                {
                    SetTile(WidthIndex, HeightIndex + 1, 2, _TileContainer[GetTileNum(WidthIndex, HeightIndex)]);
                }


            }
        }
        
        #endregion
    }



    public GameObject SummonUnit(int widthIndex, int heightIndex, GameObject unit, bool isBuy)
    {
        // 예외처리
        if( 0 > widthIndex || widthIndex > WidthIndex || 0 > heightIndex ||heightIndex > HeightIndex )
        {
            return null;
        }
        if(isBuy == true )
        {
            if(_TileContainer[GetTileNum(widthIndex, heightIndex)].IsPlayerTile == true)
            {
                return _TileContainer[GetTileNum(widthIndex, heightIndex)].SummonUnit(unit);
            }
        }
        return _TileContainer[GetTileNum(widthIndex, heightIndex)].SummonUnit(unit);
    }

    public bool SetUnit(int widthIndex, int heightIndex, GameObject unit)
    {
        // 예외처리
        if (0 > widthIndex || widthIndex > WidthIndex || 0 > heightIndex || heightIndex > HeightIndex || unit == null)
        {
            // 오류 메세지 출력
            return false;
        }

        bool result = false;
        if (_TileContainer[GetTileNum(widthIndex, heightIndex)].GetTakedUnit() == null)
        {
            _TileContainer[GetTileNum(widthIndex, heightIndex)].SetTakedUnit(unit.GetComponent<Unit>());
            result = true;
        }



        return result;
    }

    public void ClearTiles()
    {
        foreach (var tile in _TileContainer)
        {
            tile.ClearTile();
        }
    }

    public override bool Initialize()
    {
        return true;
    }
}
