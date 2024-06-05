
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using System;
using Unity.VisualScripting;


public class GameManager : Singleton<GameManager>
{
    #region variable

    private event Action _SceneChanged;

    private Dictionary<string, ISystem> _Systems;

    #endregion
    #region Property

    #endregion


    public void ChangeScene(string sceneName)
    {
        if (_SceneChanged != null)
        {
            _SceneChanged();
            _SceneChanged = null;
        }
        _Systems.Clear();
        
        SceneManager.LoadScene(sceneName);
        UnitManager.Instance.Initialize();
    }

    public override bool Initialize()
    {
        _Systems = new();
        return true;
    }


    public bool RegistSystem<T>(T system) where T :MonoBehaviour, ISystem
    {
        if(system == null)
        {
            throw new System.Exception("AddSystem : system is null");
        }
        if (_Systems == null)
        {
            _Systems = new();
        }
        system.Initialize();
        _SceneChanged += () =>
        {
            GameObject.Destroy(system.gameObject);
        };
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
    public void GetSystem<T>(out T result) where T : class, ISystem
    {
        if (_Systems[typeof(T).Name] == null)
        {
            throw new System.Exception("GetSystem : system is null");
        }

        result = (T)_Systems[typeof(T).Name];
    }




}

