using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class MenuOwnBean
{
    public long menuId;
    public bool isSell = true;//是否开放售卖
    public long sellNumber;//销售数量

    public long sellMoneyL;
    public long sellMoneyM;
    public long sellMoneyS;
    //菜品状态（ResearchStatusEnum）
    public int menuStatus = 0;
    //菜品等级
    public int menuLevel = 0;
    //菜品经验
    public long menuExp = 0;
    //研发经验
    public long researchExp = 0;
    //研究人员ID
    public List<string> listResearchCharacterId = new List<string>();

    public MenuOwnBean()
    {

    }

    public MenuOwnBean(long menuId)
    {
        this.menuId = menuId;
    }

    /// <summary>
    /// 获取价格
    /// </summary>
    /// <param name="menuInfo"></param>
    /// <param name="priceL"></param>
    /// <param name="priceM"></param>
    /// <param name="priceS"></param>
    public void GetPrice(MenuInfoBean menuInfo, out long priceL, out long priceM, out long priceS)
    {
        float addRate = 1;
        LevelTypeEnum menuLevel = GetMenuLevel();
        if (menuLevel == LevelTypeEnum.Init)
        {
            addRate = 1;
        }
        else if (menuLevel == LevelTypeEnum.Star)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.MenuForPriceAddRate1, out addRate);
        }
        else if (menuLevel == LevelTypeEnum.Moon)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.MenuForPriceAddRate2, out addRate);
        }
        else if (menuLevel == LevelTypeEnum.Sun)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.MenuForPriceAddRate3, out addRate);
        }
        priceL = (long)(menuInfo.price_l * addRate);
        priceM = (long)(menuInfo.price_m * addRate);
        priceS = (long)(menuInfo.price_s * addRate);
    }

    /// <summary>
    /// 卖出菜品
    /// </summary>
    /// <param name="number"></param>
    public void SellMenu(InnFoodManager innFoodManager, long number, long priceL, long priceM, long priceS, out bool isLevelUp)
    {
        sellNumber += number;
        sellMoneyL += priceL;
        sellMoneyM += priceM;
        sellMoneyS += priceS;
        AddLevelExp(innFoodManager,(int)number, out isLevelUp);
    }

    /// <summary>
    /// 增加经验
    /// </summary>
    /// <param name="exp"></param>
    public void AddLevelExp(InnFoodManager innFoodManager, int exp, out bool isLevelUp)
    {
        menuExp += exp;
        isLevelUp = false;
        LevelTypeEnum menuLevel = GetMenuLevel();
        int levelExp =  GetMenuLevelUpExp(innFoodManager, menuLevel);

        if (levelExp == 0)
        {
            menuExp = 0;
        }
        else if (menuExp >= levelExp)
        {
            menuExp = levelExp;
            if (menuStatus == (int)ResearchStatusEnum.Normal)
            {
                menuStatus = (int)ResearchStatusEnum.WaitForResearch;
                isLevelUp = true;
            }
        }
    }

    public int GetMenuLevelUpExp(InnFoodManager innFoodManager, LevelTypeEnum menuLevel)
    {
        int levelExp = 0;
        if (menuLevel == LevelTypeEnum.Init)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.MenuForLevelUpExp1, out levelExp);
        }
        else if (menuLevel == LevelTypeEnum.Star)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.MenuForLevelUpExp2, out levelExp);
        }
        else if (menuLevel == LevelTypeEnum.Moon)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.MenuForLevelUpExp3, out levelExp);
        }
        MenuInfoBean menuInfo = innFoodManager.GetFoodDataById(menuId);
        //不同稀有度经验不同
        if (menuInfo.rarity == 0)
        {
            levelExp = levelExp * 1;
        }
        else if (menuInfo.rarity == 1)
        {
            levelExp = levelExp * 3;
        }
        else if (menuInfo.rarity == 2)
        {
            levelExp = levelExp * 5;
        }
        return levelExp;
    }

    /// <summary>
    /// 增加研究经验
    /// </summary>
    /// <param name="exp"></param>
    public bool AddResearchExp(int exp)
    {
        researchExp += exp;
        int researchLevelExp = 0;
        LevelTypeEnum menuLevel = GetMenuLevel();
        if (menuLevel == LevelTypeEnum.Init)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.MenuForLevelResearchExp1, out researchLevelExp);
        }
        else if (menuLevel == LevelTypeEnum.Star)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.MenuForLevelResearchExp2, out researchLevelExp);
        }
        else if (menuLevel == LevelTypeEnum.Moon)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.MenuForLevelResearchExp3, out researchLevelExp);
        }

        if (researchLevelExp == 0)
        {
            researchExp = 0;
        }
        else if (researchExp >= researchLevelExp)
        {
            researchExp = researchLevelExp;
            return true;
        }
        return false;
    }



    /// <summary>
    /// 获取菜品等级
    /// </summary>
    /// <returns></returns>
    public LevelTypeEnum GetMenuLevel(InnFoodManager innFoodManager, out string levelStr, out int nextLevelExp)
    {
        levelStr = "???";
        LevelTypeEnum menuLevel = GetMenuLevel();
        levelStr = LevelTypeEnumTools.GetLevelStr(menuLevel);
        nextLevelExp =  GetMenuLevelUpExp(innFoodManager, menuLevel);
        return menuLevel;
    }

    public LevelTypeEnum GetMenuLevel()
    {
        return (LevelTypeEnum)menuLevel;
    }


    /// <summary>
    /// 获取研究进度
    /// </summary>
    /// <returns></returns>
    public float GetResearchProgress(out long completeResearchExp,out long researchExp)
    {
        completeResearchExp = 0;
        LevelTypeEnum menuLevel = GetMenuLevel();
        if (menuLevel == LevelTypeEnum.Init)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.MenuForLevelResearchExp1, out completeResearchExp);
        }
        else if (menuLevel == LevelTypeEnum.Star)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.MenuForLevelResearchExp2, out completeResearchExp);
        }
        else if (menuLevel == LevelTypeEnum.Moon)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.MenuForLevelResearchExp3, out completeResearchExp);
        }
        researchExp = this.researchExp;
        float progress = (float)researchExp / completeResearchExp;
        return (float)Math.Round(progress, 4);
    }

    /// <summary>
    /// 获取研究材料消耗
    /// </summary>
    /// <param name="menuInfo"></param>
    /// <returns></returns>
    public SortedList<IngredientsEnum, long> GetResearchIngredients(MenuInfoBean menuInfo)
    {
        SortedList<IngredientsEnum, long> listIng = new SortedList<IngredientsEnum, long>();
        float addRate = 1;
        LevelTypeEnum menuLevel = GetMenuLevel();
        if (menuLevel == LevelTypeEnum.Init)
        {
            addRate = 10;
        }
        else if (menuLevel == LevelTypeEnum.Star)
        {
            addRate = 100;
        }
        else if (menuLevel == LevelTypeEnum.Moon)
        {
            addRate = 1000;
        }
        if (menuInfo.ing_oilsalt != 0)
            listIng.Add(IngredientsEnum.Oilsalt, (long)(menuInfo.ing_oilsalt * addRate));
        if (menuInfo.ing_meat != 0)
            listIng.Add(IngredientsEnum.Meat, (long)(menuInfo.ing_meat * addRate));
        if (menuInfo.ing_riverfresh != 0)
            listIng.Add(IngredientsEnum.Riverfresh, (long)(menuInfo.ing_riverfresh * addRate));
        if (menuInfo.ing_seafood != 0)
            listIng.Add(IngredientsEnum.Seafood, (long)(menuInfo.ing_seafood * addRate));
        if (menuInfo.ing_vegetables != 0)
            listIng.Add(IngredientsEnum.Vegetables, (long)(menuInfo.ing_vegetables * addRate));
        if (menuInfo.ing_melonfruit != 0)
            listIng.Add(IngredientsEnum.Melonfruit, (long)(menuInfo.ing_melonfruit * addRate));
        if (menuInfo.ing_waterwine != 0)
            listIng.Add(IngredientsEnum.Waterwine, (long)(menuInfo.ing_waterwine * addRate));
        if (menuInfo.ing_flour != 0)
            listIng.Add(IngredientsEnum.Flour, (long)(menuInfo.ing_flour * addRate));
        return listIng;
    }


    /// <summary>
    /// 获取对应等级图标
    /// </summary>
    /// <param name="iconDataManager"></param>
    /// <returns></returns>
    public Sprite GetMenuLevelIcon(IconDataManager iconDataManager)
    {
        Sprite spIcon = null;
        LevelTypeEnum menuLevel = GetMenuLevel();
        if (menuLevel == LevelTypeEnum.Init)
        {
        }
        else if (menuLevel == LevelTypeEnum.Star)
        {
            spIcon = iconDataManager.GetIconSpriteByName("reputation_level_1_1");
        }
        else if (menuLevel == LevelTypeEnum.Moon)
        {
            spIcon = iconDataManager.GetIconSpriteByName("reputation_level_2_1");
        }
        else if (menuLevel == LevelTypeEnum.Sun)
        {
            spIcon = iconDataManager.GetIconSpriteByName("reputation_level_3_1");
        }
        return spIcon;
    }

    /// <summary>
    /// 获取菜单状态
    /// </summary>
    /// <returns></returns>
    public ResearchStatusEnum GetMenuStatus()
    {
        return (ResearchStatusEnum)menuStatus;
    }

    /// <summary>
    /// 升级
    /// </summary>
    public void LevelUp()
    {
        menuLevel += 1;
        if (menuLevel > 3)
        {
            menuLevel = 3;
        }
        menuExp = 0;
    }

    /// <summary>
    /// 获取研究人员
    /// </summary>
    /// <param name="gameData"></param>
    /// <returns></returns>
    public CharacterBean GetResearchCharacter(GameDataBean gameData)
    {
        List<CharacterBean> listCharacter = gameData.GetAllCharacterData();
        foreach (CharacterBean itemCharacterData in listCharacter)
        {
            foreach (string researcherId in listResearchCharacterId)
            {
                if (itemCharacterData.baseInfo.characterId.Equals(researcherId))
                {
                    return itemCharacterData;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// 设置研究人员ID
    /// </summary>
    /// <param name="listCharacterData"></param>
    public void StartResearch(List<CharacterBean> listCharacterData)
    {
        if (menuStatus != (int)ResearchStatusEnum.WaitForResearch)
            return;
        if (listCharacterData == null)
            return;
        if (listResearchCharacterId == null)
        {
            listResearchCharacterId = new List<string>();
        }
        listResearchCharacterId.Clear();
        List<string> listCharacterId = new List<string>();
        foreach (CharacterBean itemData in listCharacterData)
        {
            itemData.baseInfo.SetWorkerStatus(WorkerStatusEnum.Research);
            listCharacterId.Add(itemData.baseInfo.characterId);
        }
        listResearchCharacterId.AddRange(listCharacterId);
        menuStatus = (int)ResearchStatusEnum.Researching;
    }

    /// <summary>
    /// 取消研究
    /// </summary>
    public void CancelResearch(GameDataBean gameData)
    {
        if (menuStatus != (int)ResearchStatusEnum.Researching)
            return;
        List<CharacterBean> listCharacter = gameData.GetAllCharacterData();
        foreach (string characterId in listResearchCharacterId)
        {
            foreach (CharacterBean itemCharacterData in listCharacter)
            {
                if (itemCharacterData.baseInfo.characterId.Equals(characterId))
                {
                    itemCharacterData.baseInfo.SetWorkerStatus(WorkerStatusEnum.Rest);
                }
            }
        }
        listResearchCharacterId.Clear();
        menuStatus = (int)ResearchStatusEnum.WaitForResearch;
    }

    /// <summary>
    /// 完成研究
    /// </summary>
    /// <param name="gameData"></param>
    public void CompleteResearch(GameDataBean gameData)
    {
        List<CharacterBean> listCharacter = gameData.GetAllCharacterData();
        foreach (string characterId in listResearchCharacterId)
        {
            foreach (CharacterBean itemCharacterData in listCharacter)
            {
                if (itemCharacterData.baseInfo.characterId.Equals(characterId))
                {
                    itemCharacterData.baseInfo.SetWorkerStatus(WorkerStatusEnum.Rest);
                }
            }
        }
        listResearchCharacterId.Clear();
        menuStatus = (int)ResearchStatusEnum.Normal;
        researchExp = 0;
        LevelUp();
    }


    /// <summary>
    /// 检测是否能研究
    /// </summary>
    /// <returns></returns>
    public bool CheckCanResearch(GameDataBean gameData, out string failStr)
    {
        failStr = "";
        string levelStr = gameData.GetInnAttributesData().GetInnLevel(out int levelTitle, out int levelStart);
        LevelTypeEnum menuLevel = GetMenuLevel();
        if (menuLevel ==  LevelTypeEnum.Init)
        {
            if (levelTitle <= 0)
            {
                failStr = string.Format(GameCommonInfo.GetUITextById(1072), gameData.GetInnAttributesData().GetInnLevelStr(1, 1));
                return false;
            }
        }
        else if (menuLevel == LevelTypeEnum.Star)
        {
            if (levelTitle <= 1)
            {
                failStr = string.Format(GameCommonInfo.GetUITextById(1072), gameData.GetInnAttributesData().GetInnLevelStr(2, 1));
                return false;
            }
        }
        else if (menuLevel == LevelTypeEnum.Moon)
        {
            if (levelTitle <= 2)
            {
                failStr = string.Format(GameCommonInfo.GetUITextById(1072), gameData.GetInnAttributesData().GetInnLevelStr(3, 1));
                return false;
            }
        }
        else if (menuLevel == LevelTypeEnum.Sun)
        {
            failStr = GameCommonInfo.GetUITextById(1073);
            return false;
        }
        return true;
    }

}