using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public class Test : BaseMonoBehaviour
{

    public int prefabNumber = 1000;
    public int findNumber = 1;

    private void Awake()
    {
        for (int i = 0; i < prefabNumber; i++)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.name = i+"";
            for (int f = 0; f < 10; f++)
            {
                GameObject objChild = GameObject.CreatePrimitive(PrimitiveType.Cube);
                objChild.transform.parent = obj.transform;
                objChild.name = i + " "+f;
                if (i == prefabNumber - 1 && f == 9)
                {
                    objChild.AddComponent<GameItemsManager>();
                    objChild.tag = "Data";
                }
            }
        }
        Stopwatch stopwatch = TimeUtil.GetMethodTimeStart();
        GameObject findObj = null;
        for (int i = 0; i < findNumber; i++)
        {
            findObj= GameObject.Find((prefabNumber-1)+" "+ 9);
        }
        TimeUtil.GetMethodTimeEnd("Find "+ findObj.name, stopwatch);

        stopwatch = TimeUtil.GetMethodTimeStart();
        GameItemsManager findCpt = null;
        for (int i = 0; i < findNumber; i++)
        {
             findCpt = GameObject.FindObjectOfType<GameItemsManager>();
        }
        TimeUtil.GetMethodTimeEnd("FindObjectOfType " + findCpt.name, stopwatch);

        stopwatch = TimeUtil.GetMethodTimeStart();
        GameObject findTag = null;
        for (int i = 0; i < findNumber; i++)
        {
            findTag = GameObject.FindGameObjectWithTag("Data");
        }
        TimeUtil.GetMethodTimeEnd("FindGameObjectWithTag " + findTag.name, stopwatch);
    }

    private void OnGUI()
    {

    }

}
