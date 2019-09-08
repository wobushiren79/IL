using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class Test : MonoBehaviour {

    private void Start()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start(); //开启计时器
                    //给闲置的工作人员分配工作
        Text();
        sw.Stop(); //停止计时器
        UnityEngine.Debug.Log(string.Format("total: {0} ", ""+sw.ElapsedTicks)); //打印计时器 毫秒数39ms
    }
    public void Text()
    {
        for(int i = 0; i < 100; i++)
        {

        }
    }
}
