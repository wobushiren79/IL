using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class UIGameEquip : BaseUIComponent
{
    [Header("控件")]
    public Button btBack;
    public ItemGameBackpackEquipCpt equipHand;
    public ItemGameBackpackEquipCpt equipHat;
    public ItemGameBackpackEquipCpt equipClothes;
    public ItemGameBackpackEquipCpt equipShoes;

    public CharacterAttributeView characterAttributeView;

    public Text tvLoyal;
    public Text tvCook;
    public Text tvSpeed;
    public Text tvAccount;
    public Text tvCharm;
    public Text tvForce;
    public Text tvLucky;
    public Text tvLife;

    public Image ivSex;

    public Text tvNull;

    public CharacterUICpt characterUICpt;

    [Header("模型")]
    public GameObject objItemContent;
    public GameObject objItemModel;

    [Header("数据")]
    public CharacterBean characterData;
    public Sprite spSexMan;
    public Sprite spSexWoman;

    private void Start()
    {
        if (btBack != null)
            btBack.onClick.AddListener(OpenWorkUI);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        CreateBackpackData();
        RefreshUI();
    }

    public void OpenWorkUI()
    {
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameWorker));
    }

    public void SetCharacterData(CharacterBean characterData)
    {
        this.characterData = characterData;
    }

    /// <summary>
    /// 刷新UI数据
    /// </summary>
    public override void RefreshUI()
    {
        base.RefreshUI();
        SetSex(characterData.body.sex);
        SetLoyal(characterData.attributes.loyal);
        SetAttributes(characterData.attributes, characterData.equips);

        //人物刷新
        if (characterUICpt != null)
            characterUICpt.SetCharacterData(characterData.body, characterData.equips);

        //装备物品刷新
        GameItemsManager gameItemsManager = GetUIManager<UIGameManager>().gameItemsManager;
        if (characterData.equips.handId != 0)
            equipHand.SetData(characterData, gameItemsManager.GetItemsById(characterData.equips.handId), null);
        if (characterData.equips.hatId != 0)
            equipHat.SetData(characterData, gameItemsManager.GetItemsById(characterData.equips.hatId), null);
        if (characterData.equips.clothesId != 0)
            equipClothes.SetData(characterData, gameItemsManager.GetItemsById(characterData.equips.clothesId), null);
        if (characterData.equips.shoesId != 0)
            equipShoes.SetData(characterData, gameItemsManager.GetItemsById(characterData.equips.shoesId), null);
    }

    /// <summary>
    /// 设置忠诚
    /// </summary>
    /// <param name="loyal"></param>
    public void SetLoyal(int loyal)
    {
        if (tvLoyal != null)
        {
            tvLoyal.text = loyal + "";
        }
    }
    
    /// <summary>
    /// 设置性别
    /// </summary>
    /// <param name="sex"></param>
    public void SetSex(int sex)
    {
        if (ivSex == null)
            return;
        if (sex==1)
        {
            ivSex.sprite = spSexMan;
        }
        else if (sex == 2)
        {
            ivSex.sprite = spSexWoman;
        }
    }

    /// <summary>
    /// 设置属性
    /// </summary>
    /// <param name="characterAttributes"></param>
    /// <param name="characterEquip"></param>
    public void SetAttributes(CharacterAttributesBean characterAttributes, CharacterEquipBean characterEquip)
    {
        characterData.GetAttributes(GetUIManager<UIGameManager>().gameItemsManager,
            out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
        if (tvCook != null)
            tvCook.text = GameCommonInfo.GetUITextById(1) + "：" + selfAttributes.cook + (equipAttributes.cook == 0 ? "" : "+" + equipAttributes.cook);
        if (tvSpeed != null)
            tvSpeed.text = GameCommonInfo.GetUITextById(2) + "：" + selfAttributes.speed + (equipAttributes.speed == 0 ? "" : "+" + equipAttributes.speed);
        if (tvAccount != null)
            tvAccount.text = GameCommonInfo.GetUITextById(3) + "：" + selfAttributes.account + (equipAttributes.account == 0 ? "" : "+" + equipAttributes.account);
        if (tvCharm != null)
            tvCharm.text = GameCommonInfo.GetUITextById(4) + "：" + selfAttributes.charm + (equipAttributes.charm == 0 ? "" : "+" + equipAttributes.charm);
        if (tvForce != null)
            tvForce.text = GameCommonInfo.GetUITextById(5) + "：" + selfAttributes.force + (equipAttributes.force == 0 ? "" : "+" + equipAttributes.force);
        if (tvLucky != null)
            tvLucky.text = GameCommonInfo.GetUITextById(6) + "：" + selfAttributes.lucky + (equipAttributes.lucky == 0 ? "" : "+" + equipAttributes.lucky);
        if (tvLife!=null)
            tvLife.text = GameCommonInfo.GetUITextById(9) + "：" + selfAttributes.life + (equipAttributes.life == 0 ? "" : "+" + equipAttributes.life);
        if (characterAttributeView != null)
            characterAttributeView.SetData(totalAttributes.cook, totalAttributes.speed, totalAttributes.account, totalAttributes.charm, totalAttributes.force,totalAttributes.lucky);
    }

    /// <summary>
    /// 设置装备
    /// </summary>
    /// <param name="itemId"></param>
    public void SetEquip(long itemId)
    {
        GameItemsManager gameItemsManager = GetUIManager<UIGameManager>().gameItemsManager;
        if (gameItemsManager == null)
            return;
        ItemsInfoBean itemInfo = gameItemsManager.GetItemsById(itemId);
        SetEquip(itemInfo);
    }

    /// <summary>
    /// 设置装备
    /// </summary>
    /// <param name="itemInfo"></param>
    /// <param name="unloadEquipId">卸载的装备ID</param>
    public void SetEquip(ItemsInfoBean itemInfo)
    {

        if (itemInfo == null)
            return;
        CharacterDressManager characterDressManager = GetUIManager<UIGameManager>().characterDressManager;
        GameItemsManager gameItemsManager = GetUIManager<UIGameManager>().gameItemsManager;
        GameDataManager gameDataManager = GetUIManager<UIGameManager>().gameDataManager;

        if (characterDressManager == null || gameItemsManager == null)
            return;

        ItemGameBackpackEquipCpt itemCpt = null;
        long unloadEquipId = 0;
        switch (itemInfo.items_type)
        {
            case 1:
                itemCpt = equipHat;
                unloadEquipId = characterData.equips.hatId;
                characterData.equips.hatId = itemInfo.id;
                break;
            case 2:
                itemCpt = equipClothes;
                unloadEquipId = characterData.equips.clothesId;
                characterData.equips.clothesId = itemInfo.id;
                break;
            case 3:
                itemCpt = equipShoes;
                unloadEquipId = characterData.equips.shoesId;
                characterData.equips.shoesId = itemInfo.id;
                break;
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
                itemCpt = equipHand;
                unloadEquipId = characterData.equips.handId;
                characterData.equips.handId = itemInfo.id;
                break;
        }
        itemCpt.SetData(characterData, itemInfo, null);
        //从背包里扣除装备
        gameDataManager.gameData.AddItemsNumber(itemInfo.id, -1);
        //刷新显示
        RefreshUI();
        //如果有卸载的装备 则添加到背包
        if (unloadEquipId != 0)
        {
            ItemBean unloadItem = new ItemBean(unloadEquipId, 1);
            ItemsInfoBean unloadInfo = gameItemsManager.GetItemsById(unloadEquipId);
            GameObject objItem = CreateItemBackpackData(unloadItem, unloadInfo);
            objItem.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBack).From();
            gameDataManager.gameData.AddItemsNumber(unloadEquipId, 1);
        }
    }

    /// <summary>
    /// 创建背包里的装备
    /// </summary>
    public void CreateBackpackData()
    {
        CptUtil.RemoveChildsByActive(objItemContent.transform);
        if (GetUIManager<UIGameManager>().gameItemsManager == null || GetUIManager<UIGameManager>().gameDataManager == null)
            return;
        bool hasData = false;
        for (int i = 0; i < GetUIManager<UIGameManager>().gameDataManager.gameData.listItems.Count; i++)
        {
            ItemBean itemBean = GetUIManager<UIGameManager>().gameDataManager.gameData.listItems[i];
            ItemsInfoBean itemsInfoBean = GetUIManager<UIGameManager>().gameItemsManager.GetItemsById(itemBean.itemId);
            if (itemsInfoBean == null)
                continue;
            if (itemsInfoBean.items_type != (int)GeneralEnum.Hat
                && itemsInfoBean.items_type != (int)GeneralEnum.Clothes
                && itemsInfoBean.items_type != (int)GeneralEnum.Shoes
                && itemsInfoBean.items_type != (int)GeneralEnum.Chef
                && itemsInfoBean.items_type != (int)GeneralEnum.Waiter
                && itemsInfoBean.items_type != (int)GeneralEnum.Accouting
                && itemsInfoBean.items_type != (int)GeneralEnum.Accost
                && itemsInfoBean.items_type != (int)GeneralEnum.Beater
                && itemsInfoBean.items_type != (int)GeneralEnum.Book)
                continue;
            GameObject objItem = CreateItemBackpackData(itemBean, itemsInfoBean);
            objItem.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBack).SetDelay(i * 0.05f).From();
            hasData = true;
        }
        if (hasData)
            tvNull.gameObject.SetActive(false);
        else
            tvNull.gameObject.SetActive(true);
    }

    /// <summary>
    /// 单个创建
    /// </summary>
    /// <param name="itemBean"></param>
    /// <param name="itemsInfoBean"></param>
    /// <returns></returns>
    public GameObject CreateItemBackpackData(ItemBean itemBean, ItemsInfoBean itemsInfoBean)
    {
        GameObject objItem = Instantiate(objItemModel, objItemContent.transform);
        objItem.SetActive(true);
        ItemGameBackpackEquipCpt backpackCpt = objItem.GetComponent<ItemGameBackpackEquipCpt>();
        backpackCpt.SetData(characterData, itemsInfoBean, itemBean);
        return objItem;
    }
}