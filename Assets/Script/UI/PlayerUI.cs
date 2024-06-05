using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : ActivableUI
{
    public override bool Initialize()
    {
        Mouse.Instance.UnitSelect += UnActive;
        Mouse.Instance.UnitUnSelect += Active;
        return true;
    }
}
