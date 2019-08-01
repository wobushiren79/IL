﻿using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

public class ItemGameBackpackEquipCpt : ItemGameBackpackCpt
{
    public CharacterBean characterData;

    public int type = 0;

    public void SetData(CharacterBean characterData,ItemsInfoBean infoBean, ItemBean itemBean)
    {
        this.characterData = characterData;
        SetData(infoBean, itemBean);
    }

    public override void ButtonRightClick()
    {
        if (itemsInfoBean == null)
            return;
        ItemsSelectionBox selectionBox = GetUIManager<UIGameManager>().itemsSelectionBox;
        if (selectionBox != null)
            selectionBox.SetCallBack(this);
        if (type == 1)
        {
            selectionBox.Open(3);
        }
        else
        {
            switch (itemsInfoBean.items_type)
            {
                case (int)GeneralEnum.Hat:
                case (int)GeneralEnum.Clothes:
                case (int)GeneralEnum.Shoes:
                    selectionBox.Open(2);
                    break;
                case (int)GeneralEnum.Book:
                    selectionBox.Open(1);
                    break;
                default:
                    selectionBox.Open(0);
                    break;
            }
        }

    }

    #region  装备回调
    public override void SelectionUse(ItemsSelectionBox view)
    {
        ToastView toastView= GetUIManager<UIGameManager>().toastView;
        switch (itemsInfoBean.items_type)
        {
            case (int)GeneralEnum.Book:
                //读书
                if (characterData.equips.CheckLearnBook(itemsInfoBean.id))
                {
                    //已经学习过该图书
                    string toastStr = string.Format(GameCommonInfo.GetUITextById(1009), characterData.baseInfo.name, itemsInfoBean.name);
                    toastView.ToastHint(toastStr);
                }
                else
                {
                    //学习该图书
                    characterData.equips.listLearnBook.Add(itemsInfoBean.id);
                    characterData.attributes.AddAttributes(itemsInfoBean);

                    string toastStr = string.Format(GameCommonInfo.GetUITextById(1008), characterData.baseInfo.name, itemsInfoBean.name);
                    toastView.ToastHint(ivIcon.sprite, toastStr);
                    RemoveItems();
                }
                break;
            default:
                break;
        }
        GetUIComponent<UIGameEquip>().RefreshUI();
    }

    public override void SelectionEquip(ItemsSelectionBox view)
    {
        GetUIComponent<UIGameEquip>().SetEquip(itemsInfoBean);
        RemoveItems();
        GetUIComponent<UIGameEquip>().RefreshUI();
    }

    public override void SelectionUnload(ItemsSelectionBox view)
    {
        ItemsInfoBean nullItems = new ItemsInfoBean();
        nullItems.id = 0;
        nullItems.items_type = itemsInfoBean.items_type;
        GetUIComponent<UIGameEquip>().SetEquip(nullItems);
        GetUIComponent<UIGameEquip>().RefreshUI();
    }
    #endregion 
}