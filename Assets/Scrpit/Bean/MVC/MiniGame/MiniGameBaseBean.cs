using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MiniGameBaseBean
{
    public MiniGameEnum gameType;//游戏类型
    public MiniGameReasonEnum gameReason;//玩游戏的原因

    public int gameLevel;//游戏等级
    //胜利条件
    public float winSurvivalTime;//生存时间(秒)
    public float winLife;//生命值多少以上
    public int winSurvivalNumber;//生存角色个数
    public int winBringDownNumber;//打到角色个数
    public int winScore;//胜利分数

    //游戏结果 0输 1赢
    public int gameResult;

    //奖励道具
    public Dictionary<long, int> listRewardItem = new Dictionary<long, int>();

    //玩家数据
    public List<MiniGameCharacterBean> listUserGameData = new List<MiniGameCharacterBean>();
    //对手数据
    public List<MiniGameCharacterBean> listEnemyGameData = new List<MiniGameCharacterBean>();

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
            string winBringDownNumberStr = string.Format(GameCommonInfo.GetUITextById(214), winBringDownNumber+ "");
            listWinConditions.Add(winBringDownNumberStr);
        }
        if (winScore != 0)
        {
            string winScoreStr = string.Format(GameCommonInfo.GetUITextById(215), winScore + "");
            listWinConditions.Add(winScoreStr);
        }
        return listWinConditions;
    }

    /// <summary>
    /// 添加奖励物品
    /// </summary>
    /// <param name="id"></param>
    /// <param name="number"></param>
    public void AddRewardItem(long id, int number)
    {
        if (listRewardItem == null)
            listRewardItem = new Dictionary<long, int>();
        if (listRewardItem.ContainsKey(id))
        {
            listRewardItem[id] += number;
        }
        else
        {
            listRewardItem.Add(id, number);
        }
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
            //case MiniGameEnum.Barrage:
            //    break;
            //case MiniGameEnum.Barrage:
            //    break;
        }
        return itemUserGameData;
    }

    public virtual void InitData(GameItemsManager gameItemsManager, CharacterBean userData)
    {
        List<CharacterBean> listCharacterData = new List<CharacterBean>();
        listCharacterData.Add(userData);
        InitData(gameItemsManager, listCharacterData, null);
    }

    public virtual void InitData(GameItemsManager gameItemsManager, CharacterBean userData, List<CharacterBean> listEnemyData)
    {
        List<CharacterBean> listCharacterData = new List<CharacterBean>();
        listCharacterData.Add(userData);
        InitData(gameItemsManager, listCharacterData, listEnemyData);
    }

    public virtual void InitData(GameItemsManager gameItemsManager, CharacterBean userData, CharacterBean enemyData)
    {
        List<CharacterBean> listCharacterData = new List<CharacterBean>();
        listCharacterData.Add(userData);
        List<CharacterBean> listEnemyData = new List<CharacterBean>();
        listEnemyData.Add(enemyData);
        InitData(gameItemsManager, listCharacterData, listEnemyData);
    }
}