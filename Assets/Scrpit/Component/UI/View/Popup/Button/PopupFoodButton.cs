using UnityEngine;
using UnityEditor;

public class PopupFoodButton : PopupButtonView<PopupFoodShow>
{
    public MenuOwnBean ownData;
    public MenuInfoBean foodData;
    public bool isShowTime = true;


    public void SetData(MenuOwnBean ownData, MenuInfoBean foodData)
    {
        SetData(ownData, foodData, true);
    }

    public void SetData(MenuOwnBean ownData, MenuInfoBean foodData, bool isShowTime)
    {
        this.ownData = ownData;
        this.foodData = foodData;
        this.isShowTime = isShowTime;
    }

    public override void ClosePopup()
    {

    }

    public override void OpenPopup()
    {
        popupShow.SetData(ownData, foodData, isShowTime);
    }
}