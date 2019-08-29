using UnityEngine;
using UnityEditor;

public class WeatherHandler : BaseMonoBehaviour
{
    public enum WeatherStatusEnum
    {
        Sunny,//普通
        Rain,//下雨
        Snow,//下雪
        Fog,//起雾
        Wind,//大风
    }
    public WeatherStatusEnum weatherStatus = WeatherStatusEnum.Sunny;

    [Header("控件")]
    //太阳光
    public SunLightCpt sunLight;
    [Header("数据")]
    public GameTimeHandler gameTimeHandler;

    private void Update()
    {
        switch (weatherStatus)
        {
            case WeatherStatusEnum.Sunny:
                SetWeahterSunny();
                break;
        }
    }

    /// <summary>
    /// 设置天气
    /// </summary>
    /// <param name="weatherStatus"></param>
    public void SetWeahter(WeatherStatusEnum weatherStatus)
    {
        this.weatherStatus = weatherStatus;
    }

    /// <summary>
    /// 设置晴天天气
    /// </summary>
    public void SetWeahterSunny()
    {
        if (gameTimeHandler == null || sunLight == null)
            return;
        gameTimeHandler.GetTime(out float hour, out float min);
        if (hour >= 0 && hour < 7)
        {
            sunLight.SetSunColor(0.7f, 0.7f, 0.8f);
        }
        else if (hour >= 7 && hour < 10)
        {
            ChangeColor(new Vector3(0.7f, 0.7f, 0.8f), new Vector3(1, 1, 1), 3f, (hour - 7) + (min / 60f), out Vector3 outColor);
            sunLight.SetSunColor(outColor.x, outColor.y, outColor.z);
        }
        else if (hour >= 10 && hour < 17)
        {
            sunLight.SetSunColor(1, 1, 1);
        }
        else if (hour >= 17 && hour < 24)
        {
            ChangeColor(new Vector3(1, 1, 1), new Vector3(0.2f, 0.2f, 0.3f), 7f, (hour - 17) + (min / 60f), out Vector3 outColor);
            sunLight.SetSunColor(outColor.x, outColor.y, outColor.z);
        }
        else
        {
            sunLight.SetSunColor(0.2f, 0.2f, 0.3f);
        }
    }

    private void ChangeColor(Vector3 oldChange, Vector3 newChange, float changeHour, float valueHour, out Vector3 outColor)
    {
        float tempR = newChange.x - oldChange.x;
        float tempG = newChange.y - oldChange.y;
        float tempB = newChange.z - oldChange.z;

        float itemR = tempR / changeHour;
        float itemG = tempG / changeHour;
        float itemB = tempB / changeHour;

        outColor = new Vector3(valueHour * itemR + oldChange.x, valueHour * itemG + oldChange.y, valueHour * itemB + oldChange.z);
    }
}