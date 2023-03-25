using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class PopupBedShow : PopupShowView
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

    public BuildBedBean buildBedData;


    public void SetData(BuildBedBean buildBedData)
    {
        this.buildBedData = buildBedData;
        CptUtil.RemoveChildsByActive(objItemBaseContainer);
        CptUtil.RemoveChildsByActive(objItemStatisticsContainer);
        
        CreateStructure(BuildItemTypeEnum.BedBase, buildBedData.bedBase);
        CreateStructure(BuildItemTypeEnum.BedBar, buildBedData.bedBar);
        CreateStructure(BuildItemTypeEnum.BedSheets, buildBedData.bedSheets);
        CreateStructure(BuildItemTypeEnum.BedPillow, buildBedData.bedPillow);
        AddItemForSellTime(buildBedData.sellTime);
        AddItemForSellNumber(buildBedData.sellNumber);
        AddItemForSellMoney(MoneyEnum.S, buildBedData.sellMoneyS);
        //设置等级相关
        SetLevel(buildBedData);
        //设置研究相关
        SetResearch(buildBedData);

        GameUtil.RefreshRectTransform((RectTransform)objItemBaseContainer.transform);
        GameUtil.RefreshRectTransform((RectTransform)objItemStatisticsContainer.transform);
        GameUtil.RefreshRectTransform((RectTransform)transform);
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
        Sprite spIcon = buildBedData.GetBedLevelIcon();
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
            proLevel.SetContent(TextHandler.Instance.manager.GetTextById(287));
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
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        CharacterBean researcher = buildBedData.GetResearchCharacter(gameData);
        float progress = buildBedData.GetResearchProgress(out long completeExp, out long researchExp);
        if (researcher != null)
        {
            tvResearcherName.text = researcher.baseInfo.name;
            long addExp = researcher.CalculationBedResearchAddExp();
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
    /// 增加使用时间
    /// </summary>
    /// <param name="number"></param>
    public void AddItemForSellTime(long time)
    {
        Sprite spIcon = IconDataHandler.Instance.manager.GetIconSpriteByName("time_wait_1_0");
        CreateItem(objItemStatisticsContainer, spIcon, Color.white, TextHandler.Instance.manager.GetTextById(352), time + " h");
    }

    /// <summary>
    /// 增加销售数量
    /// </summary>
    /// <param name="number"></param>
    public void AddItemForSellNumber(long number)
    {
        Sprite spIcon = IconDataHandler.Instance.manager.GetIconSpriteByName("ui_features_bed");
        CreateItem(objItemStatisticsContainer, spIcon, Color.red, TextHandler.Instance.manager.GetTextById(351), number + "");
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
                contentStr = TextHandler.Instance.manager.GetTextById(333);
                break;
            case MoneyEnum.M:
                iconKey = "money_2";
                contentStr = TextHandler.Instance.manager.GetTextById(334);
                break;
            case MoneyEnum.S:
                iconKey = "money_1";
                contentStr = TextHandler.Instance.manager.GetTextById(335);
                break;
        }
        Sprite spIcon = IconDataHandler.Instance.manager.GetIconSpriteByName(iconKey);
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
        BuildItemBean buildItemdData = InnBuildHandler.Instance.manager.GetBuildDataById(buildId);
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