using UnityEngine;
using UnityEditor;
using System;

public class EffectPSCpt : BaseMonoBehaviour
{
    protected ParticleSystem effectPS;
    public AudioSoundEnum soundType;
    protected AudioHandler audioHandler;

    private void Awake()
    {
        //音效
        audioHandler = Find<AudioHandler>( ImportantTypeEnum.AudioHandler);

        effectPS = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule mainModule = effectPS.main;
        mainModule.loop = false;
        mainModule.stopAction = ParticleSystemStopAction.Callback;
    }

    /// <summary>
    /// 播放
    /// </summary>
    public void Play()
    {
        effectPS.Play();
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