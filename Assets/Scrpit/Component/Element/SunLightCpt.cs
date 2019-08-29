using UnityEngine;
using UnityEditor;
using UnityEngine.Experimental.Rendering.LWRP;

public class SunLightCpt : BaseMonoBehaviour
{
    public Light2D sunLight;

    public void SetSunColor(float r,float g,float b)
    {
        if (sunLight == null)
            return;
        sunLight.color = new Color(r, g, b);
    }

}