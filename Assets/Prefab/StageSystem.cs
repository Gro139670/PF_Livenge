using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using static UnityEngine.EventSystems.EventTrigger;


/// <summary>
/// 240321 : �����ϸ� �� �����͸� ����� �� ���� ��ġ�� �� �ְ� �ϰ�ʹ�.
/// </summary>

public class StageSystem : MonoSystem
{
    [SerializeField]
    private GameObject[] _EnemiesPrefab;

    private int _TeamID = 0;
    private int _EnemyID = 0;


    public float[] _CastleHP;
    private float _CurrRoundCastleHP;
    private int _CurrRound = 0;
    private int _MaxRound;

    private bool _IsAllEnemyDead = false;
    private bool _IsCastleDestory = false;

    private bool _IsPlayerWin = false;
    private bool _IsEnemyWin = false;

    public bool IsBattle { get; set; }
    

    private void Awake()
    {
        GameManager.Instance.RegistSystem(this);
        _MaxRound = _CastleHP.Length;
    }

    private void Start()
    {
        InitCurrRound();

    }


    public void DoBattle()
    {
        //foreach (GameObject unit in _Enemies)
        //{
        //    if (unit.GetComponent<Unit>().IsDead == true)
        //    {
        //        _Enemies.Remove(unit);
        //    }
        //}

        //foreach (GameObject unit in _Teams)
        //{
        //    if (unit.GetComponent<Unit>().IsDead == true)
        //    {
        //        _Enemies.Remove(unit);
        //    }
        //}



        if (UnitManager.Instance.GetUnits(_EnemyID).Count <= 0)
        {
            _IsAllEnemyDead = true;
            _IsPlayerWin = true;
        }
        if (_CurrRoundCastleHP <= 0)
        {
            _IsCastleDestory = true;
            _IsPlayerWin = true;
        }
        if (UnitManager.Instance.GetUnits(_TeamID).Count <= 0)
        {
            _IsEnemyWin = true;
        }

        if(_IsPlayerWin == true || _IsEnemyWin == true)
        {
            IsBattle = true;
        }
    }

    public void EndBattle()
    {
        GameManager.Instance.GetSystem<TileSystem>().ClearTiles();

        Tuple<int, int> index;
        

        if(_IsPlayerWin == true)
        {
            if(_IsCastleDestory == true)
            {

            }

            //foreach (var unit in _Teams)
            //{
            //    //index = unit.GetComponent<Unit>().GetStartIndex;
            //    //TileSystem.Instance.SetUnit(index.Item1, index.Item2, unit);
            //}

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
        _IsAllEnemyDead = _IsCastleDestory = _IsPlayerWin = _IsEnemyWin = IsBattle = false;
    }

    public void AttackCastle(float damage)
    {
        _CurrRoundCastleHP -= damage;
    }


    private void SummonEnemy(int round)
    {
        // �ϴ� ���� ��ȯ�Ѵ�.
        // ���߿� ��ȸ�� �Ǹ� ���帶�� �� ������ ��ġ�ϴ� ���� ����� �ű⼭ ������ �޾� ������ ��ġ�ϰ� �ʹ�.

        var enemy = _EnemiesPrefab[0];
        var unit = enemy.GetComponent<Unit>();
        GameManager.Instance.GetSystem<TileSystem>().SummonUnit(5, GameManager.Instance.GetSystem<TileSystem>().Height, enemy);
        _EnemyID = unit.Status.TeamID;
        UnitManager.Instance.AddUnit(_EnemyID, unit);
    }

    public void SummonTeam(GameObject team)
    {
        if (team != null)
        {
            var unit = team.GetComponent<Unit>();
            _TeamID = unit.Status.TeamID;
            UnitManager.Instance.AddUnit(_TeamID, unit);
        }
    }

    public override bool Initialize()
    {
        throw new NotImplementedException();
    }
}
