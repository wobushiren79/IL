using UnityEngine;
using UnityEditor;

public class StoryInfoDetailsBean 
{
    public long story_id;
    //类型 1 NPC站位 11对话 12剧情自动跳转
    public int type;
    //事件顺序
    public int order;

    //NPCID
    public long npc_id;
    //NPC坐标
    public float npc_position_x;
    public float npc_position_y;
    //NPC编号
    public int npc_num;

    //剧情自动跳转时间
    public float wait_time;
    //文本ID
    public long text_mark_id;
    //表情
    public int expression;
}