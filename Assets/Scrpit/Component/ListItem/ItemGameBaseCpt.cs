using UnityEngine;
using UnityEditor;

public class ItemGameBaseCpt : BaseMonoBehaviour
{
    public BaseUIComponent uiComponent;

    /// <summary>
    /// 获取UIComponent
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetUIComponent<T>() where T : BaseUIComponent
    {
        return uiComponent as T;
    }

    /// <summary>
    /// 获取UIManger
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetUIManager<T>() where T : BaseUIManager
    {
        return uiComponent.GetUIMananger<T>();
    }

    /// <summary>
    /// 获取基础UIManager
    /// </summary>
    /// <returns></returns>
    public BaseUIManager GetUIManager()
    {
        return uiComponent.uiManager;
    }
}