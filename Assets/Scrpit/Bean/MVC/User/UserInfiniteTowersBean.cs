using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class UserInfiniteTowersBean
{
    public List<string> listMembers = new List<string>();//成员
    public bool isSend = false;//是否是派遣
    public long layer = 0;//层数
    public float proForSend = 0;//派遣进度
}