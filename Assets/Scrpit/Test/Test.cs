using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public class Test : BaseMonoBehaviour
{

    private void Awake()
    {

    }

    public void Start()
    {
        int a = 0;
        for(int i = 0; i < 200; i++)
        {
            float value = Mathf.Pow(0.9f, a);
            LogUtil.Log(a +" :" + value);
            a += 1;
        }
    }

}
