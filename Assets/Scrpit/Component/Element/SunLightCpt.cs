using UnityEngine;
using UnityEditor;


public class SunLightCpt : LightCpt
{
    //最白天时颜色
    public Color whiteColor;
    //最暗时颜色
    public Color darkColor;
    //可控颜色加成
    public Color offsetColor;

    protected WeatherHandler weatherHandler;

    private void Awake()
    {
        weatherHandler = Find<WeatherHandler>(ImportantTypeEnum.WeatherHandler);
    }

    private void Update()
    {
        HandleForTime();
    }

    /// <summary>
    /// 时间处理
    /// </summary>
    public void HandleForTime()
    {
        GameTimeHandler.Instance.GetTime(out float hour, out float min);

        float leap = 0;
        if (hour >= 0 && hour <= 12)
        {
            leap = (hour * 60 + min) / (12f * 60f);
        }
        else if (hour > 12 && hour < 15)
        {
            leap = 1;
        }
        else if (hour >= 15 && hour <= 24)
        {
            leap = 1 - (((hour - 15) * 60 + min) / (10f * 60f));
        }
        else
        {
            leap = 0;
        }
        Color sunColor = Color.Lerp(darkColor , whiteColor + offsetColor, leap);
        SetSunColor(sunColor);
    }

    /// <summary>
    /// 设置太阳颜色
    /// </summary>
    /// <param name="r"></param>
    /// <param name="g"></param>
    /// <param name="b"></param>
    public void SetSunColor(float r, float g, float b)
    {
        SetLightColor(r, g, b);
    }

    /// <summary>
    /// 设置太阳颜色
    /// </summary>
    /// <param name="color"></param>
    public void SetSunColor(Color color)
    {
        SetLightColor(color);
    }

    /// <summary>
    /// 设置加成颜色
    /// </summary>
    /// <param name="offsetColor"></param>
    public void SetOffsetColor(Color offsetColor)
    {
        this.offsetColor = offsetColor;
    }

}