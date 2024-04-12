using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    #region variable
    public int debugint;

    public GameObject[] _Cards;
    public GameObject _BackGround;
    public GameObject _LevelUPButton;
    public GameObject _RerollButton;
    public GameObject _UnSommonButton;
    public GameObject _RoundStartButton;

    public GameObject _Shop;
    public GameObject _ShopUI;
    private Vector3 _DefaultShopPosition;
    private Vector3 _InterectiveShopPosition;


    public GameObject[] _Units;

    public int _BuyEXPCost = 0;
    public int _BuyRerollCost = 0;

    private int _HandCard = 4;

    [SerializeField]
    private bool _ShopToggle = false;
    #endregion
    #region property

    #endregion

    private void Start()
    {
        Reroll();
        _DefaultShopPosition = _InterectiveShopPosition = _Shop.transform.localPosition;
        _InterectiveShopPosition.y += 7.5f;
    }

    private void Update()
    {
        // debug
        if(Input.GetKeyDown(KeyCode.R))
        {
            BattleManager.GetInstance().IsBattleStart = false;
            _ShopUI.SetActive(true);
        }
            

        if (BattleManager.GetInstance().IsBattleStart == true)
        {
            return;
        }
        if(_RoundStartButton.GetComponent<ShopButton>().IsMouseDown == true)
        {
            BattleManager.GetInstance().IsBattleStart = true;
            _Shop.transform.localPosition = _DefaultShopPosition;
            _ShopUI.SetActive(false);
            return;
        }


        if (_BackGround.GetComponent<ShopButton>().IsMouseDown == true)
        {
            _ShopToggle = !_ShopToggle;
        }

        // 플레이어가 상점과 상호작용하고 있다.
        if (_ShopToggle == true)
        {
            _Shop.transform.localPosition = _InterectiveShopPosition;

            ButtonInterective();
            Mouse.SelectedCard = null;
        }

        // 플레이어가 카드와 상호작용 하고 있다.
        else
        {
            _Shop.transform.localPosition = _DefaultShopPosition;

            CardInterecteve();
        }
    }

    private void CardInterecteve()
    {
        // 현재 선택된 카드가 있다.
        if (Mouse.SelectedCard != null)
        {
            if (Mouse.IsMouseMove == true)
            {
                // 카드를 드래그 하고 있다.


                // 무엇과도 상호작용이 없다면
                if (Input.GetMouseButtonUp(0))
                {
                    // 현재 선택된 카드를 해제한다.
                    Mouse.SelectedCard = null;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                // 카드를 사용했다.

                if(GameManager.GetInstance().Player.Add_Mana(-Mouse.SelectedCard.Cost) && Mouse.SelectedCard != null)
                {
                    BuyUnit();

                    Mouse.SelectedCard.gameObject.SetActive(false);
                    Mouse.SelectedCard = null;
                }
                else
                {
                    // 마나가 부족해 소환할 수 없었다.
                }
                
            }

        }
    }

    private void ButtonInterective()
    {
        

        if(_ShopToggle == true)
        {
            if (_LevelUPButton.GetComponent<ShopButton>().IsMouseDown == true)
            {

                    GameManager.GetInstance().Player.BuyEXP(1.0f, _BuyEXPCost);
                    if(GameManager.GetInstance().Player.IsLevelUp == true)
                    {
                        Reroll();
                    }
                    return;
                
            }
            if (_RerollButton.GetComponent<ShopButton>().IsMouseDown == true)
            {
               
                    if (GameManager.GetInstance().Player.Add_Mana(-_BuyRerollCost) == true)
                    {
                        Reroll();
                        return;
                    }
                
            }
            if (_UnSommonButton.GetComponent<ShopButton>().IsMouseDown == true)
            {
              
                    _ShopToggle = true;
                    // 유닛을 삭제해야한다.

                    return;
                
            }
        }
    }

    private void Reroll()
    {
        Card.SetUnitNum(_Units.Length - ((GameManager.GetInstance().Player.MaxLevel - GameManager.GetInstance().Player.Level) / 2));

        _HandCard = 4 + GameManager.GetInstance().Player.Level;

        for (int hand = 0; hand < _HandCard; hand++)
        {
            _Cards[hand].SetActive(true);
            _Cards[hand].GetComponent<Card>().SetUnitIndex();
        }
    }

    private bool BuyUnit()
    {
        GameObject summoner = null;
        for (int height = 4; height >= 0; height--)
        {
            for (int width = 0; width <= TileManager.GetInstance().Width; width++)
            {
                summoner = TileManager.GetInstance().SummonUnit(width, height, _Units[debugint]);

                if (summoner != null)
                {
                    StageManager.GetInstance().SummonTeam(summoner);
                    return true;
                }

            }
        }
        // 타일이 가득 찼다.
        // 오류 메세지 출력
        return false;
    }
}
