using UnityEngine;
using UnityEditor;

public abstract class BaseSingleton<T> : BaseMonoBehaviour  where T : BaseMonoBehaviour
{
    protected static T _instance = null;

    protected BaseSingleton()
    {
        if (null != _instance)
        {
            LogUtil.LogError((typeof(T)).ToString()+"初始化不是NULL");
        }
        InitData();
    }

    public static T Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = FindObjectOfType(typeof(T)) as T;
            }
            return _instance;
        }
    }

    public virtual void InitData()
    {

    }
}