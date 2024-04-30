using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitStatus
{
    #region Status

    [Header("Common")]
    [SerializeField] private string _Name;
    [SerializeField] private int _UnitTear;
    [SerializeField] private int _TeamID;

    [Header("Status")]
    [SerializeField] private float _HP = 100;
    [SerializeField] private float _Damage;
    [SerializeField] private float _Defense;
    [SerializeField] private float _AttackSpeed;
    [SerializeField] private float _MoveSpeed;
    [SerializeField] private float _Size = 2;

    [Header("Cost")]
    [SerializeField] private int _ManaCost;
    [SerializeField] private int _LifeCost;

    [Header("Range")]
    [SerializeField] private float _AttackRange = 1;
    [SerializeField] private float _ChaseRange = 1;
    [SerializeField] private float _SightRange = 1;
    #endregion

    private bool _IsDead = false;


    #region StatusFunc
    public int TeamID
    { get { return _TeamID; } }

    public float Size
    { get { return _Size; } }

    public float AttackRange
    { get { return _AttackRange; } }
    public float SightRange
    { get { return _SightRange; } }

    public float ChaseRange
    { get { return _ChaseRange; } }

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

    public void Test()
    {
        _HP -= 1f;
        Debug.Log(_HP);
    }
}
