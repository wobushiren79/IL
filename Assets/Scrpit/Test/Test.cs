using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;

public class Test : BaseMonoBehaviour
{

    public Camera cameraTest1;
    public Camera cameraTest2;
    public Camera cameraTest3;

    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer.transform.DOLocalMoveY(5, 5).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnGUI()
    {

        if (GUI.Button(new Rect(20, 40, 80, 20), "测试1"))
        {
            cameraTest1.gameObject.SetActive(true);
            cameraTest2.gameObject.SetActive(false);
            cameraTest3.gameObject.SetActive(false);
        }
        if (GUI.Button(new Rect(40, 60, 80, 20), "测试2"))
        {
            cameraTest1.gameObject.SetActive(false);
            cameraTest2.gameObject.SetActive(true);
            cameraTest3.gameObject.SetActive(false);
        }
        if (GUI.Button(new Rect(60, 80, 80, 20), "测试2"))
        {
            cameraTest1.gameObject.SetActive(false);
            cameraTest2.gameObject.SetActive(false);
            cameraTest3.gameObject.SetActive(true);
        }
    }

    public void Start()
    {

    }


}
