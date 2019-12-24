using UnityEngine;
using UnityEditor;

public class StoryInfoDetailsBean 
{
    public enum StoryInfoDetailsTypeEnum
    {
        NpcPosition=1,//NPC位置
        Expression=2,//表情
        SceneInt=3,//场景互动
        Talk =11,//对话
        AutoNext=12,//指定时间跳转

        CameraPosition = 21,//摄像机位置
        CameraFollowCharacter=22,//摄像头跟随目标
    }

    public long story_id;
    //类型 1 NPC站位 11对话 12剧情自动跳转
    public int type;
    //事件顺序
    public int story_order;

    //NPCID
    public long npc_id;
    //NPC坐标
    public float npc_position_x;
    public float npc_position_y;
    //NPC编号
    public int npc_num;
    //npc朝向
    public int npc_face;

    //剧情自动跳转时间
    public float wait_time;
    //文本ID
    public long text_mark_id;
    //表情
    public int expression;

    public float camera_position_x;
    public float camera_position_y;
    public int camera_follow_character;

    //场景互动物体名称
    public string scene_intobj_name;
    //场景互动物体具体名称你
    public string scene_intcomponent_name;
    //场景互动物体方法
    public string scene_intcomponent_method;
    //场景互动物体方法参数
    public string scene_intcomponent_parameters;
}