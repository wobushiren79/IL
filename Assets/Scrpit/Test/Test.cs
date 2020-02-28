using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public class Test : BaseMonoBehaviour
{

   public  GameObject objModel;
    private void Awake()
    {

    }

    public void Start()
    {
        Instantiate(gameObject, objModel);
        LogUtil.Log("1");
    }

}
