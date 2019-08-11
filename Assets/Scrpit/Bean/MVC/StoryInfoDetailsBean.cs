using UnityEngine;
using UnityEditor;

public class StoryInfoDetailsBean 
{
    public long story_id;
    //类型 1 NPC站位 11对话
    public int type;
    //事件顺序
    public int order;

    //NPCID
    public long npc_id;
    //NPC坐标
    public float npc_position_x;
    public float npc_position_y;
}