using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] GameObject _Card;
    GameObject[] _Cards;
    int _HandCard = 0;
    float _HandTime = 0;
    private bool _IsReroll = false;
    private bool _IsFinishSetCard = false;

    private Vector3 _DefaultPosition;
    private Vector3 _InterectivePosition;

    private void Awake()
    {
        _DefaultPosition = _InterectivePosition = transform.localPosition;
        _DefaultPosition.y -= 500;

        _Cards = new GameObject[10];
        for(int i = 0; i < _Cards.Length;i++)
        {
            _Cards[i] = Instantiate(_Card,transform);
            _Cards[i].SetActive(false);
            _Cards[i].GetComponent<Card>().AddUseCardEvent = () => { _HandCard--; _HandTime = 0; };
        }
    }

    private void Start()
    {
        Reroll();
        var shop = GameManager.Instance.GetSystem<ShopSystem>();
        GameManager.Instance.GetSystem<PlayerSystem>().Add_Mana(shop.BuyRerollCost);
        _HandTime = 0;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.GetSystem<ShopSystem>().ShopToggle == true)
        {
            gameObject.transform.localPosition = _InterectivePosition;
        }
        else
        {
            gameObject.transform.localPosition = _DefaultPosition;
        }

        ShuffleHand();
        SetCards();
       



    }

    public void Reroll()
    {
        if (_IsReroll == false)
        {
            var shop = GameManager.Instance.GetSystem<ShopSystem>();


            if (GameManager.Instance.GetSystem<PlayerSystem>().Add_Mana(-shop.BuyRerollCost) == true)
            {
                _IsReroll = true;
                _HandTime = 0;
            }
        }
    }

    void ShuffleHand()
    {
        if(GameManager.Instance.GetSystem<PlayerSystem>().IsLevelUp == true)
        {
            _IsReroll = true;
            _HandTime = 0;
        }

        if (_IsReroll == false)
            return;



        bool isFinish = false;
        isFinish = TimeManager.Instance.SetTime(ref _HandTime, 1);

        if (isFinish == false)
        {
            for (int i = 0; i < _Cards.Length; i++)
            {
                if (_Cards[i].activeSelf == true)
                {
                    Vector3 pos;
                    pos = Vector3.Lerp(_Cards[i].transform.localPosition, Vector3.zero, _HandTime);
                    _Cards[i].GetComponent<Card>().SetPosition(pos);
                }
            }
        }
        else
        {
            var shop = GameManager.Instance.GetSystem<ShopSystem>();
            Card.SetUnitNum(shop.UnitLegth - GameManager.Instance.GetSystem<PlayerSystem>().UnitTear);

            _HandCard = 4 + GameManager.Instance.GetSystem<PlayerSystem>().Level;

            for (int hand = 0; hand < _HandCard; hand++)
            {
                _Cards[hand].SetActive(true);
                _Cards[hand].GetComponent<Card>().SetUnitIndex();
            }
            _IsReroll = false;
            _HandTime = 0;
        }
        

    }

    void SetCards()
    {
        if(_IsReroll == true)
        {
            return;
        }
        _IsFinishSetCard = TimeManager.Instance.SetTime(ref _HandTime, 1);
        int card = _HandCard;
        for (int i = 0; i < _Cards.Length; i++)
        {
            if (_Cards[i].activeSelf == true)
            {
                Vector3 pos;
                pos = Vector3.Lerp(_Cards[i].transform.localPosition, new Vector3((-1000 / _HandCard) * card, 0), _HandTime);
                _Cards[i].GetComponent<Card>().SetPosition(pos);
                if (_IsFinishSetCard == true)
                {

                }
                card--;
            }
        }
    }
}
