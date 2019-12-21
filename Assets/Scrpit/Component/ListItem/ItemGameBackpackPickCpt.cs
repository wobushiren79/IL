using UnityEngine;
using UnityEditor;

public class ItemGameBackpackPickCpt : ItemGameBackpackCpt
{
    private ICallBack mCallBack;

    public void SetCallBack(ICallBack callBack)
    {
        mCallBack = callBack;
    }

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
        if (mCallBack!=null)
        {
            mCallBack.ItemsSelectionForGift(itemsInfoBean,itemBean);
        }
    }

    public interface ICallBack
    {
        void ItemsSelectionForGift(ItemsInfoBean itemsInfo,ItemBean itemBean);
    }

}