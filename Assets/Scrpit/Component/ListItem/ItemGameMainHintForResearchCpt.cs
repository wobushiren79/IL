using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ItemGameMainHintForResearchCpt : ItemGameBaseCpt
{
    public MenuOwnBean menuOwn;
    public BuildBedBean bedData;

    public Image ivIcon;
    public Text tvName;
    public ProgressView progressView;

    protected InnFoodManager foodManager;
    protected IconDataManager iconDataManager;


    public void SetData(BuildBedBean bedData)
    {
        this.bedData = bedData;
        iconDataManager = Find<IconDataManager>(ImportantTypeEnum.UIManager);
        Sprite spBedIcon = iconDataManager.GetIconSpriteByName("ui_features_bed");
        SetIcon(spBedIcon);
        SetName(bedData.bedName);
        RefreshData();
    }

    public void SetData(MenuOwnBean menuOwn)
    {
        this.menuOwn = menuOwn;
        foodManager = Find<InnFoodManager>(ImportantTypeEnum.FoodManager);
        MenuInfoBean menuInfo =  foodManager.GetFoodDataById(menuOwn.menuId);
        Sprite spFoodIcon= foodManager.GetFoodSpriteByName(menuInfo.icon_key);
        SetIcon(spFoodIcon);
        SetName(menuInfo.name);
        RefreshData();
    }

    public void RefreshData()
    {
        if (menuOwn != null)
        {
            menuOwn.GetResearchProgress(out long completeResearchExp, out long researchExp);
            SetProgress(researchExp, completeResearchExp);
        }
        if (bedData!=null)
        {
            bedData.GetResearchProgress(out long completeResearchExp, out long researchExp);
            SetProgress(researchExp, completeResearchExp);
        }
    }

    public void SetIcon(Sprite spFoodIcon)
    {
        if (ivIcon != null)
            ivIcon.sprite = spFoodIcon;
    }

    public void SetName(string name)
    {
        if (tvName != null)
            tvName.text = name;
    }

    public void SetProgress(long researchExp, long completeResearchExp)
    {
        progressView.SetData(completeResearchExp , researchExp);
    }
}