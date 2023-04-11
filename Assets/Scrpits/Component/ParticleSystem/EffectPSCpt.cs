using UnityEngine;
using UnityEditor;
using System;

public class EffectPSCpt : EffectBase
{
    protected ParticleSystem effectPS;
    public AudioSoundEnum soundType;

    private void Awake()
    {
        effectPS = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule mainModule = effectPS.main;
        mainModule.stopAction = ParticleSystemStopAction.Callback;
    }

    public override void PlayEffect()
    {
        base.PlayEffect();
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