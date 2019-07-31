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
    public Image ivHand;

    public ItemGameBackpackEquipCpt equipHat;
    public Image ivHat;

    public ItemGameBackpackEquipCpt equipClothes;
    public Image ivClothes;

    public ItemGameBackpackEquipCpt equipShoes;
    public Image ivShoes;

    public Text tvLoyal;
    public Text tvCook;
    public Text tvSpeed;
    public Text tvAccount;
    public Text tvCharm;
    public Text tvForce;
    public Text tvLucky;

    public CharacterUICpt characterUICpt;

    [Header("模型")]
    public GameObject objItemContent;
    public GameObject objItemModel;

    [Header("数据")]
    public CharacterBean characterData;

    private void Start()
    {
        if (btBack != null)
            btBack.onClick.AddListener(OpenWorkUI);
        if (equipHand != null)
        {
            equipHand.SetSelectionBox(GetUIMananger<UIGameManager>().itemsSelectionBox);
            equipHand.SetPopupShowView(GetUIMananger<UIGameManager>().infoItemsPopup);
        }
        if (equipHat != null)
        {
            equipHat.SetSelectionBox(GetUIMananger<UIGameManager>().itemsSelectionBox);
            equipHat.SetPopupShowView(GetUIMananger<UIGameManager>().infoItemsPopup);
        }
        if (equipClothes != null)
        {
            equipClothes.SetSelectionBox(GetUIMananger<UIGameManager>().itemsSelectionBox);
            equipClothes.SetPopupShowView(GetUIMananger<UIGameManager>().infoItemsPopup);
        }
        if (equipShoes != null)
        {
            equipShoes.SetSelectionBox(GetUIMananger<UIGameManager>().itemsSelectionBox);
            equipShoes.SetPopupShowView(GetUIMananger<UIGameManager>().infoItemsPopup);
        }
    }

    public void SetCharacterData(CharacterBean characterData)
    {
        this.characterData = characterData;
        ItemsInfoBean itemsHand = GetUIMananger<UIGameManager>().gameItemsManager.GetItemsById(characterData.equips.handId);
        SetEquipIcon(itemsHand, (int)GeneralEnum.Hand);
        ItemsInfoBean itemsHat = GetUIMananger<UIGameManager>().gameItemsManager.GetItemsById(characterData.equips.hatId);
        SetEquipIcon(itemsHat, (int)GeneralEnum.Hat);
        ItemsInfoBean itemsClothes = GetUIMananger<UIGameManager>().gameItemsManager.GetItemsById(characterData.equips.clothesId);
        SetEquipIcon(itemsClothes, (int)GeneralEnum.Clothes);
        ItemsInfoBean itemsShoes = GetUIMananger<UIGameManager>().gameItemsManager.GetItemsById(characterData.equips.shoesId);
        SetEquipIcon(itemsShoes, (int)GeneralEnum.Shoes);
        RefreshUI();
    }

    /// <summary>
    /// 刷新UI数据
    /// </summary>
    public override void RefreshUI()
    {
        base.RefreshUI();
        SetLoyal(characterData.attributes.loyal);
        SetAttributes(characterData.attributes, characterData.equips);
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
    /// 设置属性
    /// </summary>
    /// <param name="characterAttributes"></param>
    /// <param name="characterEquip"></param>
    public void SetAttributes(CharacterAttributesBean characterAttributes, CharacterEquipBean characterEquip)
    {
        CharacterAttributesBean extraAttributes = characterEquip.GetEquipAttributes(GetUIMananger<UIGameManager>().gameItemsManager);
        if (tvCook != null)
            tvCook.text = GameCommonInfo.GetUITextById(1) + "：" + characterAttributes.cook + (extraAttributes.cook == 0 ? "" : "+" + extraAttributes.cook);
        if (tvSpeed != null)
            tvSpeed.text = GameCommonInfo.GetUITextById(2) + "：" + characterAttributes.speed + (extraAttributes.speed == 0 ? "" : "+" + extraAttributes.speed);
        if (tvAccount != null)
            tvAccount.text = GameCommonInfo.GetUITextById(3) + "：" + characterAttributes.account + (extraAttributes.account == 0 ? "" : "+" + extraAttributes.account);
        if (tvCharm != null)
            tvCharm.text = GameCommonInfo.GetUITextById(4) + "：" + characterAttributes.charm + (extraAttributes.charm == 0 ? "" : "+" + extraAttributes.charm);
        if (tvForce != null)
            tvForce.text = GameCommonInfo.GetUITextById(5) + "：" + characterAttributes.force + (extraAttributes.force == 0 ? "" : "+" + extraAttributes.force);
        if (tvLucky != null)
            tvLucky.text = GameCommonInfo.GetUITextById(6) + "：" + characterAttributes.lucky + (extraAttributes.lucky == 0 ? "" : "+" + extraAttributes.lucky);

    }

    /// <summary>
    /// 设置装备数据
    /// </summary>
    /// <param name="itemsInfo"></param>
    public void SetEquip(ItemsInfoBean itemsInfo)
    {
        SetEquip(itemsInfo, -1);
    }

    public void SetEquip(ItemsInfoBean itemsInfo, int type)
    {
        long unloadItemsId = SetEquipIcon(itemsInfo, type);
        //装备添加卸下的
        if (unloadItemsId != 0)
        {
            ItemBean itemBean = new ItemBean(unloadItemsId, 1);
            ItemsInfoBean itemsInfoBean = GetUIMananger<UIGameManager>().gameItemsManager.GetItemsById(unloadItemsId);
            GetUIMananger<UIGameManager>().gameDataManager.gameData.itemsList.Add(itemBean);
            GameObject objItem = CreateItemBackpackData(itemBean, itemsInfoBean);
            objItem.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBack).From();
        }
    }

    /// <summary>
    /// 设置装备图标
    /// </summary>
    /// <param name="itemsInfo"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public long SetEquipIcon(ItemsInfoBean itemsInfo, int type)
    {
        long unloadItemsId = 0;
        if (GetUIMananger<UIGameManager>().gameItemsManager == null)
            return unloadItemsId;
        if (itemsInfo == null)
        {
            itemsInfo = new ItemsInfoBean
            {
                items_type = type
            };
        }
        switch (itemsInfo.items_type)
        {
            case (int)GeneralEnum.Hat:
                unloadItemsId = this.characterData.equips.hatId;
                this.characterData.equips.hatId = itemsInfo.id;
                Sprite spHat = GetUIMananger<UIGameManager>().characterDressManager.GetHatSpriteByName(itemsInfo.icon_key);
                if (itemsInfo.icon_key == null || spHat == null)
                    ivHat.color = new Color(1, 1, 1, 0);
                else
                    ivHat.color = new Color(1, 1, 1, 1);
                ivHat.sprite = spHat;
                equipHat.SetData(itemsInfo, null);
                break;
            case (int)GeneralEnum.Clothes:
                unloadItemsId = this.characterData.equips.clothesId;
                this.characterData.equips.clothesId = itemsInfo.id;
                Sprite spClothes = GetUIMananger<UIGameManager>().characterDressManager.GetClothesSpriteByName(itemsInfo.icon_key);
                if (itemsInfo.icon_key == null || spClothes == null)
                    ivClothes.color = new Color(1, 1, 1, 0);
                else
                    ivClothes.color = new Color(1, 1, 1, 1);
                ivClothes.sprite = spClothes;
                equipClothes.SetData(itemsInfo, null);
                break;
            case (int)GeneralEnum.Shoes:
                unloadItemsId = this.characterData.equips.shoesId;
                this.characterData.equips.shoesId = itemsInfo.id;
                Sprite spShoes = GetUIMananger<UIGameManager>().characterDressManager.GetShoesSpriteByName(itemsInfo.icon_key);
                if (itemsInfo.icon_key == null || spShoes == null)
                    ivShoes.color = new Color(1, 1, 1, 0);
                else
                    ivShoes.color = new Color(1, 1, 1, 1);
                ivShoes.sprite = spShoes;
                equipShoes.SetData(itemsInfo, null);
                break;
            case (int)GeneralEnum.Hand:
                unloadItemsId = this.characterData.equips.handId;
                this.characterData.equips.handId = itemsInfo.id;
                Sprite spHand = GetUIMananger<UIGameManager>().gameItemsManager.GetItemsSpriteByName(itemsInfo.icon_key);
                if (itemsInfo.icon_key == null || spHand == null)
                    ivHand.color = new Color(1, 1, 1, 0);
                else
                    ivHand.color = new Color(1, 1, 1, 1);
                ivShoes.sprite = spHand;
                equipHand.SetData(itemsInfo, null);
                break;
        }

        characterUICpt.SetCharacterData(characterData.body, characterData.equips);
        return unloadItemsId;
    }

    public override void OpenUI()
    {
        base.OpenUI();
        CreateBackpackData();
    }

    public void OpenWorkUI()
    {
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameWorker));
    }

    public void CreateBackpackData()
    {
        CptUtil.RemoveChildsByActive(objItemContent.transform);
        if (GetUIMananger<UIGameManager>().gameItemsManager == null || GetUIMananger<UIGameManager>().gameDataManager == null)
            return;
        for (int i = 0; i < GetUIMananger<UIGameManager>().gameDataManager.gameData.itemsList.Count; i++)
        {
            ItemBean itemBean = GetUIMananger<UIGameManager>().gameDataManager.gameData.itemsList[i];
            ItemsInfoBean itemsInfoBean = GetUIMananger<UIGameManager>().gameItemsManager.GetItemsById(itemBean.itemId);
            if (itemsInfoBean == null)
                continue;
            if (itemsInfoBean.items_type != 1
                && itemsInfoBean.items_type != 2
                && itemsInfoBean.items_type != 3
                && itemsInfoBean.items_type != 11)
                continue;
            GameObject objItem = CreateItemBackpackData(itemBean, itemsInfoBean);
            objItem.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBack).SetDelay(i * 0.05f).From();
        }
    }

    public GameObject CreateItemBackpackData(ItemBean itemBean, ItemsInfoBean itemsInfoBean)
    {
        GameObject objItem = Instantiate(objItemModel, objItemContent.transform);
        objItem.SetActive(true);
        ItemGameBackpackEquipCpt backpackCpt = objItem.GetComponent<ItemGameBackpackEquipCpt>(); 
        backpackCpt.SetSelectionBox(GetUIMananger<UIGameManager>().itemsSelectionBox);
        backpackCpt.SetPopupShowView(GetUIMananger<UIGameManager>().infoItemsPopup);
        backpackCpt.SetData(characterData, itemsInfoBean, itemBean);
        return objItem;
    }
}