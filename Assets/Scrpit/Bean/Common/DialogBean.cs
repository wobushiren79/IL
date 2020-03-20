using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class DialogBean 
{
    public string title;
    public string content;
    public string submitStr;
    public string cancelStr;
    //弹窗编号
    public int dialogPosition;
    //备注
    public string remark;
}