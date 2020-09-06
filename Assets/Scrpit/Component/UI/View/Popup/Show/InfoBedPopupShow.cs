using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class InfoBedPopupShow : PopupShowView
{
    public GameObject objLevel;
    public GameObject objLevelProgress;
    public Image ivLevel;
    public Text tvLevelName;
    public ProgressView proLevel;


    public GameObject objResearch;
    public Text tvResearcherName;
    public Text tvResearcherAbility;
    public ProgressView proResearch;

    public GameObject objItemBaseContainer;
    public GameObject objItemStatisticsContainer;
    public GameObject objItemTextModel;

    protected GameDataManager gameDataManager;
    protected IconDataManager iconDataManager;
    protected GameItemsManager gameItemsManager;
    protected InnBuildManager innBuildManager;

    public BuildBedBean buildBedData;

    public override void Awake()
    {
        base.Awake();
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        gameItemsManager = Find<GameItemsManager>(ImportantTypeEnum.GameItemsManager);
        iconDataManager = Find<IconDataManager>(ImportantTypeEnum.UIManager);
        innBuildManager = Find<InnBuildManager>(ImportantTypeEnum.BuildManager);
    }

    public void SetData(BuildBedBean buildBedData)
    {
        this.buildBedData = buildBedData;
        CptUtil.RemoveChildsByActive(objItemBaseContainer);
        CptUtil.RemoveChildsByActive(objItemStatisticsContainer);
        
        CreateStructure(BuildItemTypeEnum.BedBase, buildBedData.bedBase);
        CreateStructure(BuildItemTypeEnum.BedBar, buildBedData.bedBar);
        CreateStructure(BuildItemTypeEnum.BedSheets, buildBedData.bedSheets);
        CreateStructure(BuildItemTypeEnum.BedPillow, buildBedData.bedPillow);
        AddItemForSellNumber(buildBedData.sellNumber);
        AddItemForSellMoney(MoneyEnum.S, buildBedData.sellMoneyS);
        //设置等级相关
        SetLevel(buildBedData);
        //设置研究相关
        SetResearch(buildBedData);

        GameUtil.RefreshRectViewHight((RectTransform)objItemBaseContainer.transform, false);
        GameUtil.RefreshRectViewHight((RectTransform)objItemStatisticsContainer.transform, false);
        GameUtil.RefreshRectViewHight((RectTransform)transform,false);
    }

    /// <summary>
    /// 设置等级
    /// </summary>
    public void SetLevel(BuildBedBean buildBedData)
    {
        LevelTypeEnum level = buildBedData.GetBedLevel(out string levelStr, out int nextLevelExp);
        if ((int)level >= 3)
        {
            objLevelProgress.SetActive(false);
        }
        else
        {
            objLevelProgress.SetActive(true);
        }
        Sprite spIcon = buildBedData.GetBedLevelIcon(iconDataManager);
        if (spIcon == null)
        {
            ivLevel.gameObject.SetActive(false);
        }
        else
        {
            ivLevel.gameObject.SetActive(true);
        }
        ivLevel.sprite = spIcon;
        tvLevelName.text = levelStr;
        proLevel.SetData(nextLevelExp, buildBedData.bedExp);
        if (buildBedData.GetBedStatus() == ResearchStatusEnum.WaitForResearch)
        {
            proLevel.SetContent(GameCommonInfo.GetUITextById(287));
        }
    }

    /// <summary>
    /// 设置研究数据
    /// </summary>
    /// <param name="ownData"></param>
    public void SetResearch(BuildBedBean buildBedData)
    {
        if (buildBedData.GetBedStatus() == ResearchStatusEnum.Normal)
        {
            objResearch.SetActive(false);
            return;
        }
        objResearch.SetActive(true);
        CharacterBean researcher = buildBedData.GetResearchCharacter(gameDataManager.gameData);
        float progress = buildBedData.GetResearchProgress(out long completeExp, out long researchExp);
        if (researcher != null)
        {
            tvResearcherName.text = researcher.baseInfo.name;
            long addExp = researcher.CalculationBedResearchAddExp(gameItemsManager);
            tvResearcherAbility.text = addExp + "/s";
        }
        else
        {
            tvResearcherName.text = "无";
            tvResearcherAbility.text = "0/s";
        }
        //设置进度
        proResearch.SetData(completeExp, researchExp);
    }

    /// <summary>
    /// 增加销售数量
    /// </summary>
    /// <param name="number"></param>
    public void AddItemForSellNumber(long number)
    {
        Sprite spIcon = iconDataManager.GetIconSpriteByName("ui_features_bed");
        CreateItem(objItemStatisticsContainer, spIcon, Color.red, GameCommonInfo.GetUITextById(351), number + "");
    }

    /// <summary>
    /// 增加销售金额
    /// </summary>
    /// <param name="moneyType"></param>
    /// <param name="money"></param>
    public void AddItemForSellMoney(MoneyEnum moneyType, long money)
    {
        string iconKey = "";
        string contentStr = "";
        switch (moneyType)
        {
            case MoneyEnum.L:
                iconKey = "money_3";
                contentStr = GameCommonInfo.GetUITextById(333);
                break;
            case MoneyEnum.M:
                iconKey = "money_2";
                contentStr = GameCommonInfo.GetUITextById(334);
                break;
            case MoneyEnum.S:
                iconKey = "money_1";
                contentStr = GameCommonInfo.GetUITextById(335);
                break;
        }
        Sprite spIcon = iconDataManager.GetIconSpriteByName(iconKey);
        CreateItem(objItemStatisticsContainer, spIcon,Color.white, contentStr, money + "");
    }

    /// <summary>
    /// 创建结构
    /// </summary>
    /// <param name="buildItemType"></param>
    /// <param name="buildId"></param>
    protected void CreateStructure(BuildItemTypeEnum buildItemType,long buildId)
    {
        string name =  BuildItemTypeEnumTools.GetBuildItemName(buildItemType);
        BuildItemBean buildItemdData = innBuildManager.GetBuildDataById(buildId);
        CreateItem(objItemBaseContainer, null, Color.white, name, buildItemdData.name);
    }

    /// <summary>
    /// 创建Item
    /// </summary>
    /// <param name="spIcon"></param>
    /// <param name="name"></param>
    /// <param name="content"></param>
    protected void CreateItem(GameObject objContainer, Sprite spIcon, Color colorIcon, string name, string content)
    {
        GameObject objItem = Instantiate(objContainer, objItemTextModel);
        ItemPopupBedTextCpt itemCpt = objItem.GetComponent<ItemPopupBedTextCpt>();
        itemCpt.SetData(spIcon, colorIcon, name, content);
    }
}