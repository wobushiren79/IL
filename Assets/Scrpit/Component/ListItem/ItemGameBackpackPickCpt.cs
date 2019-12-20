using UnityEngine;
using UnityEditor;

public class ItemGameBackpackPickCpt : ItemGameBackpackCpt
{
    public override void ButtonClick()
    {
        if (itemsInfoBean == null)
            return;
        PopupItemsSelection popupItemsSelection = uiGameManager.popupItemsSelection;
        if (popupItemsSelection != null)
            popupItemsSelection.SetCallBack(this);
        popupItemsSelection.Open(PopupItemsSelection.SelectionTypeEnum.Gift);
    }


    public override void SelectionGift(PopupItemsSelection view)
    {

    }
}