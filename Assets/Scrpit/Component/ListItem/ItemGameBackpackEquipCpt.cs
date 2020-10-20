﻿using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using System.Collections.Generic;

public class ItemGameBackpackEquipCpt : ItemGameBackpackCpt, SkillInfoManager.ICallBack
{
    public CharacterBean characterData;

    public int type = 0;

    public ItemsInfoBean itemsInfoData;
    public ItemBean itemData;

    public override void Awake()
    {
        base.Awake();
    }

    public void SetData(CharacterBean characterData, ItemsInfoBean itemsInfoData, ItemBean itemData)
    {
        this.characterData = characterData;
        this.itemsInfoData = itemsInfoData;
        this.itemData = itemData;
        SetData(itemsInfoData, itemData);
    }

    public override void ButtonClick()
    {
        if (!isOpenClick)
        {
            return;
        }
        if (itemsInfoBean == null || itemsInfoBean.id == 0)
            return;

        if (popupItemsSelection != null)
            popupItemsSelection.SetCallBack(this);
        if (type == 1)
        {
            //普通装备卸载
            popupItemsSelection.Open(PopupItemsSelection.SelectionTypeEnum.Unload);
        }
        else if (type == 2)
        {
            //幻化装备卸载
            popupItemsSelection.Open(PopupItemsSelection.SelectionTypeEnum.Unload);
        }
        else
        {
            GeneralEnum itemsType = itemsInfoBean.GetItemsType();
            switch (itemsType)
            {
                case GeneralEnum.Hat:
                case GeneralEnum.Clothes:
                case GeneralEnum.Shoes:
                case GeneralEnum.Chef:
                case GeneralEnum.Waiter:
                case GeneralEnum.Accoutant:
                case GeneralEnum.Accost:
                case GeneralEnum.Beater:
                    popupItemsSelection.Open(PopupItemsSelection.SelectionTypeEnum.EquipAndDiscardAndTFEquip);
                    break;
                case GeneralEnum.Book:
                case GeneralEnum.SkillBook:
                case GeneralEnum.Other:

                    popupItemsSelection.Open(PopupItemsSelection.SelectionTypeEnum.UseAndDiscard);
                    break;
                default:
                    popupItemsSelection.Open(PopupItemsSelection.SelectionTypeEnum.Discard);
                    break;
            }
        }

    }



    #region  装备回调
    public override void SelectionUse(PopupItemsSelection view)
    {
        ToastManager toastManager = uiGameManager.toastManager;
        GeneralEnum itemsType = itemsInfoBean.GetItemsType();
        switch (itemsType)
        {
            case GeneralEnum.Book:
                //读书
                if (characterData.attributes.CheckLearnBook(itemsInfoBean.id))
                {
                    //已经学习过该图书
                    string toastStr = string.Format(GameCommonInfo.GetUITextById(1009), characterData.baseInfo.name, itemsInfoBean.name);
                    toastManager.ToastHint(toastStr);
                }
                else
                {
                    if (!CheckUtil.StringIsNull(itemsInfoBean.remark) && itemsInfoBean.remark.Equals("Recruit"))
                    {
                        //如果是只有招募NPC才能使用的书
                        //判断是否是招募NPC
                        if (characterData.baseInfo.characterType == (int)NpcTypeEnum.RecruitTown
                            ||(characterData.npcInfoData!=null && characterData.npcInfoData.GetNpcType() == NpcTypeEnum.RecruitTown))
                        {
                            //学习该图书
                            LearnBook();
                        }
                        else
                        {
                            toastManager.ToastHint(GameCommonInfo.GetUITextById(1035));
                        }
                    }
                    else
                    {
                        LearnBook();
                    }
                }
                break;
            case GeneralEnum.SkillBook:
                if (characterData.attributes.CheckLearnSkills(itemsInfoBean.add_id))
                {
                    //已经学习过该技能
                    string toastStr = string.Format(GameCommonInfo.GetUITextById(1063), characterData.baseInfo.name, itemsInfoBean.name);
                    toastManager.ToastHint(toastStr);
                }
                else
                {
                    //判断是否可学习
                    uiGameManager.skillInfoManager.SetCallBack(this);
                    uiGameManager.skillInfoManager.GetSkillById(itemsInfoBean.add_id);
                }
                break;
            case GeneralEnum.Other:
                if ( itemsInfoBean.id == 99900001)
                {
                    //忘记技能的孟婆汤
                    if (!CheckUtil.ListIsNull( characterData.attributes.listSkills))
                    {
                        int removePosition = Random.Range(0, characterData.attributes.listSkills.Count);
                        characterData.attributes.listSkills.RemoveAt(removePosition);
                    }
                    toastManager.ToastHint(characterData.baseInfo.name + GameCommonInfo.GetUITextById(1067));
                    RefreshItems(itemsInfoBean.id, -1);
                }
                break;
            default:
                break;
        }
        GetUIComponent<UIGameEquip>().RefreshUI();
    }

    public override void SelectionEquip(PopupItemsSelection view)
    {
        UIGameEquip uiGameEquip = GetUIComponent<UIGameEquip>();
        uiGameEquip.SetEquip(itemsInfoBean,false);
        RefreshItems(itemsInfoBean.id, -1);
    }

    public override void SelectionTFEquip(PopupItemsSelection view)
    {
        UIGameEquip uiGameEquip = GetUIComponent<UIGameEquip>();
        uiGameEquip.SetEquip(itemsInfoBean,true);
        RefreshItems(itemsInfoBean.id, -1);
    }

    public override void SelectionUnload(PopupItemsSelection view)
    {
        UIGameEquip uiGameEquip = GetUIComponent<UIGameEquip>();
        ItemsInfoBean nullItems = new ItemsInfoBean();
        nullItems.id = 0;
        nullItems.items_type = itemsInfoBean.items_type;
        if (type == 1)
        {
            //普通装备卸除
            uiGameEquip.SetEquip(nullItems,false);
        }
        else if (type == 2)
        {
            //幻化装备卸除
            uiGameEquip.SetEquip(nullItems, true);
        }
        else
        {
            //其他
            uiGameEquip.SetEquip(nullItems, false);
        }
     
    }
    #endregion


    #region  技能回调
    public void GetSkillInfoSuccess(List<SkillInfoBean> listData)
    {
        ToastManager toastManager = uiGameManager.toastManager;
        if (listData == null || listData.Count == 0)
        {
            toastManager.ToastHint(ivIcon.sprite, GameCommonInfo.GetUITextById(1065));
            return;
        }
        SkillInfoBean skillInfo = listData[0];
        bool isPre = PreTypeEnumTools.CheckIsAllPre(
            uiGameManager.gameItemsManager,
            uiGameManager.iconDataManager,
            uiGameManager.characterDressManager,
            uiGameManager.innFoodManager,
            uiGameManager.npcInfoManager,
            uiGameManager.gameDataManager.gameData, characterData, skillInfo.pre_data, out string reason);
        if (!isPre)
        {
            toastManager.ToastHint(ivIcon.sprite, reason);
        }
        else
        {
            //学习该技能
            characterData.attributes.LearnSkill(itemsInfoBean.add_id);
            string toastStr = string.Format(GameCommonInfo.GetUITextById(1064), characterData.baseInfo.name, itemsInfoBean.name);
            toastManager.ToastHint(ivIcon.sprite, toastStr);
            RefreshItems(itemsInfoBean.id, -1);
        }
    }
    #endregion

    /// <summary>
    /// 学习书籍
    /// </summary>
    protected void LearnBook()
    {
        //学习该图书
        ToastManager toastManager = uiGameManager.toastManager;
        characterData.attributes.LearnBook(itemsInfoBean.id);
        characterData.attributes.AddAttributes(itemsInfoBean);
        string toastStr = string.Format(GameCommonInfo.GetUITextById(1008), characterData.baseInfo.name, itemsInfoBean.name);
        toastManager.ToastHint(ivIcon.sprite, toastStr);
        RefreshItems(itemsInfoBean.id, -1);
    }
}