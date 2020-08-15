using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class GameConfigBean 
{
    //屏幕模式 0窗口  1全屏
    public int window = 0;
    //语言
    public string language="cn";
    //音效大小
    public float soundVolume = 0.5f;
    //音乐大小
    public float musicVolume = 0.5f;
    //环境音乐大小
    public float environmentVolume = 0.5f;
    //自动保存时间
    public float autoSaveTime = 30;

    //按键提示状态 1显示 0隐藏
    public int statusForKeyTip = 1;
   
}