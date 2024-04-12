using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    int _HP;
    int _Mana;
    public int _MaxHP = 100;
    public int _MaxMana = 100;

    private float _EXP = 0;
    private float _NextEXP = 10;
    // 경험치 배율
    public float _EXPScop = 1;

    public int _Level = 0;
    public int _MaxLevel = 6;
    private bool _IsLevelUp = false;

    private void Awake()
    {
        GameManager.GetInstance().Player = this;
        Init();
    }

    public int Level { get { return _Level; } }
    public int MaxLevel { get { return _MaxLevel; } }

    public bool IsLevelUp
    { 
        get
        {
            bool result = _IsLevelUp;
            if (_IsLevelUp == true)
                _IsLevelUp = false;

            return result; 
        } 
    }

    public void RoundClear(float exp)
    {
        AddEXP(exp);
    }

    public void BuyEXP(float exp, int useMana)
    {
        if(AddEXP(exp) == true)
        {
            Add_Mana(-useMana);
        }
    }

    private bool AddEXP(float exp)
    {
        if (_Level >= _MaxLevel)
            return false;

        _EXP += exp;
        if(_EXP >= _NextEXP)
        {
            _EXP -= _NextEXP;
            _NextEXP *= _EXPScop;
            _IsLevelUp = true;
        }
        return true;
    }

    public bool Add_HP(int hp)
    {
        _HP += hp;
        if (_HP <= 0)
            return false;

        if (_HP > _MaxHP) _HP = _MaxHP;
        return true;
    }

    public bool Add_Mana(int mana)
    {
        _Mana += mana;
        if (_Mana <= 0)
            return false;
        if (_Mana > _MaxMana) _Mana = _MaxMana;
        return true;
    }

    public void Init()
    {
        _HP = _MaxHP; _Mana = _MaxMana;
    }


}
