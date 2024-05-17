using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] GameObject _Card;
    GameObject[] _Cards;



    private void Awake()
    {
        _Cards = new GameObject[10];
        for(int i = 0; i < _Cards.Length;i++)
        {
            _Cards[i] = Instantiate(_Card,transform);
            _Cards[i].SetActive(false);
        }
    }

    private void Start()
    {
        Reroll();
        var shop = GameManager.Instance.GetSystem<ShopSystem>();
        GameManager.Instance.GetSystem<PlayerSystem>().Add_Mana(shop.BuyRerollCost);
    }

    public void Reroll()
    {
        var shop = GameManager.Instance.GetSystem<ShopSystem>();


        if (GameManager.Instance.GetSystem<PlayerSystem>().Add_Mana(-shop.BuyRerollCost) == true)
        {
            int _HandCard;
            Card.SetUnitNum(shop.UnitLegth - GameManager.Instance.GetSystem<PlayerSystem>().UnitTear);

            _HandCard = 4 + GameManager.Instance.GetSystem<PlayerSystem>().Level;

            for (int hand = 0; hand < _HandCard; hand++)
            {
                _Cards[hand].SetActive(true);
                _Cards[hand].GetComponent<Card>().SetUnitIndex();
            }
        }
    }
}
