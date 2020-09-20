using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;

using Pathfinding;
public class Test : BaseMonoBehaviour
{

    public ScrollGridVertical scrollGridVertical;

    private void Awake()
    {
        scrollGridVertical.SetCellCount(100);
        scrollGridVertical.AddCellListener(CallBack);
    }

    private void Update()
    {

    }

    public void CallBack(ScrollGridCell scrollGrid)
    {

    }


}
