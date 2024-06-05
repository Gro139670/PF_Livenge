using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBackGound : ActivableUI
{
    private Vector3 _DefaultShopPosition;
    private Vector3 _InterectiveShopPosition;
    void Start()
    {
        _DefaultShopPosition = _InterectiveShopPosition = transform.localPosition;
        _InterectiveShopPosition.y += 480;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(GameManager.Instance.GetSystem<ShopSystem>().ShopInterective == true)
        {
            gameObject.transform.localPosition = _InterectiveShopPosition;
        }
        else
        {
            gameObject.transform.localPosition = _DefaultShopPosition;
        }
        
    }

    public override bool Initialize()
    {
        Mouse.Instance.UnitSelect += UnActive;
        Mouse.Instance.UnitUnSelect += Active;
        return true;
    }
}
