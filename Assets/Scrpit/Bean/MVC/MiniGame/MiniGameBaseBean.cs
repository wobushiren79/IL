using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public abstract class MiniGameBaseBean
{
    public MiniGameEnum gameType;//游戏类型
    public MiniGameReasonEnum gameReason;//玩游戏的原因
    public MiniGameResultEnum gameResult;//游戏结果

    //胜利条件
    public float winSurvivalTime;//生存时间(秒)
    public long winLife;//生命值多少以上
    public int winSurvivalNumber;//生存角色个数
    public int winBringDownNumber;//打到角色个数
    public int winScore;//胜利分数
    public int winMoneyS;//胜利的金钱
    public int winMoneyM;//胜利的金钱
    public int winMoneyL;//胜利的金钱
    public int winRank;//胜利排名

    public long preMoneyL;//前置金钱
    public long preMoneyM;
    public long preMoneyS;
    public int preGameTime;//游戏进行时间


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
    /// 设置游戏结果
    /// </summary>
    /// <param name="miniGameResult"></param>
    public void SetGameResult(MiniGameResultEnum miniGameResult)
    {
        this.gameResult = miniGameResult;
    }

    /// <summary>
    /// 获取游戏结果
    /// </summary>
    /// <returns></returns>
    public MiniGameResultEnum GetGameResult()
    {
        return gameResult;
    }

    /// <summary>
    /// 获取友方数据
    /// </summary>
    /// <returns></returns>
    public virtual List<MiniGameCharacterBean> GetListUserGameData()
    {
        return listUserGameData;
    }
    public List<CharacterBean> GetListUserCharacterData()
    {
        List<CharacterBean> listCharacterData = new List<CharacterBean>();
        foreach (MiniGameCharacterBean itemGameData in listUserGameData)
        {
            listCharacterData.Add(itemGameData.characterData);
        }
        return listCharacterData;
    }

    public virtual MiniGameCharacterBean GetUserGameData()
    {
        if (CheckUtil.ListIsNull(listUserGameData))
        {
            return null;
        }
        else
        {
            return listUserGameData[0];
        }
    }

    /// <summary>
    /// 获取敌人数据
    /// </summary>
    /// <returns></returns>
    public virtual List<MiniGameCharacterBean> GetListEnemyGameData()
    {
        return listEnemyGameData;
    }

    public virtual MiniGameCharacterBean GetEnemyGameData()
    {
        if (CheckUtil.ListIsNull(listEnemyGameData))
        {
            return null;
        }
        else
        {
            return listEnemyGameData[0];
        }
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
    public virtual List<string> GetListWinConditions()
    {
        List<string> listWinConditions = new List<string>();
        switch (gameType)
        {
            case MiniGameEnum.Cooking:
                GetListWinConditionsForWinScore(listWinConditions);
                GetListWinConditionsForWinRank(listWinConditions);
                break;
            case MiniGameEnum.Barrage:
                GetListWinConditionsForWinSurvivalTime(listWinConditions);
                GetListWinConditionsForWinLife(listWinConditions);
                break;
            case MiniGameEnum.Account:
                GetListWinConditionsForWinSurvivalTime(listWinConditions);
                GetListWinConditionsForWinMoney(listWinConditions);
                break;
            case MiniGameEnum.Debate:
                GetListWinConditionsForWinLife(listWinConditions);
                break;
            case MiniGameEnum.Combat:
                GetListWinConditionsForWinSurvivalNumber(listWinConditions);
                GetListWinConditionsForWinBringDownNumber(listWinConditions);
                break;
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
        InitForMiniGame(gameItemsManager);
    }

    /// <summary>
    /// 初始化对应的游戏数据 
    /// </summary>
    public abstract void InitForMiniGame(GameItemsManager gameItemsManager);

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


    #region 胜利列表
    protected void GetListWinConditionsForWinSurvivalTime(List<string> listData)
    {
        if (winSurvivalTime != 0)
        {
            string data = string.Format(GameCommonInfo.GetUITextById(211), winSurvivalTime + GameCommonInfo.GetUITextById(39));
            listData.Add(data);
        }
    }
    protected void GetListWinConditionsForWinLife(List<string> listData)
    {
        if (winLife != 0)
        {
            string data = string.Format(GameCommonInfo.GetUITextById(212), winLife + "");
            listData.Add(data);
        }
    }
    protected void GetListWinConditionsForWinSurvivalNumber(List<string> listData)
    {
        if (winSurvivalNumber != 0)
        {
            string data = string.Format(GameCommonInfo.GetUITextById(213), winSurvivalNumber + "");
            listData.Add(data);
        }
    }
    protected void GetListWinConditionsForWinBringDownNumber(List<string> listData)
    {
        if (winBringDownNumber != 0)
        {
            string winBringDownNumberStr = string.Format(GameCommonInfo.GetUITextById(214), winBringDownNumber + "");
            listData.Add(winBringDownNumberStr);
        }
    }
    protected void GetListWinConditionsForWinScore(List<string> listData)
    {
        if (winScore != 0)
        {
            string data = string.Format(GameCommonInfo.GetUITextById(215), winScore + "");
            listData.Add(data);
        }
    }
    protected void GetListWinConditionsForWinRank(List<string> listData)
    {
        if (winRank != 0)
        {
            string data = string.Format(GameCommonInfo.GetUITextById(217), winRank + "");
            listData.Add(data);
        }
    }

    protected void GetListWinConditionsForWinMoney(List<string> listData)
    {
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
            listData.Add(winMoneyStr);
        }
    }
    #endregion
}