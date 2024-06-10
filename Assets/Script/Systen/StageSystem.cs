using Enemy;
using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 240321 : 가능하면 맵 에디터를 만들고 적 유닛 배치할 수 있게 하고싶다.
/// </summary>

public class StageSystem : MonoSystem
{
    private event Action _RoundStart;
    private event Action _RoundEnd;
    public Action RoundEnd
    { get { return _RoundEnd; } set { _RoundEnd = value; } }
    public Action RoundStart
    {  get { return _RoundStart; } set {_RoundStart = value; } }







    [SerializeField]
    private GameObject[] _EnemiesPrefab;

    // 적 배치 툴이 없으므로 여기에 적 정보를 저장한다.
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
            if (value == true)
            {
                _RoundStart?.Invoke();
            }
            else
            {
                _RoundEnd?.Invoke();
            }
            _IsBattle = value;
        } 
    }
    public string[] TeamIDList { get { return _TeamIDList; } }
    

    private void Awake()
    {
        GameManager.Instance.RegistSystem(this);
        for (int i = 0; i < _TeamIDList.Length; i++)
        {
            UnitManager.Instance.AddTeam(i);
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
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
        if (IsBattle == true)
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


    // 일단 적을 랜덤으로 소환한다.
    // 나중에 기회가 되면 라운드마다 적 유닛을 배치하는 툴을 만들고 거기서 정보를 받아 유닛을 배치하고 싶다.
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


        else
        {
            _SummonEnemyList = new();
            for (int y = 0; y < GameManager.Instance.GetSystem<TileSystem>().HeightIndex/4; y++)
            {
                for (int x = 0; x < GameManager.Instance.GetSystem<TileSystem>().WidthIndex; x++)
                {
                    int num = UnityEngine.Random.Range(0, 10);
                    if ((num & 1) == 0)
                    {
                        int width = x;
                        int height = GameManager.Instance.GetSystem<TileSystem>().HeightIndex - y;
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

/*
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameManager.Instance.ChangeScene("Stage1");
        }
    }
 
 

 */
