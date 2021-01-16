using UnityEngine;
using UnityEditor;

public class BaseUIChildComponent<T> : BaseMonoBehaviour
    where T : BaseUIComponent
{
    public T uiComponent;

    public virtual void Awake()
    {
        AutoLinkUI();
    }

    public virtual void Close()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
    }
}