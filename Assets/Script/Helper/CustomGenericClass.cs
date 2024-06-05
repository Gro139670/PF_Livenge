using UnityEngine;
using UnityEngine.EventSystems;

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

public abstract class MonoSingleton<T> : MonoBehaviour, ISingleton<T> where T : MonoSingleton<T>, new()
{
    protected MonoSingleton() { }

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
                DontDestroyOnLoad(obj);
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

public abstract class ActivableUI : MonoBehaviour, IInitializeable
{
    void Awake()
    {
        Initialize();
    }
    protected void Active()
    {
        gameObject.SetActive(true);
    }

    protected void UnActive()
    {
        gameObject.SetActive(false);
    }
    public abstract bool Initialize();
}

public abstract class MouseInteractiveUI : ActivableUI, IPointerEnterHandler, IPointerExitHandler
{
    protected bool _IsMosueHover = false;

    public bool IsMosueHover
    { get; }
    
    void OnEnable()
    {
        _IsMosueHover = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _IsMosueHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _IsMosueHover = false;
    }
}


