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

        //打开UI
        UIMiniGameDebate uiMiniGameDebate= (UIMiniGameDebate)uiGameManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.MiniGameDebate));
        uiMiniGameDebate.SetData((MiniGameCharacterForDebateBean)miniGameData.listUserGameData[0], (MiniGameCharacterForDebateBean)miniGameData.listEnemyGameData[0]);

    }

    public override void StartGame()
    {
        base.StartGame();
    }


}