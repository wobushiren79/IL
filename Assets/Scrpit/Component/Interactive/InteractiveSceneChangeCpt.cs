using UnityEngine;
using UnityEditor;

public class InteractiveSceneChangeCpt : BaseInteractiveCpt
{
    
    
    public string interactiveContent;
    //需要跳转的场景
    public ScenesEnum changeScene;

    protected ToastManager toastManager;


    private void Awake()
    {
        toastManager = Find<ToastManager>(ImportantTypeEnum.ToastManager);
    }

    public override void InteractiveDetection(CharacterInteractiveCpt characterInt)
    {
        if (Input.GetButtonDown(InputInfo.Interactive_E))
        {
            if(changeScene== ScenesEnum.GameSquareScene || changeScene == ScenesEnum.GameForestScene)
            {
                ToastHandler.Instance.ToastHint("您被不可思议的力量阻挡了去路（暂未开放）");
            }
            else
            {
                if (changeScene == ScenesEnum.GameMountainScene)
                {
                    //如果山顶 特殊天气不能前往
                    WeatherBean weatherData = GameCommonInfo.CurrentDayData.weatherToday;
                    if (weatherData!= null && 
                        (weatherData.weatherType == WeatherTypeEnum.Cloudy
                        || weatherData.weatherType == WeatherTypeEnum.Sunny))
                    {
                        SceneUtil.SceneChange(changeScene);
                    }
                    else
                    {
                        ToastHandler.Instance.ToastHint(GameCommonInfo.GetUITextById(1321));
                    }
                }
                else
                {

                    SceneUtil.SceneChange(changeScene);
                }
            }     
        }
    }

    public override void InteractiveEnd(CharacterInteractiveCpt characterInt)
    {
        characterInt.CloseInteractive();
    }

    public override void InteractiveStart(CharacterInteractiveCpt characterInt)
    {
        characterInt.ShowInteractive(interactiveContent);
    }
}