using UnityEngine;
using UnityEditor;

public class AudioHandler : BaseHandler
{
    protected AudioListener audioListener;
    protected AudioSource audioSourceForMusic;
    protected AudioSource audioSourceForEnvironment;
    protected AudioManager audioManager;

    public virtual void Awake()
    {
        audioListener = Find<AudioListener>(ImportantTypeEnum.MainCamera);
        audioManager = Find<AudioManager>(ImportantTypeEnum.AudioManager);
        audioSourceForMusic = CptUtil.GetCptInChildrenByName<AudioSource>(Camera.main.gameObject, "Music");
        audioSourceForEnvironment = CptUtil.GetCptInChildrenByName<AudioSource>(Camera.main.gameObject, "Environment");
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="sound">音效</param>
    /// <param name="volumeScale">音量大小</param>
    public void PlaySound(AudioSoundEnum sound, Vector3 soundPosition, float volumeScale)
    {
        AudioClip audioClip = null;
        switch (sound)
        {
            case AudioSoundEnum.ButtonForNormal:
                audioClip = audioManager.GetSoundClip("sound_btn_3");
                break;
            case AudioSoundEnum.ButtonForBack:
                audioClip = audioManager.GetSoundClip("sound_btn_2");
                break;
            case AudioSoundEnum.ButtonForHighLight:
                audioClip = audioManager.GetSoundClip("sound_btn_1");
                break;
            case AudioSoundEnum.PayMoney:
                audioClip = audioManager.GetSoundClip("sound_pay_1");
                break;
            case AudioSoundEnum.Reward:
                audioClip = audioManager.GetSoundClip("sound_reward_2");
                break;
            case AudioSoundEnum.Thunderstorm:
                audioClip = audioManager.GetSoundClip("sound_thunderstorm_1");
                break;
            case AudioSoundEnum.Damage:
                audioClip = audioManager.GetSoundClip("sound_damage_1");
                break;
            case AudioSoundEnum.CountDownStart:
                audioClip = audioManager.GetSoundClip("sound_countdown_2");
                break;
            case AudioSoundEnum.CountDownEnd:
                audioClip = audioManager.GetSoundClip("sound_countdown_1");
                break;
            case AudioSoundEnum.HitWall:
                audioClip = audioManager.GetSoundClip("sound_hit_1");
                break;
            case AudioSoundEnum.HitCoin:
                audioClip = audioManager.GetSoundClip("sound_hit_2");
                break;
        }
        if (audioClip != null)
            audioSourceForMusic.PlayOneShot(audioClip, volumeScale);
        // AudioSource.PlayClipAtPoint(soundClip, soundPosition,volumeScale);
    }

    public void PlaySound(AudioSoundEnum sound)
    {
        PlaySound(sound, Camera.main.transform.position, 1);
    }
    public void PlaySound(AudioSoundEnum sound, float volumeScale)
    {
        PlaySound(sound, Camera.main.transform.position, volumeScale);
    }
    public void PlaySound(AudioSoundEnum sound, Vector3 soundPosition)
    {
        PlaySound(sound, soundPosition, 1);
    }

    /// <summary>
    /// 播放环境音乐
    /// </summary>
    /// <param name="audioEnvironment"></param>
    public void PlayEnvironment(AudioEnvironmentEnum audioEnvironment)
    {
        AudioClip audioClip = null;
        switch (audioEnvironment)
        {
            case AudioEnvironmentEnum.Rain:
                audioClip = audioManager.GetEnvironmentClip("environment_rain_1");
                break;
            case AudioEnvironmentEnum.Wind:
                audioClip = audioManager.GetEnvironmentClip("environment_wind_1");
                break;
        }
        audioSourceForEnvironment.clip = audioClip;
        audioSourceForEnvironment.Play();
    }
    /// <summary>
    /// 停止播放
    /// </summary>
    public void StopEnvironment()
    {
        audioSourceForEnvironment.clip = null;
        audioSourceForEnvironment.Stop();
    }
}