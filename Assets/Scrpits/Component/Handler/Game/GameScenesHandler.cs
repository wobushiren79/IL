using UnityEditor;
using UnityEngine;

public class GameScenesHandler : BaseHandler<GameScenesHandler, GameScenesManager>
{

    public void ChangeScene(ScenesEnum scenes)
    {
        //关门所有天气
        GameWeatherHandler.Instance.CloseWeather();
        //停止所有控制
        GameControlHandler.Instance.EndAllControl();
        //停止时间
        GameTimeHandler.Instance.SetTimeStop();
        //关闭所有UI
        UIHandler.Instance.CloseAllUI();
        //关闭所有音乐
        AudioHandler.Instance.StopMusic();
        //切换场景
        SceneUtil.SceneChange(scenes);
    }

}