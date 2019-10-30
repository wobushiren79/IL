using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MiniGameCookingBean : MiniGameBaseBean
{
    //评审员
    public List<MiniGameCharacterBean> listAuditerGameData = new List<MiniGameCharacterBean>();
    //主持人
    public List<MiniGameCharacterBean> listCompereGameData = new List<MiniGameCharacterBean>();

    //所有玩家起始位置
    public Vector3 userStartPosition = Vector3.zero;
    //所有敌人起始位置
    public List<Vector3> listEnemyStartPosition = new List<Vector3>();
    //所有评审起始位置
    public List<Vector3> listAuditerStartPosition = new List<Vector3>();
    //所有主持人起始位置
    public List<Vector3> listCompereStartPosition = new List<Vector3>();

    //游戏开幕故事ID
    public long storyGameOpenId;
    //游戏审核故事ID
    public long storyGameAuditId;

    //料理的主题
    public CookingThemeBean cookingTheme;

    public MiniGameCookingBean()
    {
        gameType = MiniGameEnum.Cooking;
    }

    public void InitData(GameItemsManager gameItemsManager, 
        List<CharacterBean> listUserData, 
        List<CharacterBean> listEnemyData,
        List<CharacterBean> listAuditerData,
        List<CharacterBean> listCompereData)
    {
        base.InitData(gameItemsManager, listUserData, listEnemyData);

        //创建评审角色数据
        if (!CheckUtil.ListIsNull(listAuditerData))
        {
            foreach (CharacterBean itemData in listAuditerData)
            {
                //获取角色属性
                itemData.GetAttributes(gameItemsManager,
                out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
                MiniGameCharacterBean itemUserGameData = CreateMiniGameCharacterBeanByType();
                itemUserGameData.characterMaxLife = totalAttributes.life;
                itemUserGameData.characterCurrentLife = totalAttributes.life;
                itemUserGameData.characterData = itemData;
                listAuditerGameData.Add(itemUserGameData);
            }
        }

        //创建主持人角色数据
        if (!CheckUtil.ListIsNull(listCompereData))
        {
            foreach (CharacterBean itemData in listCompereData)
            {
                //获取角色属性
                itemData.GetAttributes(gameItemsManager,
                out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
                MiniGameCharacterBean itemUserGameData = CreateMiniGameCharacterBeanByType();
                itemUserGameData.characterMaxLife = totalAttributes.life;
                itemUserGameData.characterCurrentLife = totalAttributes.life;
                itemUserGameData.characterData = itemData;
                listCompereGameData.Add(itemUserGameData);
            }
        }
    }
}