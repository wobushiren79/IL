using UnityEngine;
using UnityEditor;

public class WeatherForFogCpt : WeatherCpt
{
    public GameObject objFog;
    public ParticleSystem psFog;

    public override void OpenWeather(WeatherBean weatherData)
    {
        base.OpenWeather(weatherData);
        InitPS(); 
        if (objFog != null)
            objFog.SetActive(false);
        switch (weatherData.weatherType)
        {
            case WeatherTypeEnum.Fog:
                SetFog();
                break;
        }
    }

    public void SetFog()
    {
        if(objFog!=null)
        objFog.SetActive(true);
    }

    public void InitPS()
    {
        SceneBaseManager sceneBaseManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneBaseManager>();
        ParticleSystem.ShapeModule shapeModule = psFog.shape;
        shapeModule.scale = sceneBaseManager.scaleRangeForFog;
    }
}