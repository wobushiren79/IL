using UnityEngine;
using UnityEditor;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;
public class LightCpt : BaseMonoBehaviour
{
    public enum LightStatusEnum
    {
        Open,
        Close,
    }

    public Light2D light2D;

    public LightStatusEnum lightStatus = LightStatusEnum.Close;

    private void Awake()
    {
        lightStatus = LightStatusEnum.Close;
    }

    public void SetLightColor(float r, float g, float b)
    {
        if (light2D == null)
            return;
        light2D.color = new Color(r, g, b);
    }

    public void SetLightColor(Color color)
    {
        if (light2D == null)
            return;
        light2D.color = color;
    }

    public virtual void OpenLight()
    {
        if (lightStatus == LightStatusEnum.Open)
            return;
        lightStatus = LightStatusEnum.Open;
        float targetIntensity = light2D.intensity;
        light2D.intensity = 0;
        if (light2D != null)
            light2D.gameObject.SetActive(true);
        float changeIntensity = 0;
        Tween tween = DOTween
            .To(() => changeIntensity, x => changeIntensity = x, targetIntensity, 3)
            .OnUpdate(() => { light2D.intensity = changeIntensity; })
            .OnKill(() => { light2D.intensity = targetIntensity; });
    }

    public virtual void CloseLight()
    {
        if (lightStatus == LightStatusEnum.Close)
            return;
        lightStatus = LightStatusEnum.Close;
        if (light2D != null)
            light2D.gameObject.SetActive(false);
    }
}