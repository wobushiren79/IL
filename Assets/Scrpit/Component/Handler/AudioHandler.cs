using UnityEngine;
using UnityEditor;

public class AudioHandler : BaseHandler
{
    protected AudioListener audioListener;
    protected AudioSource audioSourceForCamera;
    protected AudioManager audioManager;

    private void Awake()
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
    public void PlaySound(SoundEnum sound, float volumeScale)
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
        }
        if (soundClip != null)
            AudioSource.PlayClipAtPoint(soundClip, Camera.main.transform.position, volumeScale);
    }

    public void PlaySound(SoundEnum sound)
    {
        PlaySound(sound, 1);
    }
}