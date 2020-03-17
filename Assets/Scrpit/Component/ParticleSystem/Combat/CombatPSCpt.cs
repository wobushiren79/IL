using UnityEngine;
using UnityEditor;
using System;

public class CombatPSCpt : BaseMonoBehaviour
{
    public ParticleSystem combatPS;
    public AudioSoundEnum soundType;
    protected AudioHandler audioHandler;

    private void Awake()
    {
        //音效
        audioHandler = Find<AudioHandler>( ImportantTypeEnum.AudioHandler);

        combatPS = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule mainModule = combatPS.main;
        mainModule.loop = false;
        mainModule.stopAction = ParticleSystemStopAction.Callback;
    }

    /// <summary>
    /// 播放
    /// </summary>
    public void Play()
    {
        combatPS.Play();
        audioHandler.PlaySound(soundType);
    }

    /// <summary>
    /// 粒子结束监听
    /// </summary>
    public void OnParticleSystemStopped()
    {
        Destroy(gameObject);
    }
}