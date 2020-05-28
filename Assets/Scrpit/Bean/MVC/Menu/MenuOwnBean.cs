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
    //菜品状态（MenuStatusEnum）
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
        if (menuLevel == 0)
        {
            addRate = 1;
        }
        else if (menuLevel == 1)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.MenuForPriceAddRate1, out addRate);
        }
        else if (menuLevel == 2)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.MenuForPriceAddRate2, out addRate);
        }
        else if (menuLevel == 3)
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
    public void SellMenu(long number, long priceL, long priceM, long priceS,out bool isLevelUp)
    {
        sellNumber += number;
        sellMoneyL += priceL;
        sellMoneyM += priceM;
        sellMoneyS += priceS;
        AddLevelExp((int)number,out isLevelUp);
    }

    /// <summary>
    /// 增加经验
    /// </summary>
    /// <param name="exp"></param>
    public void AddLevelExp(int exp, out bool isLevelUp)
    {
        menuExp += exp;
        isLevelUp = false;
        int levelExp = 0;
        if (menuLevel == 0)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.MenuForLevelUpExp1, out levelExp);
        }
        else if (menuLevel == 1)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.MenuForLevelUpExp2, out levelExp);
        }
        else if (menuLevel == 2)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.MenuForLevelUpExp3, out levelExp);
        }

        if (levelExp == 0)
        {
            menuExp = 0;
        }
        else if (menuExp >= levelExp)
        {
            menuExp = levelExp;
            if (menuStatus == (int)MenuStatusEnum.Normal)
            {
                menuStatus = (int)MenuStatusEnum.WaitForResearch;
                isLevelUp = true;
            }
        }
    }

    /// <summary>
    /// 增加研究经验
    /// </summary>
    /// <param name="exp"></param>
    public bool AddResearchExp(int exp)
    {
        researchExp += exp;
        int researchLevelExp = 0;
        if (menuLevel == 0)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.MenuForLevelResearchExp1, out researchLevelExp);
        }
        else if (menuLevel == 1)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.MenuForLevelResearchExp2, out researchLevelExp);
        }
        else if (menuLevel == 2)
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
    public int GetMenuLevel(out string levelStr, out int nextLevelExp)
    {
        levelStr = "???";
        nextLevelExp = 0;
        if (menuLevel == 0)
        {
            levelStr = GameCommonInfo.GetUITextById(104);
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.MenuForLevelUpExp1, out nextLevelExp);
        }
        else if (menuLevel == 1)
        {
            levelStr = GameCommonInfo.GetUITextById(101);
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.MenuForLevelUpExp2, out nextLevelExp);
        }
        else if (menuLevel == 2)
        {
            levelStr = GameCommonInfo.GetUITextById(102);
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.MenuForLevelUpExp3, out nextLevelExp);
        }
        else if (menuLevel == 3)
        {
            levelStr = GameCommonInfo.GetUITextById(103);
        }
        else
        {

        }
        return menuLevel;
    }
    public int GetMenuLevel()
    {
        return menuLevel;
    }


    /// <summary>
    /// 获取研究进度
    /// </summary>
    /// <returns></returns>
    public float GetResearchProgress(out long completeResearchExp)
    {
        completeResearchExp = 0;
        if (menuLevel == 0)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.MenuForLevelResearchExp1, out completeResearchExp);
        }
        else if (menuLevel == 1)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.MenuForLevelResearchExp2, out completeResearchExp);
        }
        else if (menuLevel == 2)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.MenuForLevelResearchExp3, out completeResearchExp);
        }

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
        if (menuLevel == 0)
        {
            addRate = 10;
        }
        else if (menuLevel == 1)
        {
            addRate = 100;
        }
        else if (menuLevel == 2)
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
        if (menuLevel == 0)
        {
        }
        else if (menuLevel == 1)
        {
            spIcon = iconDataManager.GetIconSpriteByName("reputation_level_1_1");
        }
        else if (menuLevel == 2)
        {
            spIcon = iconDataManager.GetIconSpriteByName("reputation_level_2_1");
        }
        else if (menuLevel == 3)
        {
            spIcon = iconDataManager.GetIconSpriteByName("reputation_level_3_1");
        }
        return spIcon;
    }

    /// <summary>
    /// 获取菜单状态
    /// </summary>
    /// <returns></returns>
    public MenuStatusEnum GetMenuStatus()
    {
        return (MenuStatusEnum)menuStatus;
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
        if (menuStatus != (int)MenuStatusEnum.WaitForResearch)
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
        menuStatus = (int)MenuStatusEnum.Researching;
    }

    /// <summary>
    /// 取消研究
    /// </summary>
    public void CancelResearch(GameDataBean gameData)
    {
        if (menuStatus != (int)MenuStatusEnum.Researching)
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
        menuStatus = (int)MenuStatusEnum.WaitForResearch;
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
        menuStatus = (int)MenuStatusEnum.Normal;
        researchExp = 0;
        LevelUp();
    }


    /// <summary>
    /// 检测是否能研究
    /// </summary>
    /// <returns></returns>
    public bool CheckCanResearch(GameDataBean gameData,out string failStr)
    {
        failStr = "";
        string levelStr=  gameData.GetInnAttributesData().GetInnLevel(out int levelTitle,out int levelStart);
        if (menuLevel == 0)
        {
            if (levelTitle <= 0)
            {
                failStr = string.Format(GameCommonInfo.GetUITextById(1072), levelStr);
                return false;
            }
        }
        else if (menuLevel == 1)
        {
            if (levelTitle <= 1)
            {
                failStr = string.Format(GameCommonInfo.GetUITextById(1072), levelStr);
                return false;
            }
        }
        else if (menuLevel == 2)
        {
            if (levelTitle <= 2)
            {
                failStr = string.Format(GameCommonInfo.GetUITextById(1072), levelStr);
                return false;
            }
        }
        else if (menuLevel == 3)
        {
            failStr = GameCommonInfo.GetUITextById(1073);
            return false;
        }
        return true;
    }
}