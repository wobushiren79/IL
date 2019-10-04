using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;

public class UIMiniGameEnd : BaseUIComponent
{
    //游戏结果
    public Text tvGameResult;

    public void SetData()
    {
        GameCommonInfo.GetUITextById(41);
        GameCommonInfo.GetUITextById(42);
    }
}