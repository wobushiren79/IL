using UnityEngine;
using UnityEditor;

public class InfoFoodPopupButton : PopupButtonView
{
    public MenuOwnBean ownData;
    public MenuInfoBean foodDta;

    public void SetData(MenuOwnBean ownData, MenuInfoBean foodDta)
    {
        this.ownData = ownData;
        this.foodDta = foodDta;
    }

    public override void ClosePopup()
    {

    }

    public override void OpenPopup()
    {
        ((InfoFoodPopupShow)popupShow).SetData(ownData,foodDta);
    }
}