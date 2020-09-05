using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class BedShowView : BaseMonoBehaviour
{
    public Image ivBedBase;
    public Image ivBedBar;
    public Image ivBedSheets;
    public Image ivBedPillow;

    protected InnBuildManager innBuildManager;

    public void Awake()
    {
        innBuildManager = Find<InnBuildManager>(ImportantTypeEnum.BuildManager);
    }

    public  void SetData(BuildBedBean buildBedData)
    {
        BuildItemBean bedBaseData =  innBuildManager.GetBuildDataById(buildBedData.bedBase);
        SetBaseData(bedBaseData);
        BuildItemBean bedBarData = innBuildManager.GetBuildDataById(buildBedData.bedBar);
        SetBarData(bedBarData);
        BuildItemBean bedSheetsData = innBuildManager.GetBuildDataById(buildBedData.bedSheets);
        SetSheetsData(bedSheetsData);
        BuildItemBean bedPillowData = innBuildManager.GetBuildDataById(buildBedData.bedPillow);
        SetPillowData(bedPillowData);
    }

    public void SetBaseData(BuildItemBean bedBaseData)
    {
        if (ivBedBase == null)
            return;
        ivBedBase.sprite = innBuildManager.GetFurnitureSpriteByName(bedBaseData.icon_key);
    }

    public void SetBarData(BuildItemBean bedBarData)
    {
        if (ivBedBar == null)
            return;
        ivBedBar.sprite = innBuildManager.GetFurnitureSpriteByName(bedBarData.icon_key);
    }

    public void SetSheetsData(BuildItemBean bedSheetsData)
    {
        if (ivBedSheets == null)
            return;
        ivBedSheets.sprite = innBuildManager.GetFurnitureSpriteByName(bedSheetsData.icon_key);
    }

    public void SetPillowData(BuildItemBean bedPillowData)
    {
        if (ivBedPillow == null)
            return;
        ivBedPillow.sprite = innBuildManager.GetFurnitureSpriteByName(bedPillowData.icon_key);
    }
}