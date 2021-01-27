using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;

public class AudioHandler : BaseHandler<AudioHandler, AudioManager>
{
    protected AudioListener _audioListener;
    protected AudioSource _audioSourceForMusic;
    protected AudioSource _audioSourceForSound;
    protected AudioSource _audioSourceForEnvironment;

    protected int sourceNumber = 0;
    protected int sourceMaxNumber = 5;

    public AudioListener audioSourceForListener
    {
        get
        {
            if (_audioListener == null)
            {
                _audioListener = FindWithTag<AudioListener>(TagInfo.Tag_AudioListener);
            }
            return _audioListener;
        }
    }

    public AudioSource audioSourceForMusic
    {
        get
        {
            if (_audioSourceForMusic == null)
            {
                _audioSourceForMusic = FindWithTag<AudioSource>(TagInfo.Tag_AudioMusic);
            }
            return _audioSourceForMusic;
        }
    }

    public AudioSource audioSourceForSound
    {
        get
        {
            if (_audioSourceForSound == null)
            {
                _audioSourceForSound = FindWithTag<AudioSource>(TagInfo.Tag_AudioSound);
            }
            return _audioSourceForSound;
        }
    }

    public AudioSource audioSourceForEnvironment
    {
        get
        {
            if (_audioSourceForEnvironment == null)
            {
                _audioSourceForEnvironment = FindWithTag<AudioSource>(TagInfo.Tag_AudioEnvironment);
            }
            return _audioSourceForEnvironment;
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void InitAudio()
    {
        audioSourceForMusic.volume = GameCommonInfo.GameConfig.musicVolume;
        audioSourceForSound.volume = GameCommonInfo.GameConfig.soundVolume;
        audioSourceForEnvironment.volume = GameCommonInfo.GameConfig.environmentVolume;
    }

    /// <summary>
    ///  循环播放音乐
    /// </summary>
    /// <param name="audioMusic"></param>
    public void PlayMusicForLoop(AudioMusicEnum audioMusic)
    {
        PlayMusicForLoop(audioMusic, GameCommonInfo.GameConfig.musicVolume);
    }

    /// <summary>
    /// 循环播放音乐
    /// </summary>
    /// <param name="audioMusic"></param>
    /// <param name="volumeScale"></param>
    public void PlayMusicForLoop(AudioMusicEnum audioMusic, float volumeScale)
    {
        AudioClip audioClip = null;
        switch (audioMusic)
        {
            case AudioMusicEnum.LangTaoSha:
                audioClip = manager.GetMusicClip("music_langtaosha_1");
                break;
            case AudioMusicEnum.YangChunBaiXue:
                audioClip = manager.GetMusicClip("music_yangchunbaixue_1");
                break;
            case AudioMusicEnum.Main:
                audioClip = manager.GetMusicClip("music_1");
                break;
            case AudioMusicEnum.Game:
                List<AudioClip> listGameClip = new List<AudioClip>()
                {
                    manager.GetMusicClip("music_1"),
                    manager.GetMusicClip("music_2"),
                    manager.GetMusicClip("music_3"),
                    manager.GetMusicClip("music_6"),
                    manager.GetMusicClip("music_7")
                };
                audioClip = RandomUtil.GetRandomDataByList(listGameClip);
                break;
            case AudioMusicEnum.Battle:
                List<AudioClip> listBattleClip = new List<AudioClip>()
                {
                    manager.GetMusicClip("music_4"),
                    manager.GetMusicClip("music_8")
                };
                audioClip = RandomUtil.GetRandomDataByList(listBattleClip);
                break;
        }
        if (audioClip != null)
        {
            audioSourceForMusic.clip = audioClip;
            audioSourceForMusic.volume = volumeScale;
            audioSourceForMusic.loop = true;
            audioSourceForMusic.Play();
        }
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="sound">音效</param>
    /// <param name="volumeScale">音量大小</param>
    public void PlaySound(AudioSoundEnum sound, Vector3 soundPosition, float volumeScale)
    {
        if (sourceNumber > sourceMaxNumber)
            return;
        AudioClip audioClip = null;
        AudioSource audioSource = audioSourceForSound;
        switch (sound)
        {
            case AudioSoundEnum.ButtonForNormal:
                audioClip = manager.GetSoundClip("sound_btn_3");
                break;
            case AudioSoundEnum.ButtonForBack:
                audioClip = manager.GetSoundClip("sound_btn_2");
                break;
            case AudioSoundEnum.ButtonForHighLight:
                audioClip = manager.GetSoundClip("sound_btn_1");
                break;
            case AudioSoundEnum.ButtonForShow:
                audioClip = manager.GetSoundClip("sound_btn_6");
                break;
            case AudioSoundEnum.PayMoney:
                audioClip = manager.GetSoundClip("sound_pay_1");
                break;
            case AudioSoundEnum.Reward:
                audioClip = manager.GetSoundClip("sound_reward_2");
                break;
            case AudioSoundEnum.Thunderstorm:
                audioClip = manager.GetSoundClip("sound_thunderstorm_1");
                audioSource = audioSourceForEnvironment;
                break;
            case AudioSoundEnum.Damage:
                audioClip = manager.GetSoundClip("sound_damage_1");
                break;
            case AudioSoundEnum.CountDownStart:
                audioClip = manager.GetSoundClip("sound_countdown_2");
                break;
            case AudioSoundEnum.CountDownEnd:
                audioClip = manager.GetSoundClip("sound_countdown_1");
                break;
            case AudioSoundEnum.HitWall:
                audioClip = manager.GetSoundClip("sound_hit_1");
                break;
            case AudioSoundEnum.HitCoin:
                // audioClip = manager.GetSoundClip("sound_hit_2");
                audioClip = manager.GetSoundClip("sound_pay_3");
                break;
            case AudioSoundEnum.ChangeSelect:
                audioClip = manager.GetSoundClip("sound_btn_5");
                break;
            case AudioSoundEnum.Shot:
                audioClip = manager.GetSoundClip("sound_shot_1");
                break;
            case AudioSoundEnum.GetCard:
                audioClip = manager.GetSoundClip("sound_card_1");
                break;
            case AudioSoundEnum.SetCard:
                audioClip = manager.GetSoundClip("sound_btn_3");
                break;
            case AudioSoundEnum.CardDraw:
                audioClip = manager.GetSoundClip("sound_btn_5");
                break;
            case AudioSoundEnum.CardWin:
                audioClip = manager.GetSoundClip("sound_btn_7");
                break;
            case AudioSoundEnum.CardLose:
                audioClip = manager.GetSoundClip("sound_hit_1");
                break;
            case AudioSoundEnum.Fight:
                audioClip = manager.GetSoundClip("sound_fight_1");
                break;
            case AudioSoundEnum.FightForKnife:
                audioClip = manager.GetSoundClip("sound_fight_knife_1");
                break;
            case AudioSoundEnum.UseMedicine:
                audioClip = manager.GetSoundClip("sound_medicine_1");
                break;
            case AudioSoundEnum.Show:
                audioClip = manager.GetSoundClip("sound_show_1");
                break;
            case AudioSoundEnum.Correct:
                audioClip = manager.GetSoundClip("sound_btn_5");
                break;
            case AudioSoundEnum.Error:
                audioClip = manager.GetSoundClip("sound_error_1");
                break;
            case AudioSoundEnum.Set:
                audioClip = manager.GetSoundClip("sound_set_1");
                break;
            case AudioSoundEnum.Cook:
                audioClip = manager.GetSoundClip("sound_cook_1");
                break;
            case AudioSoundEnum.Clean:
                audioClip = manager.GetSoundClip("sound_clean_1");
                break;
            case AudioSoundEnum.Eat:
                int eatRandom = Random.Range(0, 2);
                if (eatRandom == 0)
                {
                    audioClip = manager.GetSoundClip("sound_eat_1");
                }
                else if (eatRandom == 1)
                {
                    audioClip = manager.GetSoundClip("sound_eat_2");
                }
                break;
            case AudioSoundEnum.Lock:
                audioClip = manager.GetSoundClip("sound_lock_1");
                break;
            case AudioSoundEnum.Passive:
                audioClip = manager.GetSoundClip("sound_passive_1");
                break;
            case AudioSoundEnum.Dice:
                audioClip = manager.GetSoundClip("sound_dice_1");
                break;
            case AudioSoundEnum.Thunder:
                audioClip = manager.GetSoundClip("sound_thunder_1");
                break;
        }
        if (audioClip != null)
        {

            StartCoroutine(CoroutineForPlayOneShot(audioSource, audioClip, volumeScale));
            //audioSource.PlayOneShot(audioClip, volumeScale);
        }
        // AudioSource.PlayClipAtPoint(soundClip, soundPosition,volumeScale);
    }

    /// <summary>
    /// 协程播放音效
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="audioClip"></param>
    /// <param name="volumeScale"></param>
    /// <returns></returns>
    IEnumerator CoroutineForPlayOneShot(AudioSource audioSource, AudioClip audioClip, float volumeScale)
    {
        sourceNumber++;
        audioSource.PlayOneShot(audioClip, volumeScale);
        yield return new WaitForSeconds(audioClip.length);
        sourceNumber--;
    }

    public void PlaySound(AudioSoundEnum sound)
    {
        PlaySound(sound, Camera.main.transform.position, GameCommonInfo.GameConfig.soundVolume);
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
    public void PlayEnvironment(AudioEnvironmentEnum audioEnvironment, float volumeScale)
    {
        AudioClip audioClip = null;
        switch (audioEnvironment)
        {
            case AudioEnvironmentEnum.Rain:
                audioClip = manager.GetEnvironmentClip("environment_rain_1");
                break;
            case AudioEnvironmentEnum.Wind:
                audioClip = manager.GetEnvironmentClip("environment_wind_1");
                break;
        }
        audioSourceForEnvironment.volume = volumeScale;
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

    public void StopMusic()
    {
        audioSourceForMusic.clip = null;
        audioSourceForMusic.Stop();
    }

    /// <summary>
    /// 暂停
    /// </summary>
    public void PauseEnvironment()
    {
        audioSourceForEnvironment.Pause();
    }


    public void PauseMusic()
    {
        audioSourceForMusic.Pause();
    }


    /// <summary>
    /// 恢复
    /// </summary>
    public void RestoreEnvironment()
    {
        audioSourceForEnvironment.Play();
    }

    public void RestoreMusic()
    {
        audioSourceForMusic.Play();
    }
}