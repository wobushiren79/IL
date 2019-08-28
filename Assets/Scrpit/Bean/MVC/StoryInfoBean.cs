﻿using UnityEngine;
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

    //该事件是否可以反复触发
    public int trigger_loop;
    //事件触发条件
    public int trigger_date_year;
    public int trigger_date_month;
    public int trigger_date_day;
}