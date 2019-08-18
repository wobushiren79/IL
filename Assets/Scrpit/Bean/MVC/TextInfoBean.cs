using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class TextInfoBean : BaseBean
{
    //类型 0默认文本 1黑幕标题
    public int type;
    //文本编号
    public long mark_id;
    //文本发起对象ID
    public long user_id;
    //文本内容
    public string content;
    //文本顺序
    public int order;
    //停留时间
    public float wait_time;
}