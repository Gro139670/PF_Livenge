using Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;


/// <summary>
/// 240321 : �����ϸ� �� �����͸� ����� �� ���� ��ġ�� �� �ְ� �ϰ�ʹ�.
/// </summary>

public class StageSystem : MonoSystem
{
    public delegate void RoundEndEvent();
    private event RoundEndEvent _RoundEnd;
    public RoundEndEvent RoundEnd
    { get { return _RoundEnd; } set { _RoundEnd = value; } }





    [SerializeField]
    private GameObject[] _EnemiesPrefab;

    // �� ��ġ ���� �����Ƿ� ���⿡ �� ������ �����Ѵ�.
    // width height unitPrefab
    private List<Tuple<int, int, int>> _SummonEnemyList = null;

    [SerializeField] private string[] _TeamIDList;

    private int _OurTeamID, _EnemyTeamID;

    private int _CurrRound = 1;

    private bool _IsBattle = false;

    public bool IsBattle
    {
        get {return _IsBattle; }
        set
        {
            _IsBattle = value;
        } 
    }
    public string[] TeamIDList { get { return _TeamIDList; } }

    private bool _IsOurWin = false;
    

    private void Awake()
    {
        GameManager.Instance.RegistSystem(this);
    }

    private void Start()
    {
        InitCurrRound();
        for (int i = 0; i < _TeamIDList.Length; i++)
        {
            // team lose
            if (_TeamIDList[i] == "Our")
            {
                _OurTeamID = i;
            }

            // team win
            if (_TeamIDList[i] == "Enemy")
            {
                _EnemyTeamID = i;
            }
        }

    }
    private void FixedUpdate()
    {
        UnitManager.Instance.Logic();
        if (IsBattle == false)
        {
            _RoundEnd?.Invoke();
        }
        else
        {
            for (int i = 0; i < _TeamIDList.Length; i++)
            {
                // team lose
                if (UnitManager.Instance.GetAllTeamUnit(_OurTeamID).Count <= 0)
                {
                    IsBattle = false;
                    EnemyWin();
                    break;
                }

                // team win
                if (UnitManager.Instance.GetAllTeamUnit(_EnemyTeamID).Count <= 0)
                {
                    IsBattle = false;
                    OurWin();
                    break;
                }
            }
        }


    }


    public void InitCurrRound()
    {
        SummonEnemy(_CurrRound);
    }


    private void SummonEnemy(int round)
    {
        if (_SummonEnemyList != null)
        {
            foreach (var unit in _SummonEnemyList)
            {
                GameManager.Instance.GetSystem<TileSystem>().SummonUnit
                            (unit.Item1, unit.Item2, _EnemiesPrefab[unit.Item3], false);
            }
        }

        // �ϴ� ���� ��ȯ�Ѵ�.
        // ���߿� ��ȸ�� �Ǹ� ���帶�� �� ������ ��ġ�ϴ� ���� ����� �ű⼭ ������ �޾� ������ ��ġ�ϰ� �ʹ�.
        else
        {
            _SummonEnemyList = new();
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < GameManager.Instance.GetSystem<TileSystem>().Width; x++)
                {
                    int num = UnityEngine.Random.Range(0, 10);
                    if ((num & 1) == 0)
                    {
                        int width = x;
                        int height = GameManager.Instance.GetSystem<TileSystem>().Height - y;
                        GameManager.Instance.GetSystem<TileSystem>().SummonUnit
                            (width, height, _EnemiesPrefab[num / 2], false);

                        _SummonEnemyList.Add(new(width, height, num / 2));

                    }
                }
            }
        }
    }

    private void OurWin()
    {
        GameManager.Instance.GetSystem<TileSystem>().ClearTiles();


        var player = GameManager.Instance.GetSystem<PlayerSystem>();
        foreach (var unit in UnitManager.Instance.GetAllTeamUnit(_OurTeamID))
        {
            player.Add_HP(-unit.Status.LifeCost);
            unit.StartTile?.SetTakedUnit(unit);
            unit.gameObject.transform.localPosition = unit.Position;
            unit.LookDir = Unit.Direction.Up;

        }
        player.Add_Mana(_CurrRound * 100);

        _SummonEnemyList = null;
        _CurrRound++;

        SummonEnemy(_CurrRound);
    }

    private void EnemyWin()
    {
        GameManager.Instance.GetSystem<TileSystem>().ClearTiles();
        var player = GameManager.Instance.GetSystem<PlayerSystem>();

        player.Add_Mana(_CurrRound * 100);
        foreach (var unit in UnitManager.Instance.GetAllTeamUnit(_EnemyTeamID))
        {
            player.Add_Mana(unit.Status.ManaCost);
            player.Add_HP(-unit.Status.LifeCost);
        }


        UnitManager.Instance.ClearTeamUnits(_EnemyTeamID);
        SummonEnemy(_CurrRound);

    }



    public override bool Initialize()
    {
        return true;
    }
}
