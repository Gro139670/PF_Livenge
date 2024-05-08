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
        if(Reroll()== true)
        {
            var shop = GameManager.Instance.GetSystem<ShopSystem>();
            GameManager.Instance.Player.Add_Mana(shop.BuyRerollCost);
        }
    }

    public bool Reroll()
    {
        bool result = false;
        var shop = GameManager.Instance.GetSystem<ShopSystem>();


        if (GameManager.Instance.Player.Add_Mana(-shop.BuyRerollCost) == true)
        {
            int _HandCard;
            Card.SetUnitNum(shop.UnitLegth - GameManager.Instance.Player.UnitTear);

            _HandCard = 4 + GameManager.Instance.Player.Level;

            for (int hand = 0; hand < _HandCard; hand++)
            {
                _Cards[hand].SetActive(true);
                _Cards[hand].GetComponent<Card>().SetUnitIndex();
            }
            result = true;
        }
        return result;
    }
}
