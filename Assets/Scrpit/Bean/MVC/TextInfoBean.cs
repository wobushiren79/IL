using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class TextInfoBean : BaseBean
{
    //文本编号
    public long mark_id;
    //文本发起对象ID
    public long user_id;
    //文本内容
    public string content;
    //文本顺序
    public long order;
}