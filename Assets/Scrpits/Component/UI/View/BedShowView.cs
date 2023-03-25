using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class BedShowView : BaseMonoBehaviour
{
    public Image ivBedBase;
    public Image ivBedBar;
    public Image ivBedSheets;
    public Image ivBedPillow;

    public  void SetData(BuildBedBean buildBedData)
    {
        BuildItemBean bedBaseData =  InnBuildHandler.Instance.manager.GetBuildDataById(buildBedData.bedBase);
        SetBaseData(bedBaseData);
        BuildItemBean bedBarData = InnBuildHandler.Instance.manager.GetBuildDataById(buildBedData.bedBar);
        SetBarData(bedBarData);
        BuildItemBean bedSheetsData = InnBuildHandler.Instance.manager.GetBuildDataById(buildBedData.bedSheets);
        SetSheetsData(bedSheetsData);
        BuildItemBean bedPillowData = InnBuildHandler.Instance.manager.GetBuildDataById(buildBedData.bedPillow);
        SetPillowData(bedPillowData);
    }

    public void SetBaseData(BuildItemBean bedBaseData)
    {
        if (ivBedBase == null)
            return;
        ivBedBase.sprite = InnBuildHandler.Instance.manager.GetFurnitureSpriteByName(bedBaseData.icon_key);
    }

    public void SetBarData(BuildItemBean bedBarData)
    {
        if (ivBedBar == null)
            return;
        ivBedBar.sprite = InnBuildHandler.Instance.manager.GetFurnitureSpriteByName(bedBarData.icon_key);
    }

    public void SetSheetsData(BuildItemBean bedSheetsData)
    {
        if (ivBedSheets == null)
            return;
        ivBedSheets.sprite = InnBuildHandler.Instance.manager.GetFurnitureSpriteByName(bedSheetsData.icon_key);
    }

    public void SetPillowData(BuildItemBean bedPillowData)
    {
        if (ivBedPillow == null)
            return;
        ivBedPillow.sprite = InnBuildHandler.Instance.manager.GetFurnitureSpriteByName(bedPillowData.icon_key);
    }
}