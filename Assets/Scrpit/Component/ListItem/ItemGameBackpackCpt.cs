﻿using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using DG.Tweening;

public class ItemGameBackpackCpt : ItemGameBaseCpt, IPointerClickHandler, ItemsSelectionBox.ICallBack, DialogView.IDialogCallBack
{
    public Text tvName;
    public RectTransform rtIcon;
    public Image ivIcon;
    public InfoItemsPopupButton infoItemsPopup;

    public ItemsInfoBean itemsInfoBean;
    public ItemBean itemBean;

    public UnityEvent leftClick;
    public UnityEvent rightClick;

    public void Start()
    {
        leftClick.AddListener(new UnityAction(ButtonClick));
        rightClick.AddListener(new UnityAction(ButtonClick));
        if (infoItemsPopup != null)
        {
            infoItemsPopup.SetPopupShowView(GetUIManager<UIGameManager>().infoItemsPopup);
            infoItemsPopup.SetData(itemsInfoBean, ivIcon.sprite);
        }
    }

    public void SetData(ItemsInfoBean infoBean, ItemBean itemBean)
    {
        this.itemsInfoBean = infoBean;
        this.itemBean = itemBean;
        if (infoBean != null)
        {
            SetIcon(infoBean.icon_key, infoBean.items_type);
            SetName(infoBean.name);
        }
        if (infoItemsPopup != null)
        {
            infoItemsPopup.SetData(itemsInfoBean, ivIcon.sprite);
        }
    }

    /// <summary>
    /// 设置Icon
    /// </summary>
    /// <param name="iconKey"></param>
    /// <param name="itemType"></param>
    public void SetIcon(string iconKey, int itemType)
    {
        CharacterDressManager characterDressManager = GetUIManager<UIGameManager>().characterDressManager;
        GameItemsManager gameItemsManager = GetUIManager<UIGameManager>().gameItemsManager;

        Vector2 offsetMin = new Vector2(0, 0);
        Vector2 offsetMax = new Vector2(0, 0);
        Sprite spIcon = null;
        switch (itemType)
        {
            case (int)GeneralEnum.Hat:
                spIcon = characterDressManager.GetHatSpriteByName(iconKey);
                offsetMin = new Vector2(-30, -60);
                offsetMax = new Vector2(30, 0);
                break;
            case (int)GeneralEnum.Clothes:
                spIcon = characterDressManager.GetClothesSpriteByName(iconKey);
                offsetMin = new Vector2(-30, -20);
                offsetMax = new Vector2(30, 40);
                break;
            case (int)GeneralEnum.Shoes:
                spIcon = characterDressManager.GetShoesSpriteByName(iconKey);
                offsetMin = new Vector2(-30, 0);
                offsetMax = new Vector2(30, 60);
                break;
            default:
                spIcon = gameItemsManager.GetItemsSpriteByName(iconKey);
                break;
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            leftClick.Invoke();
        else if (eventData.button == PointerEventData.InputButton.Right)
            rightClick.Invoke();
    }

    public virtual void ButtonClick()
    {
        if (itemsInfoBean == null)
            return;
        ItemsSelectionBox selectionBox = GetUIManager<UIGameManager>().itemsSelectionBox;
        if (selectionBox != null)
            selectionBox.SetCallBack(this);
        switch (itemsInfoBean.items_type)
        {
            case (int)GeneralEnum.Menu:
                selectionBox.Open(1);
                break;
            default:
                selectionBox.Open(0);
                break;
        }
    }

    #region 选择回调
    public virtual void SelectionUse(ItemsSelectionBox view)
    {
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        ToastManager toastManager = GetUIManager<UIGameManager>().toastManager;

        if (itemsInfoBean == null || itemBean == null || gameDataManager == null)
            return;
        switch (itemsInfoBean.items_type)
        {
            case (int)GeneralEnum.Menu:
                //添加菜谱
                if (gameDataManager.gameData.AddFoodMenu(itemsInfoBean.add_id))
                {
                    RemoveItems();
                    toastManager.ToastHint(ivIcon.sprite, GameCommonInfo.GetUITextById(1006));
                }
                else
                {
                    toastManager.ToastHint(GameCommonInfo.GetUITextById(1007));
                };
                break;
            default:
                break;
        }
    }

    public virtual void SelectionDiscard(ItemsSelectionBox view)
    {
        DialogManager dialogManager = GetUIManager<UIGameManager>().dialogManager;
        if (dialogManager == null || itemsInfoBean == null)
            return;
        DialogBean dialogBean = new DialogBean
        {
            content = string.Format(GameCommonInfo.GetUITextById(3001), itemsInfoBean.name)
        };
        dialogManager.CreateDialog(0, this, dialogBean);
    }

    public virtual void SelectionEquip(ItemsSelectionBox view)
    {

    }

    public virtual void SelectionUnload(ItemsSelectionBox view)
    {

    }
    #endregion

    #region 删除确认回调
    public void Submit(DialogView dialogView, DialogBean dialogData)
    {
        RemoveItems();
    }

    public void Cancel(DialogView dialogView, DialogBean dialogData)
    {

    }
    #endregion

    /// <summary>
    /// 删除物品
    /// </summary>
    public void RemoveItems()
    {
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        gameObject.transform.DOLocalMove(new Vector3(0, 0), 0.2f).SetEase(Ease.InCirc).OnComplete(delegate
        {
            gameDataManager.gameData.listItems.Remove(itemBean);
            Destroy(gameObject);
        });
    }


}