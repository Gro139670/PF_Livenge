using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    #region variable


    [SerializeField] Sprite[] _CardImage;
    [SerializeField] Sprite[] _CardExplain;
    Image _Image;



    // ÃÑ À¯´Ö ¼ö
    private static int _MaxUnitNum = 0;
    private int _UnitNum = 0;
    private int _Cost = 0;

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

    public void SetUnitIndex()
    {
        _UnitNum = Random.Range(0,_MaxUnitNum);
        _Cost = _UnitNum * 2;
        _Image.sprite = _CardImage[_UnitNum];
    }

    public void BuyUnit()
    {
        if (GameManager.Instance.Player.Add_Mana(-_Cost))
        {
            if(GameManager.Instance.GetSystem<ShopSystem>().BuyUnit(0) == false)
            {
                // ±¸¸Å ºÒ°¡ ui Ãâ·Â
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
    
}
