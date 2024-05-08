using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBackGound : MonoBehaviour
{
    private Vector3 _DefaultShopPosition;
    private Vector3 _InterectiveShopPosition;
    private void Awake()
    {
        _DefaultShopPosition = _InterectiveShopPosition = transform.localPosition;
        _InterectiveShopPosition.y = -250;
    }
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(GameManager.Instance.GetSystem<ShopSystem>().ShopToggle)
        {
            gameObject.transform.localPosition = _InterectiveShopPosition;
        }
        else
        {
            gameObject.transform.localPosition = _DefaultShopPosition;
        }
        
    }
}
