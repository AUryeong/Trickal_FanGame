using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : SerializedMonoBehaviour where T : SerializedMonoBehaviour
{
    private static T instance;
    protected virtual bool IsDontDestroying => false;
    public static T Instance
    {
        get
        {
            if (instance != null) return instance;

            instance = FindObjectOfType(typeof(T)) as T;
            if (instance != null) return instance;

            var temp = new GameObject(typeof(T).Name);
            instance = temp.AddComponent<T>();
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
            instance = this as T;
        if (Instance.Equals(this))
        {
            OnCreated();
            if (IsDontDestroying)
            {
                DontDestroyOnLoad(gameObject);
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
        }
        else
        {
            Destroy(gameObject);
            (Instance as Singleton<T>).OnReset();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        OnReset();
    }
    
    protected virtual void OnReset()
    {
    }

    protected virtual void OnCreated()
    {
    }
}