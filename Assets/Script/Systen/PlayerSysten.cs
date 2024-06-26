using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSystem : MonoSystem
{
    int _HP;
    int _Mana;
    [SerializeField] int _MaxHP = 10000;
    [SerializeField] int _MaxMana = 10000;

    private float _EXP = 0;
    private float _NextEXP = 10;
    // 경험치 배율
    [SerializeField] private float _EXPScop = 1.5f;

    [SerializeField] int _Level = 0;
    [SerializeField] int _MaxLevel = 6;
    private bool _IsLevelUp = false;


    public int HP { get { return _HP; } }
    public int Mana { get { return _Mana; } }

    public int Level { get { return _Level; } }
    public int MaxLevel { get { return _MaxLevel; } }

    public int UnitTear { get { return (_MaxLevel - _Level) / 2; } }

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

    private void Awake()
    {
        Initialize();
        GameManager.Instance.RegistSystem(this);
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
            _Level++;
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
        int result = 0;
        result =_Mana + mana;
        if (result <= 0)
            return false;
        if (result > _MaxMana) _Mana = _MaxMana;

        _Mana = result;
        return true;
    }

    public override bool Initialize()
    {
        _HP = _MaxHP; _Mana = _MaxMana;
        return true;
    }
}
