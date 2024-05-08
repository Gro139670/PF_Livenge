using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShopButton : MyButton
{
    protected override void Update() 
    {
        base.Update();
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

        GameManager.Instance.Player.BuyEXP(1.0f, shop._BuyEXPCost);
        if (GameManager.Instance.Player.IsLevelUp == true)
        {
            shop.Reroll = true;
        }
    }

}
