using UnityEngine;
using UnityEditor;
using UnityEngine.Experimental.Rendering.LWRP;

public class LightCpt : BaseMonoBehaviour
{
    public enum LightStatusEnum
    {
        Open,
        Close,
    }

    public UnityEngine.Experimental.Rendering.Universal.Light2D light2D;

    public LightStatusEnum lightStatus = LightStatusEnum.Open;

    public void SetLightColor(float r, float g, float b)
    {
        if (light2D == null)
            return;
        light2D.color = new Color(r, g, b);
    }

    public virtual void OpenLight()
    {
        lightStatus = LightStatusEnum.Open;
        if (light2D != null)
            light2D.gameObject.SetActive(true);
    }

    public virtual void CloseLight()
    {
        lightStatus = LightStatusEnum.Close;
        if (light2D != null)
            light2D.gameObject.SetActive(false);
    }
}