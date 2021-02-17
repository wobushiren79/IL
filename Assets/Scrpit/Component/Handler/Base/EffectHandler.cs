using UnityEditor;
using UnityEngine;
using System.Collections;
using System;

public class EffectHandler : BaseHandler<EffectHandler, EffectManager>
{

    /// <summary>
    /// 播放粒子特效
    /// </summary>
    /// <param name="name"></param>
    /// <param name="effectPosition"></param>
    public GameObject PlayEffect(GameObject objContainer, string name, Vector3 effectPosition, float delayTime)
    {
        GameObject objEffect = manager.CreateEffect(objContainer, name);
        if (objEffect == null)
            return objEffect;
        objEffect.transform.position = effectPosition;
       
        EffectPSCpt effectPS = objEffect.GetComponent<EffectPSCpt>();
        if (effectPS)
        {
            effectPS.Play();
        }
        else
        {
            ParticleSystem[] listParticleSystem = objEffect.GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < listParticleSystem.Length; i++)
            {
                ParticleSystem particleSystem = listParticleSystem[i];
                ParticleSystem.MainModule psMain = particleSystem.main;
                psMain.loop = false;
                //psMain.stopAction = ParticleSystemStopAction.Callback;
                particleSystem.Play();
            }
        }
        if (delayTime <= 0)
        {
            return objEffect;
        }
        StartCoroutine(CoroutineForDelayDestroy(objEffect, delayTime));
        return objEffect;
    }

    public GameObject PlayEffect(string name, Vector3 effectPosition, float delayTime)
    {
        return PlayEffect(null, name, effectPosition, delayTime);
    }

    public GameObject PlayEffect(string name, Vector3 effectPosition)
    {
        return PlayEffect(name, effectPosition, 5);
    }

    public IEnumerator CoroutineForDelayDestroy(GameObject objEffect, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Destroy(objEffect);
    }

}