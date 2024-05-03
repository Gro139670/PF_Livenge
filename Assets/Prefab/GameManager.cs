
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{
    #region variable
    //public delegate void EventHandler();

    //private EventHandler _SceneChanged;
    //public EventHandler AddSceneChangeEvent { set { _SceneChanged += value; } }

    private Dictionary<string, ISystem> _Systems;

    #endregion
    #region Property

    public Player Player { get; set; }

    #endregion


    public void ChangeScene(string sceneName)
    {
        //if (_SceneChanged != null)
        //{
        //    _SceneChanged();
        //    _SceneChanged = null;
        //}

        _Systems.Clear();

        SceneManager.LoadScene(sceneName);
        UnitManager.Instance.Initialize();
    }

    public override bool Initialize()
    {
        return true;
    }


    public bool RegistSystem<T>(T system) where T :class, ISystem
    {
        if(system == null)
        {
            throw new System.Exception("AddSystem : system is null");
        }
        if (_Systems == null)
        {
            _Systems = new();
        }
        _Systems.Add(typeof(T).Name, system);

        return true;
    }

    public T GetSystem<T>() where T :class, ISystem
    {
        if(_Systems[typeof(T).Name] == null)
        {
            throw new System.Exception("GetSystem : system is null");
        }

        return (T)_Systems[typeof(T).Name];
    }




}

