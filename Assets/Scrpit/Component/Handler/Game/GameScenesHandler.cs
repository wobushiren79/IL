using UnityEditor;
using UnityEngine;

public class GameScenesHandler : BaseHandler<GameScenesHandler, GameScenesManager>
{

    public void ChangeScene(ScenesEnum scenes)
    {
        //停止所有控制
        GameControlHandler.Instance.EndAllControl();
        //停止时间
        GameTimeHandler.Instance.SetTimeStop();
        //关闭所有UI
        UIHandler.Instance.manager.CloseAllUI();
        //切换场景
        SceneUtil.SceneChange(scenes);
    }

}