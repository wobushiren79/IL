using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

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
        StartGame();
    }

    public override void StartGame()
    {
        base.StartGame();
        UIMiniGameDebate uiMiniGameDebate = (UIMiniGameDebate)uiGameManager.GetOpenUI();
        List<ItemMiniGameDebateCardCpt.DebateCardTypeEnun> listUser = new List<ItemMiniGameDebateCardCpt.DebateCardTypeEnun>();
        listUser.Add(ItemMiniGameDebateCardCpt.DebateCardTypeEnun.Rock);
        listUser.Add(ItemMiniGameDebateCardCpt.DebateCardTypeEnun.Paper);
        listUser.Add(ItemMiniGameDebateCardCpt.DebateCardTypeEnun.Scissors);
        listUser.Add(ItemMiniGameDebateCardCpt.DebateCardTypeEnun.Paper);
        listUser.Add(ItemMiniGameDebateCardCpt.DebateCardTypeEnun.Paper);
        List<ItemMiniGameDebateCardCpt.DebateCardTypeEnun> listEnemy = new List<ItemMiniGameDebateCardCpt.DebateCardTypeEnun>();
        listEnemy.Add(ItemMiniGameDebateCardCpt.DebateCardTypeEnun.Paper);
        listEnemy.Add(ItemMiniGameDebateCardCpt.DebateCardTypeEnun.Paper);
        listEnemy.Add(ItemMiniGameDebateCardCpt.DebateCardTypeEnun.Paper);
        listEnemy.Add(ItemMiniGameDebateCardCpt.DebateCardTypeEnun.Paper);
        listEnemy.Add(ItemMiniGameDebateCardCpt.DebateCardTypeEnun.Paper);
        uiMiniGameDebate.CreateCardItemList(listUser, listEnemy);
    }


}