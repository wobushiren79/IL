using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AudioManager : BaseManager
{
    public AudioBeanDictionary listMusicData = new AudioBeanDictionary();
    public AudioBeanDictionary listSoundData = new AudioBeanDictionary();
    public AudioBeanDictionary listEnvironmentData = new AudioBeanDictionary();

    /// <summary>
    /// 根据名字获取音乐
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public AudioClip GetMusicClip(string name)
    {
        return LoadClipData(1, name);
    }

    /// <summary>
    /// 根据名字获取音效
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public AudioClip GetSoundClip(string name)
    {
        return LoadClipData(2, name);
    }

    /// <summary>
    /// 根据名字获取环境音乐
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public AudioClip GetEnvironmentClip(string name)
    {
        return LoadClipData(3, name);
    }


    protected AudioClip LoadClipData(int type, string name)
    {
        string dataPath = "audio/";
        AudioBeanDictionary dicData = new AudioBeanDictionary();
        AudioClip audioClip = null;
        switch (type)
        {
            case 1:
                dicData = listMusicData;
                dataPath += "music";
                break;
            case 2:
                dicData = listSoundData;
                dataPath += "sound";
                break;
            case 3:
                dicData = listEnvironmentData;
                dataPath += "environment";
                break;
        }
        if (dicData.TryGetValue(name, out audioClip))
        {
            return audioClip;
        }
        audioClip = LoadAssetUtil.SyncLoadAsset<AudioClip>(dataPath, name);
        if (audioClip != null)
        {
            dicData.Add(name, audioClip);
        }
        return audioClip;
    }
}