using UnityEngine;
using UnityEditor;

public class BaseUIChildComponent<T> : BaseMonoBehaviour
    where T : BaseUIComponent
{
    public T uiComponent;

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }
}