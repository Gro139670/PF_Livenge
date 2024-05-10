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

    public bool ShopToggle
    {  
        get { return _ShopToggle; } 
        set {  _ShopToggle = value; } 
    }

    public bool Reroll
    { get; set; }

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


            for (int height = 4; height >= 0; height--)
            {
                for (int width = 0; width <= GameManager.Instance.GetSystem<TileSystem>().Width; width++)
                {
                    if (GameManager.Instance.GetSystem<TileSystem>().SummonUnit(width, height, unit) == true)
                    {
                        return true;
                    }
                }
            }
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
