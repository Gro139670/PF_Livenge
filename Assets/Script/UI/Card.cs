using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;
public class Card : MouseInteractiveUI
{
    #region variable
    private event Action UseCardEvent;
    public Action AddUseCardEvent
    {
        set { UseCardEvent += value; }
    }

    [SerializeField] Sprite[] _CardImage;
    [SerializeField] Sprite[] _CardExplain;
    Image _Image;


    private Vector3 _DefaultPosition;
    private Vector3 _InterectivePosition;

    // ÃÑ À¯´Ö ¼ö
    private static int _MaxUnitNum = 0;
    private int _UnitNum = 0;

    private bool _IsCanBuy = false;
    private bool _IsSetPosition = false;

    #endregion


    #region property
    public static void SetUnitNum (int num)
    {
        _MaxUnitNum = num;
    }
    #endregion
    private void Awake()
    {
        _Image = GetComponent<Image>();
       
    }

    void Update()
    {
        if(_IsMosueHover == false)
        {
            if(Input.GetMouseButtonDown(0))
            {
                _IsCanBuy = false;
                _IsSetPosition = false;
            }
        }

        if(Input.GetKeyDown(KeyCode.B))
        {
            _UnitNum = Random.Range(0, _MaxUnitNum);
            GameManager.Instance.GetSystem<ShopSystem>().BuyUnit(_UnitNum);
        }
    }

    void LateUpdate()
    {
        if(_IsCanBuy == true)
        {
            transform.localPosition = _InterectivePosition;
        }
        else
        {
            transform.localPosition = _DefaultPosition;
        }
    }

    public void SetUnitIndex()
    {
        _UnitNum = Random.Range(0,_MaxUnitNum);
        _Image.sprite = _CardImage[_UnitNum];
    }

    public void BuyUnit()   
    {

        if (GameManager.Instance.GetSystem<ShopSystem>().BuyUnit(_UnitNum) == false)
        {
            // ±¸¸Å ºÒ°¡ ui Ãâ·Â
        }
        else
        {
            Mouse.Instance.InterectiveTile = null;
            _IsCanBuy = false;
            gameObject.SetActive(false);
            _IsSetPosition = false;
            UseCardEvent?.Invoke();
            transform.localPosition = Vector3.zero;
        }
    }

    public void SetPosition(Vector3 pos)
    {
        _DefaultPosition = _InterectivePosition = pos;
        _InterectivePosition.y += 100;
        if (_IsSetPosition == false)
        {
            
            _IsSetPosition = true;
        }
    }

    public override bool Initialize()
    {
        return true;
    }
}
