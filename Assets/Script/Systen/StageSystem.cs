using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using static UnityEngine.EventSystems.EventTrigger;


/// <summary>
/// 240321 : 가능하면 맵 에디터를 만들고 적 유닛 배치할 수 있게 하고싶다.
/// </summary>

public class StageSystem : MonoSystem
{

    [SerializeField]
    private GameObject[] _EnemiesPrefab;

    [SerializeField] private string[] _TeamIDList;

    [SerializeField] private float[] _CastleHP;
    private float _CurrRoundCastleHP;
    private int _CurrRound = 0;
    private int _MaxRound;


    private bool _IsPlayerWin = false;
    private bool _IsEnemyWin = false;

    public bool IsBattle { get; set; }
    public string[] TeamIDList { get { return _TeamIDList; } }
    

    private void Awake()
    {
        GameManager.Instance.RegistSystem(this);
        _MaxRound = _CastleHP.Length;
    }

    private void Start()
    {
        InitCurrRound();

    }
    private void FixedUpdate()
    {
        UnitManager.Instance.Logic();
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
        _IsPlayerWin = _IsEnemyWin = IsBattle = false;
    }

    public void AttackCastle(float damage)
    {
        _CurrRoundCastleHP -= damage;
    }


    private void SummonEnemy(int round)
    {
        // 일단 적을 소환한다.
        // 나중에 기회가 되면 라운드마다 적 유닛을 배치하는 툴을 만들고 거기서 정보를 받아 유닛을 배치하고 싶다.

        var enemy = _EnemiesPrefab[0];
        GameManager.Instance.GetSystem<TileSystem>().SummonUnit(0, GameManager.Instance.GetSystem<TileSystem>().Height, _EnemiesPrefab[0]);
        GameManager.Instance.GetSystem<TileSystem>().SummonUnit(2, GameManager.Instance.GetSystem<TileSystem>().Height, _EnemiesPrefab[1]);
        GameManager.Instance.GetSystem<TileSystem>().SummonUnit(4, GameManager.Instance.GetSystem<TileSystem>().Height, _EnemiesPrefab[2]);
        GameManager.Instance.GetSystem<TileSystem>().SummonUnit(6, GameManager.Instance.GetSystem<TileSystem>().Height, _EnemiesPrefab[3]);
        GameManager.Instance.GetSystem<TileSystem>().SummonUnit(8, GameManager.Instance.GetSystem<TileSystem>().Height, _EnemiesPrefab[4]);
    }


    public override bool Initialize()
    {
        return true;
    }
}
