using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

public class ItemGameBackpackEquipCpt : ItemGameBackpackCpt
{
    public CharacterBean characterData;

    public int type = 0;

    public void SetData(CharacterBean characterData, ItemsInfoBean itemsInfoData, ItemBean itemData)
    {
        this.characterData = characterData;
        this.itemsInfoData = itemsInfoData;
        this.itemBean = itemData;
        SetData(itemsInfoData, itemData);
    }

    public override void ButtonClick()
    {
        if (!isOpenClick)
        {
            return;
        }
        if (itemsInfoData == null || itemsInfoData.id == 0)
            return;
        DialogBean dialogData = new DialogBean();
        dialogData.dialogType = DialogEnum.ItemsSelection;
        dialogData.callBack = this;
        ItemsSelectionDialogView itemsSelectionDialog = UIHandler.Instance.ShowDialog<ItemsSelectionDialogView>(dialogData);
        itemsSelectionDialog.SetCallBack(this);
        if (type == 1)
        {
            //普通装备卸载
            itemsSelectionDialog.Open(ItemsSelectionDialogView.SelectionTypeEnum.Unload);
        }
        else if (type == 2)
        {
            //幻化装备卸载
            itemsSelectionDialog.Open(ItemsSelectionDialogView.SelectionTypeEnum.Unload);
        }
        else
        {
            GeneralEnum itemsType = itemsInfoData.GetItemsType();
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
                    itemsSelectionDialog.Open(ItemsSelectionDialogView.SelectionTypeEnum.EquipAndDiscardAndTFEquip);
                    break;
                case GeneralEnum.Book:
                case GeneralEnum.SkillBook:
                case GeneralEnum.Other:

                    itemsSelectionDialog.Open(ItemsSelectionDialogView.SelectionTypeEnum.UseAndDiscard);
                    break;
                default:
                    itemsSelectionDialog.Open(ItemsSelectionDialogView.SelectionTypeEnum.Discard);
                    break;
            }
        }

    }


    /// <summary>
    /// 设置技能数据
    /// </summary>
    /// <param name="listData"></param>
    public void SetSkillInfoData(List<SkillInfoBean> listData)
    {
        if (listData == null || listData.Count == 0)
        {
            UIHandler.Instance.ToastHint<ToastView>(ivIcon.sprite, TextHandler.Instance.manager.GetTextById(1065));
            return;
        }
        SkillInfoBean skillInfo = listData[0];
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        bool isPre = PreTypeEnumTools.CheckIsAllPre(gameData, characterData, skillInfo.pre_data, out string reason);
        if (!isPre)
        {
            UIHandler.Instance.ToastHint<ToastView>(ivIcon.sprite, reason);
        }
        else
        {
            //学习该技能
            characterData.attributes.LearnSkill(itemsInfoData.add_id);
            string toastStr = string.Format(TextHandler.Instance.manager.GetTextById(1064), characterData.baseInfo.name, itemsInfoData.name);
            UIHandler.Instance.ToastHint<ToastView>(ivIcon.sprite, toastStr);
            RefreshItems(itemsInfoData.id, -1);
        }
    }

    #region  装备回调
    public override void SelectionUse(ItemsSelectionDialogView view)
    {
        GeneralEnum itemsType = itemsInfoData.GetItemsType();
        switch (itemsType)
        {
            case GeneralEnum.Book:
                //读书
                if (characterData.attributes.CheckLearnBook(itemsInfoData.id))
                {
                    //已经学习过该图书
                    string toastStr = string.Format(TextHandler.Instance.manager.GetTextById(1009), characterData.baseInfo.name, itemsInfoData.name);
                    UIHandler.Instance.ToastHint<ToastView>(toastStr);
                }
                else
                {
                    if (!itemsInfoData.remark.IsNull() && itemsInfoData.remark.Equals("Recruit"))
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
                            UIHandler.Instance.ToastHint<ToastView>(TextHandler.Instance.manager.GetTextById(1035));
                        }
                    }
                    else
                    {
                        LearnBook();
                    }
                }
                break;
            case GeneralEnum.SkillBook:
                if (characterData.attributes.CheckLearnSkills(itemsInfoData.add_id))
                {
                    //已经学习过该技能
                    string toastStr = string.Format(TextHandler.Instance.manager.GetTextById(1063), characterData.baseInfo.name, itemsInfoData.name);
                    UIHandler.Instance.ToastHint<ToastView>(toastStr);
                }
                else
                {
                    //判断是否可学习
                    Action<List<SkillInfoBean>> callBack = SetSkillInfoData;
                    SkillInfoHandler.Instance.manager.GetSkillById(itemsInfoData.add_id,callBack);
                }
                break;
            case GeneralEnum.Other:
                if (itemsInfoData.id == 99900001)
                {
                    //忘记技能的孟婆汤
                    if (! characterData.attributes.listSkills.IsNull())
                    {
                        int removePosition = UnityEngine.Random.Range(0, characterData.attributes.listSkills.Count);
                        characterData.attributes.listSkills.RemoveAt(removePosition);
                    }
                    UIHandler.Instance.ToastHint<ToastView>(characterData.baseInfo.name + TextHandler.Instance.manager.GetTextById(1067));
                    RefreshItems(itemsInfoData.id, -1);
                }
                else if (itemsInfoData.id == 99900002)
                {
                    if (characterData.attributes.CheckLearnItem(itemsInfoData.id))
                    {
                        //已经学习过该图书
                        string toastStr = string.Format(TextHandler.Instance.manager.GetTextById(1055), characterData.baseInfo.name, itemsInfoData.name);
                        UIHandler.Instance.ToastHint<ToastView>(toastStr);
                    }
                    else
                    {
                        LearnItem();
                    }
                }
                break;
            default:
                break;
        }
        GetUIComponent<UIGameEquip>().RefreshUI();
    }

    public override void SelectionEquip(ItemsSelectionDialogView view)
    {
        UIGameEquip uiGameEquip = GetUIComponent<UIGameEquip>();
        uiGameEquip.SetEquip(itemsInfoData,false);
        RefreshItems(itemsInfoData.id, -1);
    }

    public override void SelectionTFEquip(ItemsSelectionDialogView view)
    {
        UIGameEquip uiGameEquip = GetUIComponent<UIGameEquip>();
        uiGameEquip.SetEquip(itemsInfoData,true);
        RefreshItems(itemsInfoData.id, -1);
    }

    public override void SelectionUnload(ItemsSelectionDialogView view)
    {
        UIGameEquip uiGameEquip = GetUIComponent<UIGameEquip>();
        ItemsInfoBean nullItems = new ItemsInfoBean();
        nullItems.id = 0;
        nullItems.items_type = itemsInfoData.items_type;
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
        uiComponent.RefreshUI();
    }
    #endregion

    /// <summary>
    /// 学习书籍
    /// </summary>
    protected void LearnBook()
    {
        //学习该图书
        characterData.attributes.LearnBook(itemsInfoData.id);
        characterData.attributes.AddAttributes(itemsInfoData);
        string toastStr = string.Format(TextHandler.Instance.manager.GetTextById(1008), characterData.baseInfo.name, itemsInfoData.name);
        UIHandler.Instance.ToastHint<ToastView>(ivIcon.sprite, toastStr);
        RefreshItems(itemsInfoData.id, -1);
    }

    protected void LearnItem()
    {
        //学习该道具
        characterData.attributes.LearnItem(itemsInfoData.id);
        characterData.attributes.AddAttributes(itemsInfoData);
        string toastStr = string.Format(TextHandler.Instance.manager.GetTextById(1054), characterData.baseInfo.name, itemsInfoData.name);
        UIHandler.Instance.ToastHint<ToastView>(ivIcon.sprite, toastStr);
        RefreshItems(itemsInfoData.id, -1);
    }
}