using UnityEngine;
using UnityEditor;

public class WeatherForSunnyCpt : WeatherCpt
{
    public GameObject objCloudy;
    public ParticleSystem psCloudy;
    public override void OpenWeather(WeatherBean weatherData)
    {
        base.OpenWeather(weatherData);
        InitPS();
        objCloudy.SetActive(false);
        switch (weatherData.weatherType)
        {
            case WeatherTypeEnum.Sunny:
                SetSunny();
                break;
            case WeatherTypeEnum.Cloudy:
                SetCloudy();
                break;
        }
    }

    public void SetCloudy()
    {
        objCloudy.SetActive(true);
    }

    public void SetSunny()
    {

    }

    public void InitPS()
    {
        SceneBaseManager sceneBaseManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneBaseManager>();
        ParticleSystem.ShapeModule shapeModule = psCloudy.shape;
        shapeModule.scale = sceneBaseManager.scaleForSunny;
        shapeModule.position = sceneBaseManager.positionForSunny;
    }
}