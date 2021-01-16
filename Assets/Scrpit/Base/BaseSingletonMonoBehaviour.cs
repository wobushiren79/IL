using UnityEditor;
using UnityEngine;

public class BaseSingletonMonoBehaviour<T> : BaseMonoBehaviour where T : BaseMonoBehaviour
{
    protected static volatile T instance;
    protected static object syncRoot = new Object();

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        T[] instances = FindObjectsOfType<T>();
                        if (!CheckUtil.ArrayIsNull(instances))
                        {
                            for (var i = 0; i < instances.Length; i++)
                            {
                                GameObject objItem = instances[i].gameObject;
                                if (i == 0)
                                {
                                    instance = instances[i];
                                    DontDestroyOnLoad(objItem);
                                }
                                else
                                {
                                    Destroy(objItem);
                                }
                            }
                        }
                        else
                        {
                            GameObject objInstance = new GameObject();
                            objInstance.name = typeof(T).Name;
                            instance = objInstance.AddComponent<T>();
                            DontDestroyOnLoad(objInstance);
                        }
                    }
                }
            }
            return instance;
        }
    }
}