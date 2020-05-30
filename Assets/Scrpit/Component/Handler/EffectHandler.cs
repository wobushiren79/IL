using UnityEngine;
using UnityEditor;

public class EffectHandler : BaseMonoBehaviour
{
    protected EffectManager effectManager;

    public void Awake()
    {
        effectManager = Find<EffectManager>(ImportantTypeEnum.EffectManager);
    }

    /// <summary>
    /// 播放粒子特效
    /// </summary>
    /// <param name="objEffectContainer"></param>
    /// <param name="effectName"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public GameObject PlayEffectPS(GameObject objEffectContainer, string effectName, Vector3 position)
    {
        GameObject objEffectModel = effectManager.GetEffectPSByName(effectName);
        if (objEffectModel == null)
            return null;
        GameObject objEffect = Instantiate(objEffectContainer, objEffectModel, position);
        EffectPSCpt effectPS = objEffect.GetComponent<EffectPSCpt>();
        if (effectPS != null)
            effectPS.Play();
        return objEffect;
    }
}