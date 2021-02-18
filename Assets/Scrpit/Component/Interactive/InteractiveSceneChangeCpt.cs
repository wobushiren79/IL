using UnityEngine;
using UnityEditor;

public class InteractiveSceneChangeCpt : BaseInteractiveCpt
{
     
    public string interactiveContent;
    //需要跳转的场景
    public ScenesEnum changeScene;

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
                        GameScenesHandler.Instance.ChangeScene(changeScene);
                    }
                    else
                    {
                        ToastHandler.Instance.ToastHint(TextHandler.Instance.manager.GetTextById(1321));
                    }
                }
                else
                {

                    GameScenesHandler.Instance.ChangeScene(changeScene);
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