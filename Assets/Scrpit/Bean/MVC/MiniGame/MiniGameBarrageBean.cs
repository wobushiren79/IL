using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MiniGameBarrageBean : MiniGameBaseBean
{
    // public int gamePlayerNumber;//游戏人数
    public float currentTime;//当前时间

    public MiniGameCharacterBean userGameData = new MiniGameCharacterBean();
    public List<MiniGameCharacterBean> listEnemyGameData=new List<MiniGameCharacterBean>();
    
    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="gameItemsManager"></param>
    /// <param name="userData"></param>
    /// <param name="listEnemyData"></param>
    public void InitData(GameItemsManager gameItemsManager, CharacterBean userData, List<CharacterBean> listEnemyData)
    {
        //创建操作角色数据
        if (userData != null)
        {
            //获取角色属性
            userData.GetAttributes(gameItemsManager,
                out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
            userGameData = new MiniGameCharacterBean
            {
                characterType = 0,
                characterMaxLife = totalAttributes.life,
                characterCurrentLife = totalAttributes.life
            };
        }
        //创建敌人角色数据
        if (!CheckUtil.ListIsNull(listEnemyData))
        {
            foreach (CharacterBean itemData in listEnemyData)
            {
                //获取角色属性
                userData.GetAttributes(gameItemsManager,
                    out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
                MiniGameCharacterBean itemUserGameData = new MiniGameCharacterBean
                {
                    characterType = 0,
                    characterMaxLife = totalAttributes.life,
                    characterCurrentLife = totalAttributes.life
                };
                listEnemyGameData.Add(itemUserGameData);
            }
        }
        if (winSurvivalTime != 0)
        {
            currentTime = winSurvivalTime;
        }
    }

    public void InitData(GameItemsManager gameItemsManager, CharacterBean userData)
    {
        InitData(gameItemsManager, userData, null);
    }
}