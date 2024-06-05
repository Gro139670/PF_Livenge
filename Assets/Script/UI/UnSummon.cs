using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnSummonUI : MouseInteractiveUI
{
    public override bool Initialize()
    {
        Mouse.Instance.UnitSelect += Active;
        Mouse.Instance.UnitUnSelect += UnActive;
        return true;
    }
    private void Start()
    {
        gameObject.SetActive(false);
    }
    private void Update()
    {
        if (_IsMosueHover == true)
        {
            if(Input.GetMouseButtonUp(0))
            {
                Mouse.Instance.SelectedUnit?.gameObject.SetActive(false);
                Mouse.Instance.InterectiveTile = null;
            }
        }
    }
    public void UnSummon()
    {
        Mouse.Instance.InterectiveTile?.GetTakedUnit()?.gameObject.SetActive(false);
        Mouse.Instance.InterectiveTile = null;
    }
}
