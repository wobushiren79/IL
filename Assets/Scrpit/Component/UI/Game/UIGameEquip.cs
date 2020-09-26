using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class UIGameEquip : UIGameComponent
{
    [Header("控件")]
    public Button btBack;
    public ItemGameBackpackEquipCpt equipHand;
    public ItemGameBackpackEquipCpt equipHat;
    public ItemGameBackpackEquipCpt equipClothes;
    public ItemGameBackpackEquipCpt equipShoes;

    //幻化
    public ItemGameBackpackEquipCpt equipTFHand;
    public ItemGameBackpackEquipCpt equipTFHat;
    public ItemGameBackpackEquipCpt equipTFClothes;
    public ItemGameBackpackEquipCpt equipTFShoes;

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
    public Button btLast;
    public Button btNext;

    [Header("模型")]
    public GameObject objItemContent;
    public GameObject objItemModel;

    [Header("数据")]
    public CharacterBean characterData;
    public Sprite spSexMan;
    public Sprite spSexWoman;

    public List<CharacterBean> listCharacter = new List<CharacterBean>();

    private void Start()
    {
        if (btBack != null)
            btBack.onClick.AddListener(OpenWorkUI);
        if (btLast != null)
        {
            btLast.onClick.AddListener(OnClickForLastCharacter);
        }
        if (btNext != null)
        {
            btNext.onClick.AddListener(OnClickForNextCharacter);
        }
    }

    public override void OpenUI()
    {
        base.OpenUI();
        StopAllCoroutines();
        CptUtil.RemoveChildsByActive(objItemContent.transform);
        StartCoroutine(CreateBackpackData());
        RefreshUI();
    }

    public override void CloseUI()
    {
        base.CloseUI();
    }

    public void OpenWorkUI()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForBack);
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameWorker));
    }

    public void SetCharacterData(List<CharacterBean> listCharacter, CharacterBean characterData)
    {
        this.listCharacter = listCharacter;
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
        equipHand.SetData(characterData, uiGameManager.gameItemsManager.GetItemsById(characterData.equips.handId), null);
        equipHat.SetData(characterData, uiGameManager.gameItemsManager.GetItemsById(characterData.equips.hatId), null);
        equipClothes.SetData(characterData, uiGameManager.gameItemsManager.GetItemsById(characterData.equips.clothesId), null);
        equipShoes.SetData(characterData, uiGameManager.gameItemsManager.GetItemsById(characterData.equips.shoesId), null);

        equipTFHand.SetData(characterData, uiGameManager.gameItemsManager.GetItemsById(characterData.equips.handTFId), null);
        equipTFHat.SetData(characterData, uiGameManager.gameItemsManager.GetItemsById(characterData.equips.hatTFId), null);
        equipTFClothes.SetData(characterData, uiGameManager.gameItemsManager.GetItemsById(characterData.equips.clothesTFId), null);
        equipTFShoes.SetData(characterData, uiGameManager.gameItemsManager.GetItemsById(characterData.equips.shoesTFId), null);

        //装备列表是否为null   
        if (CptUtil.GetChildCountByActive(objItemContent) != 0)
            tvNull.gameObject.SetActive(false);
        else
            tvNull.gameObject.SetActive(true);

        RefreshBackpackData();
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
        if (sex == 1)
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
        characterData.GetAttributes(uiGameManager.gameItemsManager,
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
        if (tvLife != null)
            tvLife.text = GameCommonInfo.GetUITextById(9) + "：" + selfAttributes.life + (equipAttributes.life == 0 ? "" : "+" + equipAttributes.life);
        if (characterAttributeView != null)
            characterAttributeView.SetData(totalAttributes.cook, totalAttributes.speed, totalAttributes.account, totalAttributes.charm, totalAttributes.force, totalAttributes.lucky);
    }

    /// <summary>
    /// 设置装备
    /// </summary>
    /// <param name="itemId"></param>
    public void SetEquip(long itemId, bool isTFequip)
    {
        if (uiGameManager.gameItemsManager == null)
            return;
        ItemsInfoBean itemInfo = uiGameManager.gameItemsManager.GetItemsById(itemId);
        SetEquip(itemInfo, isTFequip);
    }


    /// <summary>
    /// 设置装备 
    /// </summary>
    /// <param name="itemInfo"></param>
    /// <param name="isTFequip">是否是幻化</param>
    public void SetEquip(ItemsInfoBean itemInfo,bool isTFequip)
    {
        if (itemInfo == null)
            return;
        if (uiGameManager.characterDressManager == null || uiGameManager.gameItemsManager == null)
            return;

        ItemGameBackpackEquipCpt itemCpt = null;
        long unloadEquipId = 0;
        GeneralEnum itemType = itemInfo.GetItemsType();
        switch (itemType)
        {
            case GeneralEnum.Hat:
    
                if (isTFequip)
                {
                    itemCpt = equipTFHat;
                    unloadEquipId = characterData.equips.hatTFId;
                    characterData.equips.hatTFId = itemInfo.id;
                }
                else
                {
                    itemCpt = equipHat;
                    unloadEquipId = characterData.equips.hatId;
                    characterData.equips.hatId = itemInfo.id;
                }
      
                break;
            case GeneralEnum.Clothes:
                if (isTFequip)
                {
                    itemCpt = equipTFClothes;
                    unloadEquipId = characterData.equips.clothesTFId;
                    characterData.equips.clothesTFId = itemInfo.id;
                }
                else
                {
                    itemCpt = equipClothes;
                    unloadEquipId = characterData.equips.clothesId;
                    characterData.equips.clothesId = itemInfo.id;
                }
                break;
            case GeneralEnum.Shoes:
                if (isTFequip)
                {
                    itemCpt = equipTFShoes;
                    unloadEquipId = characterData.equips.shoesTFId;
                    characterData.equips.shoesTFId = itemInfo.id;
                }
                else
                {
                    itemCpt = equipShoes;
                    unloadEquipId = characterData.equips.shoesId;
                    characterData.equips.shoesId = itemInfo.id;
                }
                break;
            case GeneralEnum.Chef:
            case GeneralEnum.Waiter:
            case GeneralEnum.Accoutant:
            case GeneralEnum.Accost:
            case GeneralEnum.Beater:
                if (isTFequip)
                {
                    itemCpt = equipTFHand;
                    unloadEquipId = characterData.equips.handTFId;
                    characterData.equips.handTFId = itemInfo.id;
                }
                else
                {
                    itemCpt = equipHand;
                    unloadEquipId = characterData.equips.handId;
                    characterData.equips.handId = itemInfo.id;
                }
                break;
        }
        itemCpt.SetData(characterData, itemInfo, null);

        //如果有卸载的装备 则添加到背包
        if (unloadEquipId != 0)
        {
            ItemBean unloadItemData =  uiGameManager.gameDataManager.gameData.AddItemsNumber(unloadEquipId, 1);
            ItemsInfoBean unloadItemInfo = uiGameManager.gameItemsManager.GetItemsById(unloadEquipId);
            //如果是背包里没有
            if (unloadItemData.itemNumber == 1)
            {
                GameObject objItem = CreateItemBackpackData(unloadItemData, unloadItemInfo);
                objItem.transform.DOScale(new Vector3(0, 0, 0), 0.2f).SetEase(Ease.OutBack).From();
            }
        }

        //刷新显示
        RefreshUI();
        //刷新场景中的人物
        if (uiGameManager.npcWorkerBuilder != null)
        {
            uiGameManager.npcWorkerBuilder.RefreshWorkerData();
        }

    }

    /// <summary>
    /// 创建背包里的装备
    /// </summary>
    public IEnumerator CreateBackpackData()
    {
        if (uiGameManager.gameItemsManager == null || uiGameManager.gameDataManager == null)
            yield return null;
        for (int i = 0; i < uiGameManager.gameDataManager.gameData.listItems.Count; i++)
        {
            ItemBean itemBean = uiGameManager.gameDataManager.gameData.listItems[i];
            ItemsInfoBean itemInfo = uiGameManager.gameItemsManager.GetItemsById(itemBean.itemId);
            if (itemInfo == null)
                continue;
            GameObject objItem = CreateItemBackpackData(itemBean, itemInfo);
            if (i % ProjectConfigInfo.ITEM_REFRESH_NUMBER == 0)
                yield return new WaitForEndOfFrame();
        }
        //装备列表是否为null   
        if (CptUtil.GetChildCountByActive(objItemContent) != 0)
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
    public GameObject CreateItemBackpackData(ItemBean itemBean, ItemsInfoBean itemInfo)
    {
        GeneralEnum itemType = itemInfo.GetItemsType();
        if (itemType != GeneralEnum.Hat
            && itemType != GeneralEnum.Clothes
            && itemType != GeneralEnum.Shoes
            && itemType != GeneralEnum.Chef
            && itemType != GeneralEnum.Waiter
            && itemType != GeneralEnum.Accoutant
            && itemType != GeneralEnum.Accost
            && itemType != GeneralEnum.Beater
            && itemType != GeneralEnum.Book
            && itemType != GeneralEnum.SkillBook
            && itemType != GeneralEnum.Other)
            return null;
        GameObject objItem = Instantiate(objItemContent, objItemModel);
        ItemGameBackpackEquipCpt backpackCpt = objItem.GetComponent<ItemGameBackpackEquipCpt>();
        backpackCpt.SetData(characterData, itemInfo, itemBean);
        return objItem;
    }

    /// <summary>
    /// 刷新背包数据
    /// </summary>
    public void RefreshBackpackData()
    {
        ItemGameBackpackEquipCpt[] allItems= objItemContent.GetComponentsInChildren<ItemGameBackpackEquipCpt>();
        foreach (ItemGameBackpackEquipCpt itemData in allItems)
        { 
            itemData.SetData(characterData, itemData.itemsInfoData, itemData.itemData);
        }
    }

    protected void OnClickForLastCharacter()
    {
        ChangeCharacter(-1);
    }
    protected void OnClickForNextCharacter()
    {
        ChangeCharacter(1);
    }
    protected void ChangeCharacter(int number)
    {
        if (CheckUtil.ListIsNull(listCharacter))
        {
            return;
        }
        int nextPosition = 0;
        for (int i = 0; i < listCharacter.Count; i++)
        {
            CharacterBean itemCharater = listCharacter[i];
            if (itemCharater == characterData)
            {
                nextPosition = i + number;
                if (nextPosition >= listCharacter.Count)
                {
                    nextPosition = nextPosition - listCharacter.Count;
                }
                else if (nextPosition < 0)
                {
                    nextPosition = listCharacter.Count + nextPosition;
                }
                break;
            }
        }
        SetCharacterData(listCharacter, listCharacter[nextPosition]);
        RefreshUI();
    }


}