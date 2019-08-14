using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class StoryInfoBean : BaseBean
{
    //事件发生场景 1客栈 2城镇
    public int story_scene;
    //事件发生位置
    public float position_x;
    public float position_y;
}