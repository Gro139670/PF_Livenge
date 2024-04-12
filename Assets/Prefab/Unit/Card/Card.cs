using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : Button
{
    #region variable
    public Sprite[] _CardImage;
    public Sprite[] _CardExplain;




    // ÃÑ À¯´Ö ¼ö
    private static int _MaxUnitNum = 0;
    private int _UnitNum = 0;
    private int _Cost = 0;

    #endregion
    #region property
    public static void SetUnitNum (int num)
    {
        _MaxUnitNum = num;
    }
    public int UnitNum
    {
        get { return _UnitNum; }
    }
    public int Cost
    { get { return _Cost; } }

    #endregion



   
    protected override void Update()
    {
        base.Update();
        if (Mouse.SelectedCard != null)
        {
            _IsMouseDown = false;
            return;
        }
        if ( _IsMouseDown == true )
        {
            Mouse.SelectedCard = this;
        }
    }

    public void SetUnitIndex()
    {
        _UnitNum = Random.Range(0,_MaxUnitNum);
        _Cost = _UnitNum * 2;
        _Renderer.sprite = _CardImage[_UnitNum];
    }
    
}
