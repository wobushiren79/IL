using UnityEngine;
using UnityEditor;

public class MiniGameAccountHandler : BaseMiniGameHandler<MiniGameAccountBuilder, MiniGameAccountBean>
{
    public override void InitGame(MiniGameAccountBean miniGameData)
    {
        base.InitGame(miniGameData);
        //创建玩家
        miniGameBuilder.CreateUserCharacter(miniGameData.listUserGameData, miniGameData.playerPosition);
    }
}