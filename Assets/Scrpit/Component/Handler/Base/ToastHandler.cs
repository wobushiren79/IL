using UnityEditor;
using UnityEngine;

public class ToastHandler : BaseUIHandler<ToastHandler,ToastManager>
{
    protected override void Awake()
    {
        sortingOrder = 5;
        base.Awake();
    }

}