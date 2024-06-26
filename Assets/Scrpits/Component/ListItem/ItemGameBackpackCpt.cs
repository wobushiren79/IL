﻿using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using DG.Tweening;

public class ItemGameBackpackCpt : ItemGameBaseCpt, IPointerClickHandler, ItemsSelectionDialogView.ICallBack, DialogView.IDialogCallBack
{
    public Text tvName;
    public Text tvNumber;
    public RectTransform rtIcon;
    public Image ivIcon;
    public UIPopupItemsButton infoItemsPopup;

    public ItemsInfoBean itemsInfoData;
    public ItemBean itemBean;

    public UnityEvent leftClick;
    public UnityEvent rightClick;

    public bool isOpenClick = true;



    public void Start()
    {
        leftClick.AddListener(new UnityAction(ButtonClick));
        rightClick.AddListener(new UnityAction(ButtonClick));
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="infoBean"></param>
    /// <param name="itemBean"></param>
    public void SetData(ItemsInfoBean infoBean, ItemBean itemBean)
    {
        this.itemsInfoData = infoBean;
        this.itemBean = itemBean;
        if (infoBean != null)
        {
            SetName(infoBean.name);
        }
        else
        {
            SetName("");
        }
        SetIcon(infoBean);
        if (itemBean != null)
        {
            SetNumber(itemBean.itemNumber);
        }
        else
        {
            SetNumber(0);
        }
        infoItemsPopup.SetData(itemsInfoData, ivIcon.sprite);
    }

    /// <summary>
    /// 设置Icon
    /// </summary>
    /// <param name="iconKey"></param>
    /// <param name="itemType"></param>
    public void SetIcon(ItemsInfoBean itemsInfo)
    {
        Vector2 offsetMin = new Vector2(0, 0);
        Vector2 offsetMax = new Vector2(0, 0);
        Sprite spIcon = null;
        if (itemsInfo != null)
        {
            switch (itemsInfo.items_type)
            {
                case (int)GeneralEnum.Hat:
                    offsetMin = new Vector2(-30, -60);
                    offsetMax = new Vector2(30, 0);
                    break;
                case (int)GeneralEnum.Clothes:
                    offsetMin = new Vector2(-30, -20);
                    offsetMax = new Vector2(30, 40);
                    break;
                case (int)GeneralEnum.Shoes:
                    offsetMin = new Vector2(-30, 0);
                    offsetMax = new Vector2(30, 60);
                    break;
                default:
                    break;
            }
            spIcon = GeneralEnumTools.GetGeneralSprite(itemsInfo, false);
        }
        if (spIcon != null)
            ivIcon.color = new Color(1, 1, 1, 1);
        else
            ivIcon.color = new Color(1, 1, 1, 0);
        ivIcon.sprite = spIcon;

        if (rtIcon != null)
        {
            rtIcon.offsetMin = offsetMin;
            rtIcon.offsetMax = offsetMax;
        }
    }

    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        if (tvName != null)
            tvName.text = name;
    }

    /// <summary>
    /// 设置数量
    /// </summary>
    /// <param name="number"></param>
    public void SetNumber(long number)
    {
        if (tvNumber != null)
            tvNumber.text = "x" + number;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            leftClick.Invoke();
        else if (eventData.button == PointerEventData.InputButton.Right)
            rightClick.Invoke();
    }

    public virtual void ButtonClick()
    {
        if (!isOpenClick)
        {
            return;
        }
        if (itemsInfoData == null)
            return;
        DialogBean dialogData = new DialogBean();
        dialogData.dialogType = DialogEnum.ItemsSelection;
        dialogData.callBack = this;
        ItemsSelectionDialogView selectionDialog = UIHandler.Instance.ShowDialog<ItemsSelectionDialogView>(dialogData);
        selectionDialog.SetCallBack(this);
        switch (itemsInfoData.GetItemsType())
        {
            case GeneralEnum.Menu:
                selectionDialog.Open(ItemsSelectionDialogView.SelectionTypeEnum.UseAndDiscard);
                break;
            case GeneralEnum.Read:
                selectionDialog.Open(ItemsSelectionDialogView.SelectionTypeEnum.ReadAndDiscard);
                break;
            default:
                selectionDialog.Open(ItemsSelectionDialogView.SelectionTypeEnum.Discard);
                break;
        }
    }

    #region 选择回调
    /// <summary>
    /// 使用
    /// </summary>
    /// <param name="view"></param>
    public virtual void SelectionUse(ItemsSelectionDialogView view)
    {
        if (itemsInfoData == null || itemBean == null)
            return;
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        switch (itemsInfoData.GetItemsType())
        {
            case GeneralEnum.Menu:
                //添加菜谱
                if (gameData.AddFoodMenu(itemsInfoData.add_id))
                {
                    MenuInfoBean menuInfo = InnFoodHandler.Instance.manager.GetFoodDataById(itemsInfoData.add_id);
                    RefreshItems(itemsInfoData.id, -1);
                    DialogBean dialogData = new DialogBean
                    {
                        dialogType = DialogEnum.Achievement,
                        title = TextHandler.Instance.manager.GetTextById(1047),
                        content = menuInfo.name,
                        callBack = this
                    };
                    AchievementDialogView achievementDialog = UIHandler.Instance.ShowDialog<AchievementDialogView>(dialogData);
                    achievementDialog.SetData(1, menuInfo.icon_key);
                    UIHandler.Instance.ToastHint<ToastView>(ivIcon.sprite, TextHandler.Instance.manager.GetTextById(1006));
                }
                else
                {
                    UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1007));
                };
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="view"></param>
    public virtual void SelectionDiscard(ItemsSelectionDialogView view)
    {
        if (itemsInfoData == null)
            return;
        if (itemBean.itemNumber == 1)
        {
            DialogBean dialogBean = new DialogBean
            {
                dialogType = DialogEnum.Normal,
                content = string.Format(TextHandler.Instance.manager.GetTextById(3001), itemsInfoData.name),
                remark = "1",
                callBack = this
            };
            UIHandler.Instance.ShowDialog<DialogView>(dialogBean);
        }
        else
        {
            DialogBean dialogBean = new DialogBean
            {
                dialogType = DialogEnum.PickForNumber,
                content = string.Format(TextHandler.Instance.manager.GetTextById(3001), itemsInfoData.name),
                callBack = this
            };
            PickForNumberDialogView pickForNumberDialog = UIHandler.Instance.ShowDialog<PickForNumberDialogView>(dialogBean);
            pickForNumberDialog.SetData(ivIcon.sprite, itemBean.itemNumber);
        }

    }

    public virtual void SelectionEquip(ItemsSelectionDialogView view)
    {

    }

    public virtual void SelectionTFEquip(ItemsSelectionDialogView view)
    {

    }

    public virtual void SelectionUnload(ItemsSelectionDialogView view)
    {

    }

    public virtual void SelectionGift(ItemsSelectionDialogView view)
    {

    }

    public virtual void SelectionRead(ItemsSelectionDialogView view)
    {
        GameEventHandler.Instance.EventTriggerForLook(itemsInfoData.add_id);
    }

    #endregion

    #region 删除确认回调
    public void Submit(DialogView dialogView, DialogBean dialogData)
    {
        if (dialogView as PickForNumberDialogView)
        {
            PickForNumberDialogView pickForNumberDialog = dialogView as PickForNumberDialogView;
            long pickNumber = pickForNumberDialog.GetPickNumber();

            //创建确认弹窗
            DialogBean dialogBean = new DialogBean
            {
                dialogType = DialogEnum.Normal,
                content = string.Format(TextHandler.Instance.manager.GetTextById(3001), itemsInfoData.name + "x" + pickNumber),
                remark = "" + pickNumber,
                callBack = this
            };
            UIHandler.Instance.ShowDialog<DialogView>(dialogBean);
        }
        else if (dialogView as AchievementDialogView)
        {

        }
        else
        {
            RefreshItems(itemsInfoData.id, -long.Parse(dialogData.remark));
        }
    }

    public void Cancel(DialogView dialogView, DialogBean dialogData)
    {

    }
    #endregion

    /// <summary>
    /// 删除物品
    /// </summary>
    public void RefreshItems(long id, long changeNumber)
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        gameData.AddItemsNumber(id, changeNumber);
        uiComponent.RefreshUI();
    }


}