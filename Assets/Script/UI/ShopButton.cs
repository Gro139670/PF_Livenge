using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShopButton : MonoBehaviour
{
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
    }

    public void UnSummon()
    {
        Mouse.Instance.InterectiveTile?.GetTakedUnit()?.gameObject.SetActive(false);
        Mouse.Instance.InterectiveTile = null;
    }

}
