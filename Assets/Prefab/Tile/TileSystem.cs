using System;
using UnityEngine;

public class TileSystem : MonoSystem
{
    public GameObject _Tile;
    public Color[] _TileColor;
    
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
        Tile.SetTileBaseColor(_TileColor);
        _Height = _TileHeightNum;
        _Width = _TileWidthNum;
        Tile.SetMaxTileSize(_Width, _Height);
        #region TileGenerator

        _TileContainer = new GameObject[_Height, _Width];
        int tileNum = 0;

        for (int tile_HeightIndex = 0; tile_HeightIndex < _Height; tile_HeightIndex++)
        {
            int startNum = tileNum;
            for (int tile_WidthIndex = 0; tile_WidthIndex < _Width; tile_WidthIndex++)
            {


                GameObject tile = Instantiate(_Tile, this.transform);
                Tile tileComponent = tile.GetComponent<Tile>();
                tileComponent.TileNum = tileNum;
                tileNum++;
                tileNum %= _TileColor.Length;




                tileComponent.SetIndex(tile_WidthIndex, tile_HeightIndex);

                tile.transform.position = new Vector3(tile.transform.position.x + (_Tile.transform.localScale.x * tile_WidthIndex * transform.localScale.x),
                    tile.transform.position.y + (_Tile.transform.localScale.y * tile_HeightIndex * transform.localScale.y));

                _TileContainer[tile_HeightIndex, tile_WidthIndex] = tile;
            }

            // 타일 색을 지정한다.
            if (tileNum == startNum)
            {
                tileNum++;
                tileNum %= _TileColor.Length;
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
        if (_TileContainer[height, width].GetComponent<Tile>().TakedUnit == null)
        {
            _TileContainer[height, width].GetComponent<Tile>().TakedUnit = unit.GetComponent<Unit>();
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
        throw new NotImplementedException();
    }
}
