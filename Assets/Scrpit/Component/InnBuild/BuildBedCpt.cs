using UnityEngine;
using UnityEditor;

public class BuildBedCpt : BaseBuildItemCpt
{
    public enum BedStatusEnum
    {
        Idle = 0,//空闲
        Ready = 1,//有人,等待移动到座位
        Use=2,//使用中
        WaitClean = 3,//等待清理
        Cleaning = 4,//清理
    }

    public GameObject objBed;
    public GameObject objSleep;
    public SpriteRenderer srBase;
    public SpriteRenderer srBar;
    public SpriteRenderer srSheets;
    public SpriteRenderer srPillow;

    //作用范围
    public SpriteRenderer addRange;

    public GameObject objLeftPosition;
    public GameObject objUpPosition;
    public GameObject objRightPosition;
    public GameObject objDownPosition;

    public BuildBedBean buildBedData;

    public BedStatusEnum bedStatus = BedStatusEnum.Idle;

    public float addAesthetics = 0;
    public float subAesthetics = 0;

    protected InnBuildManager innBuildManager;
    protected GameDataManager gameDataManager;
    private void Awake()
    {
        innBuildManager = Find<InnBuildManager>(ImportantTypeEnum.BuildManager);
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
    }

    public void SetAddAesthetics(float addAesthetics, float subAesthetics)
    {
        this.addAesthetics = addAesthetics;
        this.subAesthetics = subAesthetics;
    }

    public void GetAesthetics(out float addAesthetics, out float subAesthetics,out float totalAesthetics)
    {
        addAesthetics = this.addAesthetics;
        subAesthetics = this.subAesthetics;

        InnAttributesBean innAttributes= gameDataManager.gameData.GetInnAttributesData();
        int maxAesthetics = innAttributes.CalculationBedMaxAesthetics();
        if (addAesthetics > maxAesthetics)
        {
            addAesthetics = maxAesthetics;
        }

        totalAesthetics = addAesthetics + subAesthetics;

        if (totalAesthetics < 0)
        {
            totalAesthetics = 0;
        }

    }

    public void SetData(BuildItemBean buildItemData, BuildBedBean buildBedData)
    {
        this.buildBedData = buildBedData;
        base.SetData(buildItemData);
    }


    public void SetBedStatus(BedStatusEnum bedStatus)
    {
        this.bedStatus = bedStatus;

        //设置床单的样式
        BuildItemBean buildItemData = innBuildManager.GetBuildDataById(buildBedData.bedSheets);
        if (bedStatus== BedStatusEnum.Idle)
        {
            string cleanStr = buildItemData.GetIconList()[0] + "_clean";
            string iconKey = GetIconKey(cleanStr);
            srSheets.sprite = innBuildManager.GetFurnitureSpriteByName(iconKey);
        }
        else if (bedStatus == BedStatusEnum.WaitClean)
        {
            string noCleanStr = buildItemData.GetIconList()[0] + "_noclean";
            string iconKey = GetIconKey(noCleanStr);
            srSheets.sprite = innBuildManager.GetFurnitureSpriteByName(iconKey);
        }

    }
    public BedStatusEnum GetBedStatus()
    {
        return bedStatus;
    }

    public override void SetDirection(Direction2DEnum direction)
    {
        base.SetDirection(direction);
        switch (direction)
        {
            case Direction2DEnum.Left:
                objBed.transform.position = objLeftPosition.transform.position;
                break;
            case Direction2DEnum.Right:
                objBed.transform.position = objRightPosition.transform.position;
                break;
            case Direction2DEnum.Down:
                objBed.transform.position = objDownPosition.transform.position;
                break;
            case Direction2DEnum.UP:
                objBed.transform.position = objUpPosition.transform.position;
                break;
        }
        if (buildBedData==null)
        {
            return;
        }
        SetBase(buildBedData.bedBase);
        SetBar(buildBedData.bedBar);
        SetSheets(buildBedData.bedSheets);
        SetPillow(buildBedData.bedPillow);
    }

    public void SetBase(long baseId)
    {
        BuildItemBean buildItemData = innBuildManager.GetBuildDataById(baseId);
        string iconKey = GetIconKey(buildItemData.GetIconList()[0]);
        srBase.sprite = innBuildManager.GetFurnitureSpriteByName(iconKey);
    }

    public void SetBar(long barId)
    {
        BuildItemBean buildItemData = innBuildManager.GetBuildDataById(barId);
        string iconKey = GetIconKey(buildItemData.GetIconList()[0]);
        srBar.sprite = innBuildManager.GetFurnitureSpriteByName(iconKey);
    }

    public void SetSheets(long sheetsId)
    {
        BuildItemBean buildItemData = innBuildManager.GetBuildDataById(sheetsId);
        string cleanStr = buildItemData.GetIconList()[0] + "_clean";
        string noCleanStr = buildItemData.GetIconList()[0] + "_noclean";
        string iconKey = GetIconKey(cleanStr);
        srSheets.sprite = innBuildManager.GetFurnitureSpriteByName(iconKey);
    }

    public void SetPillow(long pillowId)
    {
        BuildItemBean buildItemData = innBuildManager.GetBuildDataById(pillowId);
        string iconKey = GetIconKey(buildItemData.GetIconList()[0]);
        srPillow.sprite = innBuildManager.GetFurnitureSpriteByName(iconKey);
    }
    
    /// <summary>
    /// 获取睡觉的位置
    /// </summary>
    /// <returns></returns>
    public Vector3 GetSleepPosition()
    {
        return objSleep.transform.position;
    }

    /// <summary>
    /// 获取价格
    /// </summary>
    /// <param name="basePriceS"></param>
    /// <param name="addPriceS"></param>
    public void GetPrice(out long basePriceS,out long addPriceS)
    {
        basePriceS = 0;
        addPriceS = 0;
        buildBedData.GetPrice(out long priceL, out long priceM, out long priceS);
        basePriceS = priceS;

        GetAesthetics(out float addAesthetics, out float subAesthetics, out float totalAesthetics);
        addPriceS = (long)(totalAesthetics * 20 * buildBedData.GetPriceAddRate());
    }

    /// <summary>
    /// 清理床
    /// </summary>
    public void CleanBed()
    {
        SetBedStatus(BedStatusEnum.Idle);
    }

    protected string GetIconKey(string iconKeyTitle)
    {
        string iconKey = iconKeyTitle;
        switch (direction)
        {
            case Direction2DEnum.Left:
                iconKey += "_0";
                break;
            case Direction2DEnum.Right:
                iconKey += "_1";
                break;
            case Direction2DEnum.Down:
                iconKey += "_2";
                break;
            case Direction2DEnum.UP:
                iconKey += "_3";
                break;
        }
        return iconKey;
    }

    public void ShowRange(bool isShow)
    {
        if (isShow)
        {
            addRange.gameObject.SetActive(true);
        }
        else
        {
            addRange.gameObject.SetActive(false);
        }    
    }

}