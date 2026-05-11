using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;

public partial class AudioHandler
{
    protected int sourceNumber = 0;
    protected int sourceMaxNumber = 5;

    /// <summary>
    /// 循环播放音乐
    /// </summary>
    /// <param name="audioMusic"></param>
    /// <param name="volumeScale"></param>
    public void PlayMusicForLoop(AudioMusicEnum audioMusic)
    {
        switch (audioMusic)
        {
            case AudioMusicEnum.LangTaoSha:
                PlayMusicForLoop(1);
                break;
            case AudioMusicEnum.YangChunBaiXue:
                PlayMusicForLoop(2);
                break;
            case AudioMusicEnum.Main:
                PlayMusicForLoop(3);
                break;
            case AudioMusicEnum.Game:
                List<int> listGameClip = new List<int>()
                {
                    3,4,5,7,8
                };
                int randomIndexGame = RandomUtil.GetRandomDataByList(listGameClip);
                PlayMusicForLoop(randomIndexGame);
                break;
            case AudioMusicEnum.Battle:
                List<int> listBattleClip = new List<int>()
                {
                    6,9,12
                };
                int randomIndexBattle = RandomUtil.GetRandomDataByList(listBattleClip);
                PlayMusicForLoop(randomIndexBattle);
                break;
            case AudioMusicEnum.Rest:
                List<int> listRestClip = new List<int>()
                {
                    10,11
                };
                int randomIndexRest = RandomUtil.GetRandomDataByList(listRestClip);
                PlayMusicForLoop(randomIndexRest);
                break;
            case AudioMusicEnum.Marry:
                PlayMusicForLoop(13);
                break;
        }
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="sound">音效</param>
    /// <param name="volumeScale">音量大小</param>
    public void PlaySound(AudioSoundEnum sound)
    {
        switch (sound)
        {
            case AudioSoundEnum.Thunderstorm:
                PlaySound((int)AudioSoundEnum.Thunderstorm, manager.audioSourceForEnvironment);
                break;
            case AudioSoundEnum.Fight:
                int fightRandom = Random.Range(0, 2);
                if (fightRandom == 0)
                {
                    PlaySound(100020);
                }
                else if (fightRandom == 1)
                {
                    PlaySound(100021);
                }
                break;
            case AudioSoundEnum.Eat:
                int eatRandom = Random.Range(0, 2);
                if (eatRandom == 0)
                {
                    PlaySound(100030);
                }
                else if (eatRandom == 1)
                {
                    PlaySound(100031);
                }
                break;
            case AudioSoundEnum.None:
                break;
            default:
                PlaySound((int)sound);
                break;
        }
    }

    /// <summary>
    /// 播放环境音乐
    /// </summary>
    /// <param name="audioEnvironment"></param>
    public void PlayEnvironment(AudioEnvironmentEnum audioEnvironment)
    {
        PlayEnvironment((int)audioEnvironment);
    }
}