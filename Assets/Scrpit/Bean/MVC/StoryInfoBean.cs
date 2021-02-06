using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class StoryInfoBean : BaseBean
{
    //事件发生场景 1客栈 2城镇 3竞技场
    public int story_scene;
    //地点类型
    public int location_type;
    //发生地外还是里
    public int out_in;
    //事件发生位置
    public float position_x;
    public float position_y;

    //该事件是否可以反复触发
    public int trigger_loop;
    //备注
    public string note;

    //触发条件
    public string trigger_condition;

    public ScenesEnum GetStoryScene()
    {
        return (ScenesEnum)story_scene;
    }
}