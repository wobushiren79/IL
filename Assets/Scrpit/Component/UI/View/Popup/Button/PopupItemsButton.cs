using UnityEngine;
using UnityEditor;

public class PopupItemsButton : PopupButtonView<PopupItemsShow>
{
    public ItemsInfoBean itemsInfo;
    public Sprite spIcon;


    public void SetData(ItemsInfoBean itemsInfo,Sprite spIcon)
    {
        this.itemsInfo  = itemsInfo;
        this.spIcon = spIcon;
    }

    public override void ClosePopup()
    {

    }

    public override void OpenPopup()
    {
        if (popupShow == null)
            return;
        popupShow.SetData(spIcon, itemsInfo);
        if (itemsInfo == null || itemsInfo.id == 0)
        {
            if (popupShow != null)
                popupShow.gameObject.SetActive(false);
        }
    }
}