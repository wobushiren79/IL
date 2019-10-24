using UnityEngine;
using UnityEditor;

public class MiniGameCookingHandler : BaseMiniGameHandler<MiniGameCookingBuilder, MiniGameCookingBean>
{
    //事件处理
    public EventHandler eventHandler;
    
    /// <summary>
    /// 初始化游戏
    /// </summary>
    /// <param name="miniGameData"></param>
    public override void InitGame(MiniGameCookingBean miniGameData)
    {
        base.InitGame(miniGameData);
        miniGameBuilder.CreateAllCharacter(
            miniGameData.listUserGameData, miniGameData.userStartPosition,
            miniGameData.listEnemyGameData, miniGameData.listEnemyStartPosition,
            miniGameData.listAuditerGameData, miniGameData.listAuditerStartPosition,
            miniGameData.listCompereGameData, miniGameData.listCompereStartPosition);
        //因为剧情需要，先隐藏主持人
        //miniGameBuilder. SetCompereCharacterActive(false);
        //触发开场事件
        if (eventHandler != null)
            eventHandler.EventTriggerForStoryCooking(miniGameData, miniGameData.storyGameOpenId);

    }


}