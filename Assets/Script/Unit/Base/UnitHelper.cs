using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitStatus : IInitializeable
{
    #region Status

    [Header("Common")]
    [SerializeField] private string _Name;
    [SerializeField] private int _UnitTear = 0;
    [SerializeField] private int _TeamID;

    [Header("Status")]
    [SerializeField] private float _HP = 100;
    private float _MaxHP = 0;
    [SerializeField] private float _Damage = 1;
    [SerializeField] private float _Defense = 1;
    [SerializeField] private float _Size = 2;

    [Header("Speed")]
    [SerializeField] private float _AttackSpeed = 1;
    [SerializeField] private float _MoveSpeed = 1;
    [SerializeField] private float _SearchSpeed = 1;
    private float _SpeedDebuff = 0;

    [Header("Cost")]
    [SerializeField] private int _ManaCost = 1;
    [SerializeField] private int _LifeCost = 1;

    [Header("Range")]
    [SerializeField] private float _AttackRange = 1;
    [SerializeField] private float _ChaseRange = 1;
    [SerializeField] private float _SightRange = 1;



    #endregion

    private bool _IsDead = false;


    #region Property
    public float Damage
    {
        get {return 0; return _Damage; }
    }

    public int TeamID
    { get { return _TeamID; } set { _TeamID = value; } }

    public float Size
    { get { return _Size; } }

    public float AttackRange
    { get { return _AttackRange; } }
    public float SightRange
    { get { return _SightRange; } }

    public float ChaseRange
    { get { return _ChaseRange; } }





    public int ManaCost
    {  get { return _ManaCost; } }
    public int LifeCost
    { get { return _LifeCost; } }



    public float AttackSpeed
    { get { return _AttackSpeed * SpeedDebuff; } }

    public float MoveSpeed
    { get { return 200/(_MoveSpeed * SpeedDebuff * 10); } }

    public float SearchSpeed
    {
        get
        {
            if (_SearchSpeed == 0) _SearchSpeed = 1;
            return 1;
            return 100 / (_SearchSpeed * SpeedDebuff);
        }
        set { _SearchSpeed = value; }
    }

    public float SpeedDebuff
    {
        get { return _SpeedDebuff; }
        set { _SpeedDebuff = 1 - (value / 100f); }
    }

    public bool IsDead
    {
        get
        {
            if (_HP <= 0)
            {
                _IsDead = true;
            }
            else
            {
                _IsDead = false;
            }
            return _IsDead;
        }
    }

    #endregion

    public float Damaged(float value)
    {
        float AllDamage = 0;
        AllDamage = value / (1 + _Defense);
        _HP -= AllDamage;
        return AllDamage;
    }

    public void AddHP(float hp)
    {
        _HP += hp;

        if (_HP > _MaxHP) _HP = _MaxHP;
    }

    public bool Initialize()
    {
        _MaxHP = _HP;
        return true;
    }
}

[RequireComponent(typeof(Unit))]
public class UnitHelper : MonoBehaviour
{
    protected Unit _OwnerInfo;

    protected void Awake()
    {
        _OwnerInfo = GetComponent<Unit>();
    }
}

public abstract class TeamHelper : UnitHelper, ITeamSetting
{
    public abstract void SetEnemyID();
    public abstract void SetTeamID();

    protected void Start()
    {
        SetTeamID();
        SetEnemyID();
    }

    protected int GetTeamID(string name)
    {
        int result = 0;

        var idList = GameManager.Instance.GetSystem<StageSystem>().TeamIDList;
        for (int i = 0; i < idList.Length; i++)
        {
            if (idList[i] == name)
            {
                result = i;
                break;
            }
        }

        return result;
    }
}
