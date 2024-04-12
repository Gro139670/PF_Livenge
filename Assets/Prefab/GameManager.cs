
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{
    #region variable
    public delegate void EventHandler();

    private EventHandler _SceneChanged;
    public EventHandler SceneChanged { set { _SceneChanged += value; } }


    private BattleManager _BattleManager = null;
    private TileManager _TileManager = null;
    private StageManager _StageManager = null;
    private Player _Player = null;

    #endregion
    #region Property

    public BattleManager BattleManager
    {
        get { return _BattleManager; }
        set { _BattleManager = value; }
    }

    public TileManager TileManager
    {
        get { return _TileManager; }
        set { _TileManager = value; }
    }

    public StageManager StageManager
    {
        get { return _StageManager; }
        set { _StageManager = value; }
    }

    public Player Player
    {
        get { return _Player; }
        set { _Player = value; }
    }

    #endregion

    public void ChangeScene(string sceneName)
    {
        if(_SceneChanged != null)
        {
            _SceneChanged();
            _SceneChanged = null;
        }

        SceneManager.LoadScene(sceneName);
        Units.Initialize();
    }

}

