using UnityEngine;
using UnityEditor;


public class SunLightCpt : LightCpt
{

    /// <summary>
    /// 设置太阳颜色
    /// </summary>
    /// <param name="r"></param>
    /// <param name="g"></param>
    /// <param name="b"></param>
    public void SetSunColor(float r,float g,float b) {
        SetLightColor(r, g, b);
    }

}