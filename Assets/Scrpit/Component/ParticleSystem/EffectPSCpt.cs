using UnityEngine;
using UnityEditor;
using System;

public class EffectPSCpt : BaseMonoBehaviour
{
    protected ParticleSystem effectPS;
    public AudioSoundEnum soundType;

    private void Awake()
    {
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
        AudioHandler.Instance.PlaySound(soundType);
    }

    /// <summary>
    /// 粒子结束监听
    /// </summary>
    public void OnParticleSystemStopped()
    {
        Destroy(gameObject);
    }
}