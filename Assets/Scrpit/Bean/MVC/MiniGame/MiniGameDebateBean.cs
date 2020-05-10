using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MiniGameDebateBean : MiniGameBaseBean
{
    public enum DebateStatus
    {
        Idle=0,//闲置中
        Combat=1,//战斗中
    }

    public List<ItemMiniGameDebateCardCpt> listUserCard = new List<ItemMiniGameDebateCardCpt>();
    public List<ItemMiniGameDebateCardCpt> listEnemyCard = new List<ItemMiniGameDebateCardCpt>();

    public DebateStatus debateStatus = DebateStatus.Idle;//是否正在战斗

    public MiniGameDebateBean()
    {
        gameType = MiniGameEnum.Debate;
    }

    public override void InitForMiniGame(GameItemsManager gameItemsManager)
    {

    }

    /// <summary>
    /// 设置状态
    /// </summary>
    /// <param name="debateStatus"></param>
    public void SetDebateStatus(DebateStatus debateStatus)
    {
        this.debateStatus = debateStatus;
    }

    /// <summary>
    /// 设置状态
    /// </summary>
    /// <returns></returns>
    public DebateStatus GetDebateStatus()
    {
        return debateStatus; 
    }


}