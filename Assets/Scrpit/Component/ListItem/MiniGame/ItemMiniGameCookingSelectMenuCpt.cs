using UnityEngine;
using UnityEditor;

public class ItemMiniGameCookingSelectMenuCpt : ItemGameBaseCpt 
{
    public InfoFoodPopupButton infoFoodPopup;

    private void Start()
    {
        InfoFoodPopupShow infoFoodPopupShow =   GetUIManager<UIGameManager>().infoFoodPopup;
        infoFoodPopup.SetPopupShowView(infoFoodPopupShow);
    }

    public void SetData(MenuOwnBean menuOwn,MenuInfoBean menuInfo)
    {
        infoFoodPopup.SetData(menuOwn, menuInfo);
    }
}