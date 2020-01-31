using UnityEngine;
using UnityEditor;

public class AudioHandler : BaseHandler
{
    protected AudioListener audioListener;
    protected AudioSource audioSourceForCamera;
    protected AudioManager audioManager;

    public virtual void Awake()
    {
        audioListener = Find<AudioListener>(ImportantTypeEnum.MainCamera);
        audioSourceForCamera = Find<AudioSource>(ImportantTypeEnum.MainCamera);
        audioManager = Find<AudioManager>(ImportantTypeEnum.AudioManager);
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="sound">音效</param>
    /// <param name="volumeScale">音量大小</param>
    public void PlaySound(SoundEnum sound, Vector3 soundPosition, float volumeScale)
    {
        AudioClip soundClip = null;
        switch (sound)
        {
            case SoundEnum.ButtonForNormal:
                soundClip = audioManager.GetSoundClip("sound_btn_3");
                break;
            case SoundEnum.ButtonForBack:
                soundClip = audioManager.GetSoundClip("sound_btn_2");
                break;
            case SoundEnum.ButtonForHighLight:
                soundClip = audioManager.GetSoundClip("sound_btn_1");
                break;
            case SoundEnum.PayMoney:
                soundClip = audioManager.GetSoundClip("sound_pay_1");
                break;
        }
        if (soundClip != null)
            AudioSource.PlayClipAtPoint(soundClip, soundPosition,volumeScale);
    }

    public void PlaySound(SoundEnum sound)
    {
        PlaySound(sound, Camera.main.transform.position, 1);
    }
    public void PlaySound(SoundEnum sound, float volumeScale)
    {
        PlaySound(sound, Camera.main.transform.position, volumeScale);
    }
    public void PlaySound(SoundEnum sound , Vector3 soundPosition)
    {
        PlaySound(sound, soundPosition, 1);
    }
}