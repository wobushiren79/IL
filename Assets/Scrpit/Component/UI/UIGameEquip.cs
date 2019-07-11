using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class UIGameEquip : BaseUIComponent
{
    public Button btBack;
    public Image ivHand;
    public Image ivHat;
    public Image ivClothes;
    public Image ivShoes;


    public GameObject objItemContent;
    public GameObject objItemModel;

    public GameDataManager gameDataManager;
    public GameItemsManager gameItemsManager;
    public CharacterDressManager characterDressManager;

    public CharacterBean characterData;
    private void Start()
    {
        if (btBack != null)
            btBack.onClick.AddListener(OpenWorkUI);
    }

    public void SetCharacterData(CharacterBean characterData)
    {
        this.characterData = characterData;
        ItemsInfoBean itemsHand = gameItemsManager.GetItemsById(characterData.equips.handId);
        Equip(itemsHand, (int)GeneralEnum.Hand);
        ItemsInfoBean itemsHat = gameItemsManager.GetItemsById(characterData.equips.hatId);
        Equip(itemsHat, (int)GeneralEnum.Hat);
        ItemsInfoBean itemsClothes = gameItemsManager.GetItemsById(characterData.equips.clothesId);
        Equip(itemsClothes, (int)GeneralEnum.Clothes);
        ItemsInfoBean itemsShoes = gameItemsManager.GetItemsById(characterData.equips.shoesId);
        Equip(itemsShoes, (int)GeneralEnum.Shoes);
    }

    public void Equip(ItemsInfoBean itemsInfo)
    {
        Equip(itemsInfo, -1);
    }
    public void Equip(ItemsInfoBean itemsInfo, int type)
    {
        if (gameItemsManager == null)
            return;
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
                this.characterData.equips.hatId = itemsInfo.id;
                Sprite spHat = characterDressManager.GetHatSpriteByName(itemsInfo.icon_key);
                if (itemsInfo.icon_key == null || spHat == null)
                    ivHat.color = new Color(1, 1, 1,0);
                else
                    ivHat.color = new Color(1, 1, 1, 1);
                ivHat.sprite = spHat;
                break;
            case (int)GeneralEnum.Clothes:
                this.characterData.equips.clothesId = itemsInfo.id;
                Sprite spClothes = characterDressManager.GetClothesSpriteByName(itemsInfo.icon_key);
                if (itemsInfo.icon_key == null || spClothes == null)
                    ivClothes.color = new Color(1, 1, 1, 0);
                else
                    ivClothes.color = new Color(1, 1, 1, 1);
                ivClothes.sprite = spClothes;
                break;
            case (int)GeneralEnum.Shoes:
                this.characterData.equips.shoesId = itemsInfo.id;
                Sprite spShoes = characterDressManager.GetShoesSpriteByName(itemsInfo.icon_key);
                if (itemsInfo.icon_key == null || spShoes == null)
                    ivShoes.color = new Color(1, 1, 1, 0);
                else
                    ivShoes.color = new Color(1, 1, 1, 1);
                ivShoes.sprite = spShoes;
                break;
            case (int)GeneralEnum.Hand:
                this.characterData.equips.handId = itemsInfo.id;
                Sprite spHand = characterDressManager.GetShoesSpriteByName(itemsInfo.icon_key);
                if (itemsInfo.icon_key == null || spHand == null)
                    ivHand.color = new Color(1, 1, 1, 0);
                else
                    ivHand.color = new Color(1, 1, 1, 1);
                ivShoes.sprite = spHand;
                break;
        }
    }


    public override void OpenUI()
    {
        base.OpenUI();
        CreateBackpackData();
    }


    public void OpenWorkUI()
    {
        uiManager.OpenUIAndCloseOtherByName("Worker");
    }

    public void CreateBackpackData()
    {
        CptUtil.RemoveChildsByActive(objItemContent.transform);
        if (gameItemsManager == null || gameDataManager == null)
            return;
        for (int i = 0; i < gameDataManager.gameData.itemsList.Count; i++)
        {
            ItemBean itemBean = gameDataManager.gameData.itemsList[i];
            ItemsInfoBean itemsInfoBean = gameItemsManager.GetItemsById(itemBean.itemId);
            if (itemsInfoBean == null)
                continue;
            if (itemsInfoBean.items_type != 1
                && itemsInfoBean.items_type != 2
                && itemsInfoBean.items_type != 3
                && itemsInfoBean.items_type != 11)
                continue;
            GameObject objItem = Instantiate(objItemModel, objItemContent.transform);
            objItem.SetActive(true);
            ItemGameBackpackCpt backpackCpt = objItem.GetComponent<ItemGameBackpackCpt>();
            backpackCpt.SetData(itemsInfoBean, itemBean);
            objItem.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBack).SetDelay(i * 0.05f).From();
        }
    }
}