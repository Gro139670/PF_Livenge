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
    #endregion
    #region debug
    public int _DebugUnitNum = 4;
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
        _Image.sprite = _CardImage[_UnitNum];
    }

    public void BuyUnit()   
    {
        // debug
        _UnitNum = _DebugUnitNum;

        if (GameManager.Instance.GetSystem<ShopSystem>().BuyUnit(_UnitNum) == false)
        {
            // ±¸¸Å ºÒ°¡ ui Ãâ·Â
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    
}
