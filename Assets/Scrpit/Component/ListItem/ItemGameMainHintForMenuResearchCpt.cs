using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ItemGameMainHintForMenuResearchCpt : ItemGameBaseCpt
{
    public MenuOwnBean menuOwn;

    public Image ivIcon;
    public ProgressView progressView;


    protected InnFoodManager foodManager;


    public void SetData(MenuOwnBean menuOwn)
    {
        this.menuOwn = menuOwn;
        foodManager = Find<InnFoodManager>(ImportantTypeEnum.FoodManager);
        MenuInfoBean menuInfo =   foodManager.GetFoodDataById(menuOwn.menuId);
        Sprite spFoodIcon= foodManager.GetFoodSpriteByName(menuInfo.icon_key);
        SetIcon(spFoodIcon);

        RefreshData();
    }

    public void RefreshData()
    {
        float pro = menuOwn.GetResearchProgress(out long completeResearchExp, out long researchExp);
        SetProgress(researchExp, completeResearchExp);
    }

    public void SetIcon(Sprite spFoodIcon)
    {
        if (ivIcon != null)
            ivIcon.sprite = spFoodIcon;
    }

    public void SetProgress(long researchExp, long completeResearchExp)
    {
        progressView.SetData(completeResearchExp , researchExp);
    }
}