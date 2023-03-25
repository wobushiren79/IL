using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class BuildBedBean : BaseBean
{
    //床的名字
    public string bedName;

    //床的数据
    public int bedSize = 1;
    public long bedBase = 1010001;
    public long bedBar = 1020001;
    public long bedSheets = 1030001;
    public long bedPillow = 1040001;

    //价格
    public long priceL;
    public long priceM;
    public long priceS;

    //备注ID
    public string remarkId;

    //稀有度
    public int rarity = 0;

    //是否已经设置
    public bool isSet = false;

    public long sellNumber;//销售数量

    public long sellMoneyL;
    public long sellMoneyM;
    public long sellMoneyS;

    //睡觉时间
    public long sellTime;

    //床状态（ResearchStatusEnum）
    public int bedStatus = 0;
    //床等级
    public int bedLevel = 0;
    //床经验
    public long bedExp = 0;
    //研发经验
    public long researchExp = 0;
    //研究人员ID
    public List<string> listResearchCharacterId = new List<string>();


    //获取稀有度
    public RarityEnum GetRarity()
    {
        return (RarityEnum)rarity;
    }

    /// <summary>
    /// 根据幸运值随机生成数据
    /// </summary>
    /// <param name="lucky"></param>
    /// <returns></returns>
    public BuildBedBean RandomDataByLucky(int lucky)
    {
        BuildBedBean buildBed = new BuildBedBean();
        buildBed.bedSize = bedSize;
        buildBed.bedBase = bedBase;
        buildBed.bedBar = bedBar;
        buildBed.bedSheets = bedSheets;
        buildBed.bedPillow = bedPillow;
        buildBed.bedName = bedName;
        float luckRate = (lucky / 100f) * 0.3f + 0.05f;
        float randomRate = UnityEngine.Random.Range(0f, 1f);
        if (randomRate>= luckRate)
        {
            buildBed.rarity = 0;
        }
        else
        {
            buildBed.rarity = 1;
        }
        buildBed.priceS = 100;
        if (buildBed.rarity == 0)
        {
            buildBed.priceS += UnityEngine.Random.Range(0, 50);
        }
        else
        {
            buildBed.priceS += UnityEngine.Random.Range(100, 150);
        }
        buildBed.remarkId = SystemUtil.GetUUID(SystemUtil.UUIDTypeEnum.N);
        return buildBed;
    }


    /// <summary>
    /// 卖出菜品
    /// </summary>
    /// <param name="number"></param>
    public void SellBed(long number,long sleepTime, long priceL, long priceM, long priceS, out bool isLevelUp)
    {
        sellNumber += number;
        sellMoneyL += priceL;
        sellMoneyM += priceM;
        sellMoneyS += priceS;
        sellTime += sleepTime;
        AddLevelExp((int)number, out isLevelUp);
    }

    /// <summary>
    /// 增加经验
    /// </summary>
    /// <param name="exp"></param>
    public void AddLevelExp(int exp, out bool isLevelUp)
    {
        bedExp += exp;
        isLevelUp = false;
        LevelTypeEnum bedLevel = GetBedLevel();
        int levelExp = GetBedLevelUpExp(bedLevel);

        if (levelExp == 0)
        {
            bedExp = 0;
        }
        else if (bedExp >= levelExp)
        {
            bedExp = levelExp;
            if (bedStatus == (int)ResearchStatusEnum.Normal)
            {
                bedStatus = (int)ResearchStatusEnum.WaitForResearch;
                isLevelUp = true;
            }
        }
    }

    public int GetBedLevelUpExp(LevelTypeEnum level)
    {
        int levelExp = 0;
        if (level == LevelTypeEnum.Init)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.BedForLevelUpExp1, out levelExp);
        }
        else if (level == LevelTypeEnum.Star)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.BedForLevelUpExp2, out levelExp);
        }
        else if (level == LevelTypeEnum.Moon)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.BedForLevelUpExp3, out levelExp);
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
        LevelTypeEnum bedLevel = GetBedLevel();
        if (bedLevel == LevelTypeEnum.Init)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.BedForLevelResearchExp1, out researchLevelExp);
        }
        else if (bedLevel == LevelTypeEnum.Star)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.BedForLevelResearchExp2, out researchLevelExp);
        }
        else if (bedLevel == LevelTypeEnum.Moon)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.BedForLevelResearchExp3, out researchLevelExp);
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
    /// 获取价格
    /// </summary>
    /// <param name="menuInfo"></param>
    /// <param name="priceL"></param>
    /// <param name="priceM"></param>
    /// <param name="priceS"></param>
    public void GetPrice(out long outPriceL, out long outPriceM, out long outPriceS)
    {
        float addRate = 1;
        LevelTypeEnum bedLevel = GetBedLevel();
        if (bedLevel == LevelTypeEnum.Init)
        {
            addRate = 1;
        }
        else if (bedLevel == LevelTypeEnum.Star)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.BedForPriceAddRate1, out addRate);
        }
        else if (bedLevel == LevelTypeEnum.Moon)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.BedForPriceAddRate2, out addRate);
        }
        else if (bedLevel == LevelTypeEnum.Sun)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.BedForPriceAddRate3, out addRate);
        }
        outPriceL = (long)(priceL * addRate);
        outPriceM = (long)(priceM * addRate);
        outPriceS = (long)(priceS * addRate);
    }

    public float GetPriceAddRate()
    {
        float addRate = 1;
        LevelTypeEnum bedLevel = GetBedLevel();
        if (bedLevel == LevelTypeEnum.Init)
        {
            addRate = 1;
        }
        else if (bedLevel == LevelTypeEnum.Star)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.BedForPriceAddRate1, out addRate);
        }
        else if (bedLevel == LevelTypeEnum.Moon)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.BedForPriceAddRate2, out addRate);
        }
        else if (bedLevel == LevelTypeEnum.Sun)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.BedForPriceAddRate3, out addRate);
        }
        return addRate;
    }

    /// <summary>
    /// 获取菜品等级
    /// </summary>
    /// <returns></returns>
    public LevelTypeEnum GetBedLevel(out string levelStr, out int nextLevelExp)
    {
        levelStr = "???";
        LevelTypeEnum bedLevel = GetBedLevel();
        levelStr = LevelTypeEnumTools.GetLevelStr(bedLevel);
        nextLevelExp = GetBedLevelUpExp(bedLevel);
        return bedLevel;
    }

    public LevelTypeEnum GetBedLevel()
    {
        return (LevelTypeEnum)bedLevel;
    }


    /// <summary>
    /// 获取研究进度
    /// </summary>
    /// <returns></returns>
    public float GetResearchProgress(out long completeResearchExp, out long researchExp)
    {
        completeResearchExp = 0;
        LevelTypeEnum bedLevel = GetBedLevel();
        if (bedLevel == LevelTypeEnum.Init)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.BedForLevelResearchExp1, out completeResearchExp);
        }
        else if (bedLevel == LevelTypeEnum.Star)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.BedForLevelResearchExp2, out completeResearchExp);
        }
        else if (bedLevel == LevelTypeEnum.Moon)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.BedForLevelResearchExp3, out completeResearchExp);
        }
        researchExp = this.researchExp;
        float progress = (float)researchExp / completeResearchExp;
        return (float)Math.Round(progress, 4);
    }



    /// <summary>
    /// 获取对应等级图标
    /// </summary>
    /// <param name="iconDataManager"></param>
    /// <returns></returns>
    public Sprite GetBedLevelIcon()
    {
        LevelTypeEnum bedLevel = GetBedLevel();
        Sprite spIcon = LevelTypeEnumTools.GetLevelIcon(bedLevel);
        return spIcon;
    }

    /// <summary>
    /// 获取菜单状态
    /// </summary>
    /// <returns></returns>
    public ResearchStatusEnum GetBedStatus()
    {
        return (ResearchStatusEnum)bedStatus;
    }

    /// <summary>
    /// 升级
    /// </summary>
    public void LevelUp()
    {
        bedLevel += 1;
        if (bedLevel > 3)
        {
            bedLevel = 3;
        }
        bedExp = 0;
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
    /// 获取研究价格
    /// </summary>
    /// <param name="researchPriceL"></param>
    /// <param name="researchPriceM"></param>
    /// <param name="researchPriceS"></param>
    public void GetResearchPrice(out long researchPriceL, out long researchPriceM, out long researchPriceS)
    {
        string researchPrice="0,0,0";
        LevelTypeEnum bedLevel = GetBedLevel();
        if (bedLevel == LevelTypeEnum.Init)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.BedForResearchPrice1, out  researchPrice);

        }
        else if (bedLevel == LevelTypeEnum.Star)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.BedForResearchPrice2, out  researchPrice);
        }
        else if (bedLevel == LevelTypeEnum.Moon)
        {
            GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.BedForResearchPrice3, out  researchPrice);
        }
        long[] priceList= researchPrice.SplitForArrayLong(',');
        researchPriceL = priceList[0];
        researchPriceM = priceList[1];
        researchPriceS = priceList[2];
    }

    /// <summary>
    /// 设置研究人员ID
    /// </summary>
    /// <param name="listCharacterData"></param>
    public void StartResearch(List<CharacterBean> listCharacterData)
    {
        if (bedStatus != (int)ResearchStatusEnum.WaitForResearch)
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
        bedStatus = (int)ResearchStatusEnum.Researching;
    }

    /// <summary>
    /// 取消研究
    /// </summary>
    public void CancelResearch(GameDataBean gameData)
    {
        if (bedStatus != (int)ResearchStatusEnum.Researching)
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
        bedStatus = (int)ResearchStatusEnum.WaitForResearch;
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
        bedStatus = (int)ResearchStatusEnum.Normal;
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
        LevelTypeEnum bedLevel = GetBedLevel();
        if (bedLevel == LevelTypeEnum.Init)
        {
            if (levelTitle <= 0)
            {
                failStr = string.Format(TextHandler.Instance.manager.GetTextById(1072), gameData.GetInnAttributesData().GetInnLevelStr(1, 1));
                return false;
            }
        }
        else if (bedLevel == LevelTypeEnum.Star)
        {
            if (levelTitle <= 1)
            {
                failStr = string.Format(TextHandler.Instance.manager.GetTextById(1072), gameData.GetInnAttributesData().GetInnLevelStr(2, 1));
                return false;
            }
        }
        else if (bedLevel == LevelTypeEnum.Moon)
        {
            if (levelTitle <= 2)
            {
                failStr = string.Format(TextHandler.Instance.manager.GetTextById(1072), gameData.GetInnAttributesData().GetInnLevelStr(3, 1));
                return false;
            }
        }
        else if (bedLevel == LevelTypeEnum.Sun)
        {
            failStr = TextHandler.Instance.manager.GetTextById(1073);
            return false;
        }

        //判断是否有足够的研究经费
        GetResearchPrice(out long researchPriceL, out long researchPriceM, out long researchPriceS);
        if (!gameData.HasEnoughMoney(researchPriceL, researchPriceM, researchPriceS))
        {
            failStr = TextHandler.Instance.manager.GetTextById(1005);
            return false;
        }

        return true;
    }
}