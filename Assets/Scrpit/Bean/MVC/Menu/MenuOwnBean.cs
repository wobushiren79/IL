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
    /// 卖出菜品
    /// </summary>
    /// <param name="number"></param>
    public void SellMenu(long number,long priceL, long priceM, long priceS)
    {
        sellNumber += number;
        sellMoneyL += priceL;
        sellMoneyM += priceM;
        sellMoneyS += priceS;
        AddLevelExp((int)number);
    }

    /// <summary>
    /// 增加经验
    /// </summary>
    /// <param name="exp"></param>
    public void AddLevelExp(int exp)
    {
        menuExp += exp;
        if (menuLevel == 0)
        {
            if (menuExp >= 100)
            {
                menuExp = 100;
                menuStatus =(int)MenuStatusEnum.WaitForResearch;
            }
        }
        else if (menuLevel == 1)
        {
            if (menuExp >= 1000)
            {
                menuExp = 1000;
                menuStatus = (int)MenuStatusEnum.WaitForResearch;
            }
        }
        else if (menuLevel == 2)
        {
            if (menuExp >= 10000)
            {
                menuExp = 10000;
                menuStatus = (int)MenuStatusEnum.WaitForResearch;
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
        if (menuLevel == 0)
        {
            if (researchExp >= 1000)
            {
                researchExp = 1000;
                return true;
            }
        }
        else if (menuLevel == 1)
        {
            if (researchExp >= 10000)
            {
                researchExp = 10000;
                return true;
            }
        }
        else if (menuLevel == 2)
        {
            if (researchExp >= 100000)
            {
                researchExp = 100000;
                return true;
            }
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
            levelStr = "";
            nextLevelExp = 100;
        }
        else if (menuLevel == 1)
        {
            levelStr = GameCommonInfo.GetUITextById(101);
            nextLevelExp = 1000;
        }
        else if (menuLevel == 2)
        {
            levelStr = GameCommonInfo.GetUITextById(102);
            nextLevelExp = 10000;
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

    /// <summary>
    /// 获取研究进度
    /// </summary>
    /// <returns></returns>
    public float GetResearchProgress(out long completeResearchExp)
    {
         completeResearchExp = 0;
        if (menuLevel == 0)
        {
            completeResearchExp = 1000;
        }
        else if (menuLevel == 1)
        {
            completeResearchExp = 10000;
        }
        else if (menuLevel == 2)
        {
            completeResearchExp = 100000;
        }
        float progress = (float)researchExp / completeResearchExp;
        return (float)Math.Round(progress,4);
    }

    /// <summary>
    /// 获取对应等级图标
    /// </summary>
    /// <param name="iconDataManager"></param>
    /// <returns></returns>
    public Sprite GetMenuLevelIcon(IconDataManager iconDataManager)
    {
        Sprite spIcon=null;
        if (menuLevel == 0)
        {
        }
        else if (menuLevel == 1)
        {
            spIcon= iconDataManager.GetIconSpriteByName("reputation_level_1_1");
        }
        else if (menuLevel == 2)
        {
            spIcon = iconDataManager.GetIconSpriteByName("reputation_level_2_1");
        }
        else if (menuLevel == 3)
        {
            spIcon = iconDataManager.GetIconSpriteByName("reputation_level_1_1");
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
}