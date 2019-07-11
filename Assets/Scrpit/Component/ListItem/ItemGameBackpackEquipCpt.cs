using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

public class ItemGameBackpackEquipCpt : ItemGameBackpackCpt
{
    public UIGameEquip uiGameEquip;

    public override void ButtonRightClick()
    {
        if (itemsInfoBean == null)
            return;
        if (selectionBox != null)
            selectionBox.SetCallBack(this);
        switch (itemsInfoBean.items_type)
        {
            case (int)GeneralEnum.Hat:
            case (int)GeneralEnum.Clothes:
            case (int)GeneralEnum.Shoes:
                selectionBox.Open(2);
                break;
            default:
                selectionBox.Open(0);
                break;
        }
    }

    #region  装备回调
    public override void SelectionEquip(ItemsSelectionBox view)
    {
        uiGameEquip.Equip(itemsInfoBean);
        RemoveItems();
    }

    public new void SelectionUnload(ItemsSelectionBox view)
    {

    }
    #endregion 
}