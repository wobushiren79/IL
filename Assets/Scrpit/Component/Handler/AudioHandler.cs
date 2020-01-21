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
    /// <param name="sound"></param>
    public void PlaySound(SoundEnum sound)
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
        }
        if (soundClip != null)
            AudioSource.PlayClipAtPoint(soundClip, Camera.main.transform.position);
    }
}