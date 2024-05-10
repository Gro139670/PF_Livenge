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
        // �ϴ� ���� ��ȯ�Ѵ�.
        // ���߿� ��ȸ�� �Ǹ� ���帶�� �� ������ ��ġ�ϴ� ���� ����� �ű⼭ ������ �޾� ������ ��ġ�ϰ� �ʹ�.

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
