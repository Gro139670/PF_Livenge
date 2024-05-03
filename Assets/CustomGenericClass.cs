using UnityEngine;

public abstract class Singleton<T> : ISingleton<T> where T : Singleton<T>, new()
{
    protected Singleton()
    {
    }

    private static T _Instance;

    public static T Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new T();
                _Instance.Initialize();
            }
            return _Instance;
        }
    }

    public abstract bool Initialize();
}

public abstract class MonoSingleton<T> : MonoBehaviour, ISingleton<T> where T : MonoSingleton<T>
{
    protected MonoSingleton()
    {
    }
    static public T _Instance = null;

    public static T Instance
    {
        get
        {
            if (_Instance == null)
            {
                GameObject obj;
                obj = GameObject.Find(typeof(T).Name);
                if (obj == null)
                {
                    obj = new GameObject(typeof(T).Name);
                    _Instance = obj.AddComponent<T>();
                }
                else
                {
                    _Instance = obj.GetComponent<T>();
                }

                //GameManager.Instance.AddSceneChangeEvent = () => { _Instance = null; };
            }
            return _Instance;
        }
        
    }

    public abstract bool Initialize();
}

public abstract class MonoSystem :MonoBehaviour, ISystem
{
    public abstract bool Initialize();
}


