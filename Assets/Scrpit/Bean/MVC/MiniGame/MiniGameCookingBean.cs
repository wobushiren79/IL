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
    public long storyGameStartId;
    //游戏审核故事ID
    public long storyGameAuditId;
    //游戏烹饪主题ID
    public long cookingThemeId;
    //游戏烹饪主题等级
    public int cookingThemeLevel;

    //料理的主题
    protected CookingThemeBean cookingTheme;
    //烹饪按钮个数
    public int cookButtonNumber;

    public MiniGameCookingBean()
    {
        gameType = MiniGameEnum.Cooking;
    }

    public void InitData(
        CharacterBean userData, 
        List<CharacterBean> listEnemyData,
        List<CharacterBean> listAuditerData,
        List<CharacterBean> listCompereData)
    {
        List<CharacterBean> listUserData = new List<CharacterBean>();
        if (userData != null)
            listUserData.Add(userData);
        base.InitData(listUserData, listEnemyData);

        //创建评审角色数据
        if (!CheckUtil.ListIsNull(listAuditerData))
        {
            foreach (CharacterBean itemData in listAuditerData)
            {
                //获取角色属性
                itemData.GetAttributes(
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
                itemData.GetAttributes(
                out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
                MiniGameCharacterBean itemUserGameData = CreateMiniGameCharacterBeanByType();
                itemUserGameData.characterMaxLife = totalAttributes.life;
                itemUserGameData.characterCurrentLife = totalAttributes.life;
                itemUserGameData.characterData = itemData;
                listCompereGameData.Add(itemUserGameData);
            }
        }
    }


    public override void InitForMiniGame()
    {

    }

    /// <summary>
    /// 获取烹饪主题
    /// </summary>
    public CookingThemeBean GetCookingTheme()
    {
        return cookingTheme;
    }

    /// <summary>
    /// 通过ID设置食物主题
    /// </summary>
    /// <param name="innFoodManager"></param>
    /// <param name="themeId"></param>
    public void SetCookingThemeById(InnFoodManager innFoodManager,long themeId)
    {
        cookingTheme =  innFoodManager.GetCookingThemeById(themeId);
    }

    /// <summary>
    /// 通过等级设置食物主题
    /// </summary>
    /// <param name="innFoodManager"></param>
    /// <param name="themeLevel"></param>
    public void SetCookingThemeByLevel(InnFoodManager innFoodManager, int themeLevel)
    {
        List<CookingThemeBean> listData= innFoodManager.GetCookingThemeByLevel(themeLevel);
        cookingTheme = RandomUtil.GetRandomDataByList(listData);
    }


}