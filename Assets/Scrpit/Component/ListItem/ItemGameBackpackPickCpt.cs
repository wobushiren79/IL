using UnityEngine;
using UnityEditor;

public class ItemGameBackpackPickCpt : ItemGameBackpackCpt
{
    private ICallBack mCallBack;
    protected PopupItemsSelection.SelectionTypeEnum selectionType;
    public void SetCallBack(ICallBack callBack)
    {
        mCallBack = callBack;
    }
    
    public void SetSelectionType(PopupItemsSelection.SelectionTypeEnum  selectionType)
    {
        this.selectionType = selectionType;
    }

    public override void ButtonClick()
    {
        if (itemsInfoData == null)
            return;
        PopupItemsSelection popupItemsSelection = uiGameManager.popupItemsSelection;
        if (popupItemsSelection != null)
            popupItemsSelection.SetCallBack(this);
        popupItemsSelection.Open(selectionType);
    }

    public override void SelectionGift(PopupItemsSelection view)
    {
        if (mCallBack!=null)
        {
            mCallBack.ItemsSelection(itemsInfoData, itemBean);
        }
    }
    public override void SelectionUse(PopupItemsSelection view)
    {
        if (mCallBack != null)
        {
            mCallBack.ItemsSelection(itemsInfoData, itemBean);
        }
    }
    public interface ICallBack
    {
        void ItemsSelection(ItemsInfoBean itemsInfo,ItemBean itemBean);
    }


}