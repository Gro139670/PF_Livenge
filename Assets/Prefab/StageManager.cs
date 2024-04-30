using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 240321 : 가능하면 맵 에디터를 만들고 적 유닛 배치할 수 있게 하고싶다.
/// </summary>

public class StageManager : MonoSingleton<StageManager>
{
    [SerializeField]
    private GameObject[] _EnemiesPrefab;
    private List<GameObject> _Enemies = new List<GameObject>();
    private List<GameObject> _Teams = new List<GameObject>();

    public float[] _CastleHP;
    private float _CurrRoundCastleHP;
    private int _CurrRound = 0;
    private int _MaxRound;

    private bool _IsAllEnemyDead = false;
    private bool _IsCastleDestory = false;

    private bool _IsPlayerWin = false;
    private bool _IsEnemyWin = false;
    private bool _IsBattleEnd = false;

    public bool IsBattleEnd
    {  get { return _IsBattleEnd; } }
    

    private void Awake()
    {
        _MaxRound = _CastleHP.Length;
    }

    private void Start()
    {
        InitCurrRound();

    }

    public void StartBattle()
    {
        //foreach (var unit in _Enemies)
        //{
        //    unit.GetComponent<UnitInfo>().SetStartIndex();
        //}
        //foreach (var unit in _Teams)
        //{
        //    unit.GetComponent<UnitInfo>().SetStartIndex();
        //}
    }

    public void DoBattle()
    {
        //foreach (GameObject unit in _Enemies)
        //{
        //    if (unit.GetComponent<UnitInfo>().IsDead == true)
        //    {
        //        _Enemies.Remove(unit);
        //    }
        //}

        //foreach (GameObject unit in _Teams)
        //{
        //    if (unit.GetComponent<UnitInfo>().IsDead == true)
        //    {
        //        _Enemies.Remove(unit);
        //    }
        //}



        if (_Enemies.Count <= 0)
        {
            _IsAllEnemyDead = true;
            _IsPlayerWin = true;
        }
        if (_CurrRoundCastleHP <= 0)
        {
            _IsCastleDestory = true;
            _IsPlayerWin = true;
        }
        if (_Teams.Count <= 0)
        {
            _IsEnemyWin = true;
        }

        if(_IsPlayerWin == true || _IsEnemyWin == true)
        {
            _IsBattleEnd = true;
        }
    }

    public void EndBattle()
    {
        TileManager.GetInstance().ClearTiles();

        Tuple<int, int> index;
        

        if(_IsPlayerWin == true)
        {
            if(_IsCastleDestory == true)
            {

            }

            foreach (var unit in _Teams)
            {
                //index = unit.GetComponent<UnitInfo>().GetStartIndex;
                //TileManager.GetInstance().SetUnit(index.Item1, index.Item2, unit);
            }

        }
        else if (_IsEnemyWin == true)
        {

        }

    }


    public void InitCurrRound()
    {
       
        SummonEnemy(_CurrRound);
        InitFlag();
    }
    public void InitNextRound()
    {
        _CurrRound++;
        InitCurrRound();
        _CurrRoundCastleHP = _CastleHP[_CurrRound];
    }

    private void InitFlag()
    {
        _IsAllEnemyDead = _IsCastleDestory = _IsPlayerWin = _IsEnemyWin = _IsBattleEnd = false;
    }

    public void AttackCastle(float damage)
    {
        _CurrRoundCastleHP -= damage;
    }


    private void SummonEnemy(int round)
    {
        // 일단 적을 소환한다.
        // 나중에 기회가 되면 라운드마다 적 유닛을 배치하는 툴을 만들고 거기서 정보를 받아 유닛을 배치하고 싶다.
        GameObject Enemy = TileManager.GetInstance().SummonUnit(5, TileManager.GetInstance().Height, _EnemiesPrefab[0]);
        if (Enemy != null)
        {
            _Enemies.Add(Enemy);
        }
    }

    public void SummonTeam(GameObject unit)
    {
        if (unit != null)
        {
            _Teams.Add(unit);
        }
    }
}
