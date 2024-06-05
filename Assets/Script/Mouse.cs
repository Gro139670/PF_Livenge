using System;
using UnityEngine;

public class Mouse : MonoSingleton<Mouse>
{
    private event Action _UnitSelect;
    private event Action _UnitUnSelect;
    public Action UnitSelect
    { 
        get { return _UnitSelect; }
        set { _UnitSelect = value; }
    }
    public Action UnitUnSelect
    {
        get { return _UnitUnSelect; } 
        set { _UnitUnSelect = value; }
    }

    private void Mouse__UnitUnSelect()
    {
        throw new NotImplementedException();
    }

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

    public Unit SelectedUnit
    { get { return _SelectedUnit; } }


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
        if (GameManager.Instance.GetSystem<ShopSystem>().ShopInterective == true)
        {
            _InterectiveTile = null;
            _UnitUnSelect?.Invoke();
            return;
        }

        if (_InterectiveTile?.GetTakedUnit() != null)
        {
            _UnitSelect?.Invoke();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (_HoveredTile?.IsPlayerTile == true)
            {
                _SelectedTile = HoveredTile;
                _SelectedUnit = _SelectedTile.GetTakedUnit();
                if(_SelectedUnit != null)
                {
                    _UnitSelect?.Invoke();
                }    

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
            

            // À¯´ÖÀÇ ÀÚ¸®¸¦ ¹Ù²Û´Ù.
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
            if(_InterectiveTile?.GetTakedUnit() == null || _InterectiveTile == null)
            {
                _UnitUnSelect?.Invoke();
            }

        }
        {
            HoveredTile = null;
        }

    }
}
