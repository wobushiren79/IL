using UnityEngine;
using UnityEditor;

public class StoryInfoDetailsBean
{
    public enum StoryInfoDetailsTypeEnum
    {
        NpcPosition = 1,//NPC位置
        Expression = 2,//表情
        SceneInt = 3,//场景互动
        NpcDestory = 4,//销毁NPC

        Talk = 11,//对话
        AutoNext = 12,//指定时间跳转
        PropPosition = 13,//道具位置

        CameraPosition = 21,//摄像机位置
        CameraFollowCharacter = 22,//摄像头跟随目标
        AudioSound = 31,//音效播放
    }

    public long story_id;
    //类型 1 NPC站位 11对话 12剧情自动跳转
    public int type;
    //事件顺序
    public int story_order;

    public float position_x;
    public float position_y;

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

    //摧毁的NPCID
    public string npc_destroy;

    //场景互动物体名称
    public string scene_intobj_name;
    //场景互动物体具体名称你
    public string scene_intcomponent_name;
    //场景互动物体方法
    public string scene_intcomponent_method;
    //场景互动物体方法参数
    public string scene_intcomponent_parameters;

    //播放的音效
    public int audio_sound;

    /// <summary>
    /// 获取播放的音效
    /// </summary>
    /// <returns></returns>
    public AudioSoundEnum GetAudioSound()
    {
        return (AudioSoundEnum)audio_sound;
    }

    /// <summary>
    /// 获取类型
    /// </summary>
    /// <returns></returns>
    public StoryInfoDetailsTypeEnum GetStoryInfoDetailsType()
    {
        return (StoryInfoDetailsTypeEnum)type;
    }
}