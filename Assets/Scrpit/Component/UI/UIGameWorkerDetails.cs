using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
public class UIGameWorkerDetails : BaseUIComponent
{
    [Header("控件")]
    public CharacterUICpt characterUICpt;

    public InfoItemsPopupButton pbHand;
    public Image ivHand;
    public InfoItemsPopupButton pbHat;
    public Image ivHat;
    public InfoItemsPopupButton pbClothes;
    public Image ivClothes;
    public InfoItemsPopupButton pbShoes;
    public Image ivShoes;

    public Text tvLoyal;
    public Text tvCook;
    public Text tvSpeed;
    public Text tvAccount;
    public Text tvCharm;
    public Text tvForce;
    public Text tvLucky;

    public Button btBack;

    [Header("数据")]
    public CharacterBean characterData;

    

    public void SetCharacterData(CharacterBean characterData)
    {
        this.characterData = characterData;
    }

    private void Start()
    {
        InfoItemsPopupShow infoItemsPopupShow = GetUIMananger<UIGameManager>().infoItemsPopup;
        if (btBack != null)
            btBack.onClick.AddListener(OpenWorkUI);

        if (pbHand != null)
            pbHand.SetPopupShowView(infoItemsPopupShow);
        if (pbHat != null)
            pbHat.SetPopupShowView(infoItemsPopupShow);
        if (pbClothes != null)
            pbClothes.SetPopupShowView(infoItemsPopupShow);
        if (pbShoes != null)
            pbShoes.SetPopupShowView(infoItemsPopupShow);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        if (characterData == null)
            return;
        SetLoyal(characterData.attributes.loyal);
        SetAttributes(characterData.attributes, characterData.equips);
        SetEquip(characterData.equips);
    }

    public void OpenWorkUI()
    {
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameWorker));
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

    public void SetEquip(CharacterEquipBean characterEquip)
    {
        GameItemsManager gameItemsManager = GetUIMananger<UIGameManager>().gameItemsManager;
        CharacterDressManager characterDressManager = GetUIMananger<UIGameManager>().characterDressManager;
        Sprite spHand = null;
        Sprite spHat = null;
        Sprite spClothes = null;
        Sprite spShoes = null;
        if (characterData.equips.handId != 0)
        {
            //查询装备数据
            ItemsInfoBean itemHand = gameItemsManager.GetItemsById(characterData.equips.handId);
            if (itemHand != null && !CheckUtil.StringIsNull(itemHand.icon_key))
            {
                //获取装备图标
                spHand = gameItemsManager.GetItemsSpriteByName(itemHand.icon_key);
                pbHand.SetData(itemHand, spHand);
            }
        }
        if (characterData.equips.hatId != 0)
        {
            //查询装备数据
            ItemsInfoBean itemsHat = gameItemsManager.GetItemsById(characterData.equips.hatId);
            if (itemsHat != null && !CheckUtil.StringIsNull(itemsHat.icon_key))
            {
                //获取装备图标
                spHat = characterDressManager.GetHatSpriteByName(itemsHat.icon_key);
                pbHat.SetData(itemsHat, spHat);
            }
        }
        if (characterData.equips.clothesId != 0)
        {
            //查询装备数据
            ItemsInfoBean itemsClothes = gameItemsManager.GetItemsById(characterData.equips.clothesId);
            if (itemsClothes != null && !CheckUtil.StringIsNull(itemsClothes.icon_key))
            {
                //获取装备图标
                spClothes = characterDressManager.GetClothesSpriteByName(itemsClothes.icon_key);
                pbClothes.SetData(itemsClothes, spClothes);
            }
        }
        if (characterData.equips.shoesId != 0)
        {
            //查询装备数据
            ItemsInfoBean itemShoes = gameItemsManager.GetItemsById(characterData.equips.shoesId);
            if (itemShoes != null && !CheckUtil.StringIsNull(itemShoes.icon_key))
            {
                //获取装备图标
                spShoes = characterDressManager.GetShoesSpriteByName(itemShoes.icon_key);
                pbShoes.SetData(itemShoes, spShoes);
            }
        }
        SetEquipSprite(ivHand, spHand);
        SetEquipSprite(ivHat, spHat);
        SetEquipSprite(ivClothes, spClothes);
        SetEquipSprite(ivShoes, spShoes);
    }

    private void SetEquipSprite(Image iv, Sprite spIcon)
    {
        if (spIcon != null)
        {
            iv.sprite = spIcon;
            iv.color = new Color(1, 1, 1, 1);
        }
        else
            iv.color = new Color(1, 1, 1, 0);
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
}