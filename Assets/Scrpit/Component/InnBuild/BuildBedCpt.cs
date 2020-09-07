using UnityEngine;
using UnityEditor;

public class BuildBedCpt : BaseBuildItemCpt
{
    public GameObject objBed;
    public SpriteRenderer srBase;
    public SpriteRenderer srBar;
    public SpriteRenderer srSheets;
    public SpriteRenderer srPillow;

    public GameObject objLeftPosition;
    public GameObject objUpPosition;
    public GameObject objRightPosition;
    public GameObject objDownPosition;

    public BuildBedBean buildBedData;

    protected InnBuildManager innBuildManager;

    private void Awake()
    {
        innBuildManager = Find<InnBuildManager>(ImportantTypeEnum.BuildManager);
    }


    public void SetData(BuildItemBean buildItemData, BuildBedBean buildBedData)
    {
        this.buildBedData = buildBedData;
        base.SetData(buildItemData);
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
}