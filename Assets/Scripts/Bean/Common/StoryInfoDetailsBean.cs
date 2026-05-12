using UnityEngine;
using UnityEditor;

public partial class StoryInfoDetailsBean
{
    public enum StoryInfoDetailsTypeEnum
    {
        NpcPosition = 1,//NPC位置
        NpcExpression = 2,//表情
        NpcEquip = 3,//NPC穿着
        NpcDestory = 4,//销毁NPC

        Talk = 11,//对话
        AutoNext = 12,//指定跳转
        PropPosition = 13,//道具位置
        WorkerPosition = 14,//员工位置
        Effect = 15,//粒子特效
        SetTime = 16,//设置时间

        CameraPosition = 21,//摄像机位置
        CameraFollowCharacter = 22,//摄像头跟随目标

        AudioSound = 31,//音效播放
        AudioMusic = 32,//音乐播放

        SceneInt = 41,//场景互动
    }

    //获取NPC装备
    public void GetNpcEquip(SexEnum sex,out long hatId, out long clothesId, out long shoesId)
    {
        hatId = GetNpcEquip(sex, npc_hat);
        clothesId = GetNpcEquip(sex, npc_clothes);
        shoesId = GetNpcEquip(sex, npc_shoes);
    }

    protected long GetNpcEquip(SexEnum sex,string data)
    {
        long hatId = 0;
        if (data.IsNull())
        {
            hatId = -1;
        }
        else
        {
            string[] hatList = data.SplitForArrayStr(',');
            if (hatList.Length >= 2)
            {
                switch (sex)
                {
                    case SexEnum.Man:
                        hatId = long.Parse(hatList[0]);
                        break;
                    case SexEnum.Woman:
                        hatId = long.Parse(hatList[1]);
                        break;
                    default:
                        hatId = long.Parse(hatList[0]);
                        break;
                }
            }
            else
            {
                hatId = long.Parse(npc_hat);
            }
        }
        return hatId;
    }

    /// <summary>
    /// 获取播放的音效
    /// </summary>
    /// <returns></returns>
    public AudioSoundEnum GetAudioSound()
    {
        return (AudioSoundEnum)audio_sound;
    }
    public AudioMusicEnum GetAudioMusic()
    {
        return (AudioMusicEnum)audio_music;
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