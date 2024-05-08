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

}
