using UnityEngine;

public class Singleton<T> where T : Singleton<T>, new()
{
    protected Singleton()
    {
    }

    private static T _Instance;

    public static T GetInstance()
    {
        if (_Instance == null)
        {
            _Instance = new T();
        }
        return _Instance;
    }
}

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected MonoSingleton()
    {
    }
    static public T _Instance = null;

    public static T GetInstance()
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

            GameManager.GetInstance().SceneChanged = () => { _Instance = null; };
        }
        return _Instance;
    }
}


