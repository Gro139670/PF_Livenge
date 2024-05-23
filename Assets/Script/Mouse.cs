

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class Mouse : MonoSingleton<Mouse>
{
    private Unit _SelectedUnit;
    private Tile _InterectiveTile;
    private Tile _SelectedTile;
    private Tile _HoveredTile;

    public Tile InterectiveTile
    { get { return _InterectiveTile; } set { _InterectiveTile = value; } }
    public Tile HoveredTile
    { get { return _HoveredTile; } set { _HoveredTile = value; } }

    public Card SelectedCard
    { get; set; }


    public Vector2 MousePoint { get; set; }

    public override bool Initialize()
    {
        return true;
    }
    private void FixedUpdate()
    {
        MousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void Update()
    {
        if(_SelectedUnit != null)
        {
            _SelectedUnit.gameObject.transform.position = MousePoint;
        }
        
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(_HoveredTile?.IsPlayerTile == true)
            {
                _SelectedTile = HoveredTile;
                _SelectedUnit = _SelectedTile.GetTakedUnit();
                

            }
            
        }

        if (Input.GetMouseButtonUp(0))
        {
           

            if (_SelectedTile == _HoveredTile)
            {
                if (_SelectedTile != null)
                {
                    if (_InterectiveTile == _SelectedTile)
                    {
                        _InterectiveTile = null;
                    }
                    else
                    {
                        _InterectiveTile = _SelectedTile;
                    }
                }

                if (_HoveredTile == null)
                {
                    _SelectedTile?.SetTakedUnit(_SelectedUnit);

                }
            }
            else if(HoveredTile?.IsPlayerTile == true)
            {
                var unit = HoveredTile.GetTakedUnit();

                _SelectedTile?.SetTakedUnit(unit);

                HoveredTile.SetTakedUnit(_SelectedUnit);

                _InterectiveTile = null;
            }
            


            if (_SelectedTile?.GetTakedUnit() != null)
            {
                _SelectedTile.GetTakedUnit().transform.localPosition = _SelectedTile.GetTakedUnit().Position;
            }

            if (HoveredTile?.GetTakedUnit() != null)
            {
                HoveredTile.GetTakedUnit().transform.localPosition = HoveredTile.GetTakedUnit().Position;
            }

            _SelectedTile = null;
            _SelectedUnit = null;
        }


        {
            HoveredTile = null;
        }
    }
}
