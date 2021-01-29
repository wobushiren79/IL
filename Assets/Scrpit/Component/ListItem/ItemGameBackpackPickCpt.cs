using UnityEngine;
using UnityEditor;

public class ItemGameBackpackPickCpt : ItemGameBackpackCpt
{
    private ICallBack mCallBack;
    protected ItemsSelectionDialogView.SelectionTypeEnum selectionType;
    public void SetCallBack(ICallBack callBack)
    {
        mCallBack = callBack;
    }

    public void SetSelectionType(ItemsSelectionDialogView.SelectionTypeEnum selectionType)
    {
        this.selectionType = selectionType;
    }

    public override void ButtonClick()
    {
        if (itemsInfoData == null)
            return;
        DialogBean dialogData = new DialogBean();
        ItemsSelectionDialogView itemsSelectionDialog = DialogHandler.Instance.CreateDialog<ItemsSelectionDialogView>(DialogEnum.ItemsSelection, null, dialogData);
        itemsSelectionDialog.SetCallBack(this);
        itemsSelectionDialog.Open(selectionType);
    }

    public override void SelectionGift(ItemsSelectionDialogView view)
    {
        if (mCallBack != null)
        {
            mCallBack.ItemsSelection(itemsInfoData, itemBean);
        }
    }
    public override void SelectionUse(ItemsSelectionDialogView view)
    {
        if (mCallBack != null)
        {
            mCallBack.ItemsSelection(itemsInfoData, itemBean);
        }
    }
    public interface ICallBack
    {
        void ItemsSelection(ItemsInfoBean itemsInfo, ItemBean itemBean);
    }


}