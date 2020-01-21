using UnityEngine;
using UnityEditor;

public class AudioManager : BaseManager
{

    public AudioBeanDictionary listMusicData;
    public AudioBeanDictionary listSoundData;

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

}