using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSystem : MonoSystem
{
    #region variable

    [SerializeField] GameObject _Buttons;
    [SerializeField] GameObject[] _TeamUnits;

    public int _BuyEXPCost = 0;
    [SerializeField] int _BuyRerollCost = 0;

    [SerializeField]
    private bool _ShopToggle = false;
    #endregion
    #region property
    public int BuyRerollCost
    {
        get { return _BuyRerollCost; }
    }

    public bool ShopInterective
    {  
        get { return _ShopToggle; } 
        set {  _ShopToggle = value; } 
    }

    public int UnitLegth
    {
        get { return _TeamUnits.Length; }
    }
    #endregion
    private void Awake()
    {
        GameManager.Instance.RegistSystem(this);
    }

    private void Start()
    {
        return;
    }

    private void FixedUpdate()
    {
        _Buttons.SetActive(!GameManager.Instance.GetSystem<StageSystem>().IsBattle);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.Instance.GetSystem<StageSystem>().IsBattle = false;
        }
    }


    public bool BuyUnit(int unitNum)    
    {
        var unit = _TeamUnits[unitNum];
        if (GameManager.Instance.GetSystem<PlayerSystem>().Add_Mana(-unit.GetComponent<Unit>().Status.ManaCost) == true)
        {
            var interectiveTile = Mouse.Instance.InterectiveTile;
            if (interectiveTile != null)
            {
                if (GameManager.Instance.GetSystem<TileSystem>().SummonUnit(interectiveTile.Index.Item1, interectiveTile.Index.Item2, unit, true) == true)
                {
                    return true;
                }
            }
            else
            {
                for (int height = 0; height < GameManager.Instance.GetSystem<TileSystem>().HeightIndex; height++)
                {
                    for (int width = 0; width <= GameManager.Instance.GetSystem<TileSystem>().WidthIndex; width++)
                    {
                        if (GameManager.Instance.GetSystem<TileSystem>().SummonUnit(width, height, unit, true) == true)
                        {
                            return true;
                        }
                    }
                }
            }
            GameManager.Instance.GetSystem<PlayerSystem>().Add_Mana(unit.GetComponent<Unit>().Status.ManaCost);
            // 타일이 가득 찼다.
            // 오류 메세지 출력
            return false;
        }
        // 플레이어 마나가 부족하다.
        return false;
    }

    public override bool Initialize()
    {
        return true;
    }
}
