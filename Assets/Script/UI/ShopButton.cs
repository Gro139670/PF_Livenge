using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShopButton : MonoBehaviour
{
    protected void Update() 
    {
    }

    public void BattleStart()
    {
        GameManager.Instance.GetSystem<StageSystem>().IsBattle = true;
    }

    public void ShopToggle()
    {
        var shop = GameManager.Instance.GetSystem<ShopSystem>();

        shop.ShopToggle = !shop.ShopToggle;
    }

    public void BuyEXP()
    {
        var shop = GameManager.Instance.GetSystem<ShopSystem>();

        GameManager.Instance.GetSystem<PlayerSystem>().BuyEXP(shop._BuyEXPCost, shop._BuyEXPCost);
        //if (GameManager.Instance.GetSystem<PlayerSystem>().IsLevelUp == true)
        //{
        //    shop.Reroll = true;
        //}
    }

}
