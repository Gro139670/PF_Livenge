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

        // �÷��̾ ������ ��ȣ�ۿ��ϰ� �ִ�.
        if (_ShopToggle == true)
        {
            _Shop.transform.localPosition = _InterectiveShopPosition;

            ButtonInterective();
            Mouse.SelectedCard = null;
        }

        // �÷��̾ ī��� ��ȣ�ۿ� �ϰ� �ִ�.
        else
        {
            _Shop.transform.localPosition = _DefaultShopPosition;

            CardInterecteve();
        }
    }

    private void CardInterecteve()
    {
        // ���� ���õ� ī�尡 �ִ�.
        if (Mouse.SelectedCard != null)
        {
            if (Mouse.IsMouseMove == true)
            {
                // ī�带 �巡�� �ϰ� �ִ�.


                // �������� ��ȣ�ۿ��� ���ٸ�
                if (Input.GetMouseButtonUp(0))
                {
                    // ���� ���õ� ī�带 �����Ѵ�.
                    Mouse.SelectedCard = null;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                // ī�带 ����ߴ�.

                if(GameManager.GetInstance().Player.Add_Mana(-Mouse.SelectedCard.Cost) && Mouse.SelectedCard != null)
                {
                    BuyUnit();

                    Mouse.SelectedCard.gameObject.SetActive(false);
                    Mouse.SelectedCard = null;
                }
                else
                {
                    // ������ ������ ��ȯ�� �� ������.
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
                    // ������ �����ؾ��Ѵ�.

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
        // Ÿ���� ���� á��.
        // ���� �޼��� ���
        return false;
    }
}
