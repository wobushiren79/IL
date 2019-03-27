using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class GameConfigBean 
{
    //语言
    public string language="cn";
    //音效大小
    public float soundVolume = 1;
    //音乐大小
    public float musicVolume = 1;
    //自动保存时间
    public float autoSaveTime = 30;
   
}