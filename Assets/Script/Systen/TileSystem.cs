using System;
using UnityEngine;

public class TileSystem : MonoSystem
{
    [SerializeField] private GameObject _Tile;
    [SerializeField] private Color[] _TileColor;
    [SerializeField] private GameObject _TileGeneratePoint;
    
    private GameObject[,] _TileContainer;

    public int _TileWidthNum = 0;
    public int _TileHeightNum = 0;

    private int _Width = 0;
    private int _Height = 0;

    public int Width
    {
        get { return _Width - 1; }
    }
    public int Height
    { 
        get { return _Height - 1; } 
    }

    private void Awake()
    {
        GameManager.Instance.RegistSystem(this);
        _Height = _TileHeightNum;
        _Width = _TileWidthNum;
        #region TileGenerator
        Vector3 tilePoint = new();
        if(_TileGeneratePoint != null)
        {
            tilePoint = _TileGeneratePoint.transform.position;
        }
        _TileContainer = new GameObject[_Height, _Width];
        int tileNum = 0;
        GameObject tile;
        for (int tile_HeightIndex = 0; tile_HeightIndex < _Height; tile_HeightIndex++)
        {
            int startNum = tileNum;
            for (int tile_WidthIndex = 0; tile_WidthIndex < _Width; tile_WidthIndex++)
            {


                tile = Instantiate(_Tile, transform);
                Tile tileComponent = tile.GetComponent<Tile>();
                tileNum++;
                tileNum %= _TileColor.Length;




                tileComponent.Index = new(tile_WidthIndex, tile_HeightIndex);

                tile.transform.localPosition = new Vector3(tile.transform.localPosition.x + (tile.transform.localScale.x * tile_WidthIndex) + tilePoint.x,
                    tile.transform.localPosition.y + (_Tile.transform.localScale.y * tile_HeightIndex) + tilePoint.y);

                _TileContainer[tile_HeightIndex, tile_WidthIndex] = tile;

                // 타일 색을 지정한다.
                tileComponent.BaseColor = _TileColor[(tile_WidthIndex + (tile_HeightIndex & 1)) % _TileColor.Length];
            }

            
        }




        Action<int, int, int, GameObject> SetTile = (width, height, line, tile) =>
        {
            Tile tileComponent;
            // left
            if (width - 1 >= 0)
            {
                tileComponent = _TileContainer[height, width - 1].GetComponent<Tile>();
                tile.GetComponent<Tile>().SetAdjacentTile(tileComponent, (1 + (line * 3)));
            }

            // mid
            tileComponent = _TileContainer[height, width].GetComponent<Tile>();
            tile.GetComponent<Tile>().SetAdjacentTile(tileComponent, (0 + (line * 3)));

            // right
            if (width + 1 < _Width)
            {
                tileComponent = _TileContainer[height, width + 1].GetComponent<Tile>();
                tile.GetComponent<Tile>().SetAdjacentTile(tileComponent, (2 + (line * 3)));
            }
        };


        for (int tile_HeightIndex = 0; tile_HeightIndex < _Height; tile_HeightIndex++)
        {
            for (int tile_WidthIndex = 0; tile_WidthIndex < _Width; tile_WidthIndex++)
            {
                // top
                if (tile_HeightIndex - 1 >= 0)
                {
                    SetTile(tile_WidthIndex, tile_HeightIndex - 1, 0, _TileContainer[tile_HeightIndex, tile_WidthIndex]);
                }

                // mid
                SetTile(tile_WidthIndex, tile_HeightIndex, 1, _TileContainer[tile_HeightIndex, tile_WidthIndex]);

                // bottom
                if (tile_HeightIndex + 1 < _Height)
                {
                    SetTile(tile_WidthIndex, tile_HeightIndex + 1, 2, _TileContainer[tile_HeightIndex, tile_WidthIndex]);
                }


            }
        }
        Tile._FarTile = new GameObject[4];
        Tile._FarTile[0] = _TileContainer[1, 1];
        Tile._FarTile[1] = _TileContainer[1, 1];
        Tile._FarTile[2]= _TileContainer[1, 1];
        Tile._FarTile[3] = _TileContainer[1, 1];
        
        #endregion
    }



    public bool SummonUnit(int width, int height, GameObject unit)
    {
        // 예외처리
        if( 0 > width || width > Width || 0 > height ||height > Height )
        {
            return false;
        }
        return _TileContainer[height, width].GetComponent<Tile>().SummonUnit(unit);
    }

    public bool SetUnit(int width, int height, GameObject unit)
    {
        // 예외처리
        if (0 > width || width > Width || 0 > height || height > Height || unit == null)
        {
            // 오류 메세지 출력
            return false;
        }

        bool result = false;
        if (_TileContainer[height, width].GetComponent<Tile>().GetTakedUnit() == null)
        {
            _TileContainer[height, width].GetComponent<Tile>().SetTakedUnit(unit.GetComponent<Unit>());
            result = true;
        }



        return result;
    }

    public void ClearTiles()
    {
        foreach (var tile in _TileContainer)
        {
            tile.GetComponent<Tile>().ClearTile();
        }
    }

    public override bool Initialize()
    {
        return true;
    }
}
