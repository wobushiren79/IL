using UnityEngine;
using UnityEditor;

public class MiniGameDebateHandler : BaseMiniGameHandler<MiniGameDebateBuilder, MiniGameDebateBean>
{
    public override void InitGame(MiniGameDebateBean miniGameData)
    {
        base.InitGame(miniGameData);
        //创建角色
        miniGameBuilder.CreateAllCharacter(miniGameData.listUserGameData, miniGameData.listEnemyGameData, miniGameData.debatePosition);
        //设置摄像机位置
        controlHandler.StartControl(ControlHandler.ControlEnum.MiniGameDebate);
        controlHandler.GetControl().SetCameraPosition(miniGameData.debatePosition);
    }

    public override void StartGame()
    {
        base.StartGame();
    }


}