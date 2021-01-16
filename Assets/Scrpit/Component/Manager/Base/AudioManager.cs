using UnityEngine;
using UnityEditor;

public class AudioManager : BaseManager
{
    public AudioBeanDictionary listMusicData;
    public AudioBeanDictionary listSoundData;
    public AudioBeanDictionary listEnvironmentData;

    /// <summary>
    /// 根据名字获取音乐
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public AudioClip GetMusicClip(string name)
    {
        return GetAudioClipByName(name, listMusicData);
    }

    /// <summary>
    /// 根据名字获取音效
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public AudioClip GetSoundClip(string name)
    {
        return GetAudioClipByName(name, listSoundData);
    }

    /// <summary>
    /// 根据名字获取环境音乐
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public AudioClip GetEnvironmentClip(string name)
    {
        return GetAudioClipByName(name, listEnvironmentData);
    }
}