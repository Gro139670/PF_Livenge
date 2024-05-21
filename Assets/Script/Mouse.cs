

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class Mouse : MonoSingleton<Mouse>
{
    private Vector3 _CurrPosition;
    private Vector3 _PrevPosition;
    private Unit _SelectedUnit;
    private Tile _SelectedTile;
    private Tile _HoveredTile;


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
            

            if(_SelectedTile == _HoveredTile || _HoveredTile == null)
            {
                _SelectedTile?.SetTakedUnit(_SelectedUnit);
            }
            else if(HoveredTile?.IsPlayerTile == true)
            {
                var unit = HoveredTile.GetTakedUnit();

                _SelectedTile?.SetTakedUnit(unit);

                HoveredTile.SetTakedUnit(_SelectedUnit);
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
