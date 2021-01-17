using UnityEditor;
using UnityEngine;

public class MsgHandler : BaseUIHandler<MsgHandler,MsgManager>
{
    protected override void Awake()
    {
        sortingOrder = 3;
        base.Awake();
    }
}