using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;

public class ItemMiniGameCookingSelectMenuCpt : ItemGameBaseCpt, DialogView.IDialogCallBack
{
    public UIPopupFoodButton infoFoodPopup;

    public Text tvName;
    public Image ivIcon;
    public Button btSubmit;

    public MenuInfoBean menuInfo;

    private void Start()
    {
        if (btSubmit != null)
            btSubmit.onClick.AddListener(SelectMenu);
    }

    public void SetData(MenuOwnBean menuOwn, MenuInfoBean menuInfo)
    {
        this.menuInfo = menuInfo;
        infoFoodPopup.SetData(menuOwn, menuInfo);
        SetName(menuInfo.name);
        SetIcon(menuInfo.icon_key);
    }

    public void SetName(string name)
    {
        if (tvName != null)
        {
            tvName.text = name;
        }
    }

    public void SetIcon(string iconKey)
    {
        Sprite spFood = InnFoodHandler.Instance.manager.GetFoodSpriteByName(iconKey);
        if (ivIcon != null)
        {
            ivIcon.sprite = spFood;
        }
    }

    private void SelectMenu()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        DialogBean dialogData = new DialogBean();
        dialogData.content = string.Format(TextHandler.Instance.manager.GetTextById(3051), menuInfo.name);
        dialogData.dialogType = DialogEnum.Normal;
        dialogData.callBack = this;
        UIHandler.Instance.ShowDialog<DialogView>(dialogData);
    }

    #region 确认选择回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        UIMiniGameCookingSelect uiMiniGameCooking = GetUIComponent<UIMiniGameCookingSelect>();
        uiMiniGameCooking.SelectMenu(menuInfo);
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {
    }
    #endregion
}