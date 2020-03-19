using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

public class ItemGameBackpackEquipCpt : ItemGameBackpackCpt
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
        if (itemsInfoBean == null || itemsInfoBean.id == 0)
            return;

        if (popupItemsSelection != null)
            popupItemsSelection.SetCallBack(this);
        if (type == 1)
        {
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
                    popupItemsSelection.Open(PopupItemsSelection.SelectionTypeEnum.EquipAndDiscard);
                    break;
                case GeneralEnum.Book:
                case GeneralEnum.SkillBook:
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
        ToastManager toastManager = GetUIManager<UIGameManager>().toastManager;
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
                    //学习该图书
                    characterData.attributes.LearnBook(itemsInfoBean.id);
                    characterData.attributes.AddAttributes(itemsInfoBean);
                    string toastStr = string.Format(GameCommonInfo.GetUITextById(1008), characterData.baseInfo.name, itemsInfoBean.name);
                    toastManager.ToastHint(ivIcon.sprite, toastStr);
                    RefreshItems(itemsInfoBean.id, -1);
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
                    //学习该技能
                    characterData.attributes.LearnSkill(itemsInfoBean.add_id);
                    string toastStr = string.Format(GameCommonInfo.GetUITextById(1064), characterData.baseInfo.name, itemsInfoBean.name);
                    toastManager.ToastHint(ivIcon.sprite, toastStr);
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
        uiGameEquip.SetEquip(itemsInfoBean);
        RefreshItems(itemsInfoBean.id, -1);
    }

    public override void SelectionUnload(PopupItemsSelection view)
    {
        UIGameEquip uiGameEquip = GetUIComponent<UIGameEquip>();
        ItemsInfoBean nullItems = new ItemsInfoBean();
        nullItems.id = 0;
        nullItems.items_type = itemsInfoBean.items_type;
        uiGameEquip.SetEquip(nullItems);
    }
    #endregion 
}