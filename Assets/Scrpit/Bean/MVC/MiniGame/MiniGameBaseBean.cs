using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MiniGameBaseBean
{
    public MiniGameEnum gameType;//游戏类型
    public MiniGameReasonEnum gameReason;//玩游戏的原因
    //胜利条件
    public float winSurvivalTime;//生存时间(秒)
    public long winLife;//生命值多少以上
    public int winSurvivalNumber;//生存角色个数
    public int winBringDownNumber;//打到角色个数
    public int winScore;//胜利分数
    public int winMoneyS;//胜利的金钱
    public int winMoneyM;//胜利的金钱
    public int winMoneyL;//胜利的金钱

    public long preMoneyL;//前置金钱
    public long preMoneyM;
    public long preMoneyS;
    public int preGameTime;//游戏进行时间

    //游戏结果 0输 1赢
    public int gameResult;
    //结果之后的ID
    public long gameResultWinStoryId;
    public long gameResultLoseStoryId;
    public long gameResultWinTalkMarkId;
    public long gameResultLoseTalkMarkId;

    //游戏地点
    public Vector3 miniGamePosition;

    //奖励
    public List<RewardTypeBean> listReward = new List<RewardTypeBean>();

    //玩家数据
    public List<MiniGameCharacterBean> listUserGameData = new List<MiniGameCharacterBean>();
    //对手数据
    public List<MiniGameCharacterBean> listEnemyGameData = new List<MiniGameCharacterBean>();

    /// <summary>
    /// 获取友方数据
    /// </summary>
    /// <returns></returns>
    public List<MiniGameCharacterBean> GetUserGameData()
    {
        return listUserGameData;
    }

    /// <summary>
    /// 获取敌人数据
    /// </summary>
    /// <returns></returns>
    public List<MiniGameCharacterBean> GetEnemyGameData()
    {
        return listEnemyGameData;
    }

    /// <summary>
    /// 获取所有玩家数据
    /// </summary>
    /// <returns></returns>
    public List<MiniGameCharacterBean> GetPlayerGameData()
    {
        List<MiniGameCharacterBean> listData = new List<MiniGameCharacterBean>();
        listData.AddRange(listUserGameData);
        listData.AddRange(listEnemyGameData);
        return listData;
    }

    /// <summary>
    /// 获取胜利条件列表
    /// </summary>
    /// <returns></returns>
    public List<string> GetListWinConditions()
    {
        List<string> listWinConditions = new List<string>();
        if (winSurvivalTime != 0)
        {
            string winSurvivalTimeStr = string.Format(GameCommonInfo.GetUITextById(211), winSurvivalTime + GameCommonInfo.GetUITextById(39));
            listWinConditions.Add(winSurvivalTimeStr);
        }
        if (winLife != 0)
        {
            string winLifeStr = string.Format(GameCommonInfo.GetUITextById(212), winLife + "");
            listWinConditions.Add(winLifeStr);
        }
        if (winSurvivalNumber != 0)
        {
            string winSurvivalNumberStr = string.Format(GameCommonInfo.GetUITextById(213), winSurvivalNumber + "");
            listWinConditions.Add(winSurvivalNumberStr);
        }
        if (winBringDownNumber != 0)
        {
            string winBringDownNumberStr = string.Format(GameCommonInfo.GetUITextById(214), winBringDownNumber + "");
            listWinConditions.Add(winBringDownNumberStr);
        }
        if (winScore != 0)
        {
            string winScoreStr = string.Format(GameCommonInfo.GetUITextById(215), winScore + "");
            listWinConditions.Add(winScoreStr);
        }
        if (winMoneyL != 0 || winMoneyM != 0 || winMoneyS != 0)
        {
            string moneyStr = "";
            if (winMoneyL != 0)
            {
                moneyStr += winMoneyL + GameCommonInfo.GetUITextById(16);
            }
            if (winMoneyM != 0)
            {
                moneyStr += winMoneyM + GameCommonInfo.GetUITextById(17);
            }
            if (winMoneyS != 0)
            {
                moneyStr += winMoneyS + GameCommonInfo.GetUITextById(18);
            }
            string winMoneyStr = string.Format(GameCommonInfo.GetUITextById(216), moneyStr);
            listWinConditions.Add(winMoneyStr);
        }

        return listWinConditions;
    }

    /// <summary>
    /// 获取游戏名字
    /// </summary>
    /// <returns></returns>
    public string GetGameName()
    {
        string gameName = "???";
        switch (gameType)
        {
            case MiniGameEnum.Cooking:
                gameName = GameCommonInfo.GetUITextById(201);
                break;
            case MiniGameEnum.Barrage:
                gameName = GameCommonInfo.GetUITextById(202);
                break;
            case MiniGameEnum.Account:
                gameName = GameCommonInfo.GetUITextById(203);
                break;
            case MiniGameEnum.Debate:
                gameName = GameCommonInfo.GetUITextById(204);
                break;
            case MiniGameEnum.Combat:
                gameName = GameCommonInfo.GetUITextById(205);
                break;
        }
        return gameName;
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="gameItemsManager"></param>
    /// <param name="userData"></param>
    /// <param name="listEnemyData"></param>
    public virtual void InitData(GameItemsManager gameItemsManager, List<CharacterBean> listUserData, List<CharacterBean> listEnemyData)
    {
        //创建操作角色数据
        if (!CheckUtil.ListIsNull(listUserData))
        {
            foreach (CharacterBean itemData in listUserData)
            {
                //获取角色属性
                itemData.GetAttributes(gameItemsManager,
                out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
                MiniGameCharacterBean itemUserGameData = CreateMiniGameCharacterBeanByType();
                itemUserGameData.characterType = 1;
                itemUserGameData.characterMaxLife = totalAttributes.life;
                itemUserGameData.characterCurrentLife = totalAttributes.life;
                itemUserGameData.characterData = itemData;
                listUserGameData.Add(itemUserGameData);
            }
        }
        //创建敌人角色数据
        if (!CheckUtil.ListIsNull(listEnemyData))
        {
            foreach (CharacterBean itemData in listEnemyData)
            {
                //获取角色属性
                itemData.GetAttributes(gameItemsManager,
                out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
                MiniGameCharacterBean itemEnemyGameData = CreateMiniGameCharacterBeanByType();
                itemEnemyGameData.characterType = 0;
                itemEnemyGameData.characterMaxLife = totalAttributes.life;
                itemEnemyGameData.characterCurrentLife = totalAttributes.life;
                itemEnemyGameData.characterData = itemData;
                listEnemyGameData.Add(itemEnemyGameData);
            }
        }
    }

    /// <summary>
    /// 通过游戏类型获取角色数据类型
    /// </summary>
    /// <returns></returns>
    public MiniGameCharacterBean CreateMiniGameCharacterBeanByType()
    {
        MiniGameCharacterBean itemUserGameData = null;
        switch (gameType)
        {
            case MiniGameEnum.Barrage:
                itemUserGameData = new MiniGameCharacterForBarrageBean();
                break;
            case MiniGameEnum.Combat:
                itemUserGameData = new MiniGameCharacterForCombatBean();
                break;
            case MiniGameEnum.Cooking:
                itemUserGameData = new MiniGameCharacterForCookingBean();
                break;
            case MiniGameEnum.Account:
                itemUserGameData = new MiniGameCharacterForAccountBean();
                break;
            case MiniGameEnum.Debate:
                itemUserGameData = new MiniGameCharacterForDebateBean();
                break;
        }
        return itemUserGameData;
    }

    public virtual void InitData(GameItemsManager gameItemsManager, List<CharacterBean> listUserData)
    {
        InitData(gameItemsManager, listUserData, null);
    }
    public virtual void InitData(GameItemsManager gameItemsManager, CharacterBean userData)
    {
        List<CharacterBean> listCharacterData = new List<CharacterBean>();
        if (userData != null)
            listCharacterData.Add(userData);
        InitData(gameItemsManager, listCharacterData, null);
    }
    public virtual void InitData(GameItemsManager gameItemsManager, CharacterBean userData, List<CharacterBean> listEnemyData)
    {
        List<CharacterBean> listCharacterData = new List<CharacterBean>();
        if (userData != null)
            listCharacterData.Add(userData);
        InitData(gameItemsManager, listCharacterData, listEnemyData);
    }
    public virtual void InitData(GameItemsManager gameItemsManager, CharacterBean userData, CharacterBean enemyData)
    {
        List<CharacterBean> listCharacterData = new List<CharacterBean>();
        if (userData != null)
            listCharacterData.Add(userData);
        List<CharacterBean> listEnemyData = new List<CharacterBean>();
        if (enemyData != null)
            listEnemyData.Add(enemyData);
        InitData(gameItemsManager, listCharacterData, listEnemyData);
    }
}