using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UIGameEquip : BaseUIComponent, TextSearchView.ICallBack
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

    public ScrollGridVertical gridVertical;
    public TextSearchView textSearchView;

    [Header("数据")]
    public CharacterBean characterData;
    public Sprite spSexMan;
    public Sprite spSexWoman;

    public List<CharacterBean> listCharacter = new List<CharacterBean>();
    protected List<ItemBean> listItemData = new List<ItemBean>();

    public Button ui_ItemShowSortDetails_Weapons;
    public Button ui_ItemShowSortDetails_Hat;
    public Button ui_ItemShowSortDetails_Clothes;
    public Button ui_ItemShowSortDetails_Shoes;
    public Button ui_ItemShowSortDetails_Book;
    public Button ui_ItemShowSortDetails_Menu;
    public Button ui_ItemShowSortDetails_Medicine;
    public Button ui_ItemShowSortDetails_Skill;
    public Button ui_ItemShowSortDetails_Gift;

    protected GeneralEnum sortType = GeneralEnum.Null;
    private void Start()
    {
        if (btBack != null)
            btBack.onClick.AddListener(OpenWorkUI);
        if (btLast != null)
            btLast.onClick.AddListener(OnClickForLastCharacter);
        if (btNext != null)
            btNext.onClick.AddListener(OnClickForNextCharacter);
        if (gridVertical)
            gridVertical.AddCellListener(OnCellForItems);
        if (textSearchView)
            textSearchView.SetCallBack(this); 
        if (ui_ItemShowSortDetails_Weapons)
            ui_ItemShowSortDetails_Weapons.onClick.AddListener(OnClickForSortWeapons);
        if (ui_ItemShowSortDetails_Hat)
            ui_ItemShowSortDetails_Hat.onClick.AddListener(OnClickForSortHat);
        if (ui_ItemShowSortDetails_Clothes)
            ui_ItemShowSortDetails_Clothes.onClick.AddListener(OnClickForSortClothes);
        if (ui_ItemShowSortDetails_Shoes)
            ui_ItemShowSortDetails_Shoes.onClick.AddListener(OnClickForSortShoes);
        if (ui_ItemShowSortDetails_Book)
            ui_ItemShowSortDetails_Book.onClick.AddListener(OnClickForSortBook);
        if (ui_ItemShowSortDetails_Menu)
            ui_ItemShowSortDetails_Menu.onClick.AddListener(OnClickForSortMenu);
        if (ui_ItemShowSortDetails_Medicine)
            ui_ItemShowSortDetails_Medicine.onClick.AddListener(OnClickForSortMedicine);
        if (ui_ItemShowSortDetails_Skill)
            ui_ItemShowSortDetails_Skill.onClick.AddListener(OnClickForSortSkill);
        if (ui_ItemShowSortDetails_Gift)
            ui_ItemShowSortDetails_Gift.onClick.AddListener(OnClickForSortGift);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        RefreshUI();
    }

    public override void CloseUI()
    {
        base.CloseUI();
    }

    public void OpenWorkUI()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForBack);
        UIHandler.Instance.OpenUIAndCloseOther<UIGameWorker>();
    }

    public void OnCellForItems(ScrollGridCell itemCell)
    {
        int index = itemCell.index;
        ItemBean itemBean = listItemData[index];
        ItemsInfoBean itemInfo = GameItemsHandler.Instance.manager.GetItemsById(itemBean.itemId);
        ItemGameBackpackEquipCpt backpackCpt = itemCell.GetComponent<ItemGameBackpackEquipCpt>();
        backpackCpt.SetData(characterData, itemInfo, itemBean);
    }

    public void SetCharacterData(List<CharacterBean> listCharacter, CharacterBean characterData)
    {
        this.listCharacter = listCharacter;
        this.characterData = characterData;
    }

    /// <summary>
    /// 刷新UI数据
    /// </summary>
    public override void RefreshUI(bool isOpenInit = false)
    {
        base.RefreshUI(isOpenInit);
        if (isOpenInit)
            return;
        SetSex(characterData.body.sex);
        SetLoyal(characterData.attributes.loyal);
        SetAttributes(characterData.attributes, characterData.equips);

        //人物刷新
        if (characterUICpt != null)
            characterUICpt.SetCharacterData(characterData.body, characterData.equips);

        //装备物品刷新
        equipHand.SetData(characterData, GameItemsHandler.Instance.manager.GetItemsById(characterData.equips.handId), null);
        equipHat.SetData(characterData, GameItemsHandler.Instance.manager.GetItemsById(characterData.equips.hatId), null);
        equipClothes.SetData(characterData, GameItemsHandler.Instance.manager.GetItemsById(characterData.equips.clothesId), null);
        equipShoes.SetData(characterData, GameItemsHandler.Instance.manager.GetItemsById(characterData.equips.shoesId), null);

        equipTFHand.SetData(characterData, GameItemsHandler.Instance.manager.GetItemsById(characterData.equips.handTFId), null);
        equipTFHat.SetData(characterData, GameItemsHandler.Instance.manager.GetItemsById(characterData.equips.hatTFId), null);
        equipTFClothes.SetData(characterData, GameItemsHandler.Instance.manager.GetItemsById(characterData.equips.clothesTFId), null);
        equipTFShoes.SetData(characterData, GameItemsHandler.Instance.manager.GetItemsById(characterData.equips.shoesTFId), null);

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
        characterData.GetAttributes(out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
        if (tvCook != null)
            tvCook.text = TextHandler.Instance.manager.GetTextById(1) + "：" + selfAttributes.cook + (equipAttributes.cook == 0 ? "" : "+" + equipAttributes.cook);
        if (tvSpeed != null)
            tvSpeed.text = AttributesTypeEnumTools.GetAttributesName(AttributesTypeEnum.Speed) + "：" + selfAttributes.speed + (equipAttributes.speed == 0 ? "" : "+" + equipAttributes.speed);
        if (tvAccount != null)
            tvAccount.text = AttributesTypeEnumTools.GetAttributesName(AttributesTypeEnum.Account) + "：" + selfAttributes.account + (equipAttributes.account == 0 ? "" : "+" + equipAttributes.account);
        if (tvCharm != null)
            tvCharm.text = AttributesTypeEnumTools.GetAttributesName(AttributesTypeEnum.Charm) + "：" + selfAttributes.charm + (equipAttributes.charm == 0 ? "" : "+" + equipAttributes.charm);
        if (tvForce != null)
            tvForce.text = AttributesTypeEnumTools.GetAttributesName(AttributesTypeEnum.Force) + "：" + selfAttributes.force + (equipAttributes.force == 0 ? "" : "+" + equipAttributes.force);
        if (tvLucky != null)
            tvLucky.text = AttributesTypeEnumTools.GetAttributesName(AttributesTypeEnum.Lucky) + "：" + selfAttributes.lucky + (equipAttributes.lucky == 0 ? "" : "+" + equipAttributes.lucky);
        if (tvLife != null)
            tvLife.text = AttributesTypeEnumTools.GetAttributesName(AttributesTypeEnum.Life) + "：" + selfAttributes.life + (equipAttributes.life == 0 ? "" : "+" + equipAttributes.life);
        if (characterAttributeView != null)
            characterAttributeView.SetData(totalAttributes.cook, totalAttributes.speed, totalAttributes.account, totalAttributes.charm, totalAttributes.force, totalAttributes.lucky);
    }

    /// <summary>
    /// 设置装备
    /// </summary>
    /// <param name="itemId"></param>
    public void SetEquip(long itemId, bool isTFequip)
    {
        ItemsInfoBean itemInfo = GameItemsHandler.Instance.manager.GetItemsById(itemId);
        SetEquip(itemInfo, isTFequip);
    }


    /// <summary>
    /// 设置装备 
    /// </summary>
    /// <param name="itemInfo"></param>
    /// <param name="isTFequip">是否是幻化</param>
    public void SetEquip(ItemsInfoBean itemInfo, bool isTFequip)
    {
        if (itemInfo == null)
            return;
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
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
            gameData.AddItemsNumber(unloadEquipId, 1);
        }
        //刷新场景中的人物
        if (SceneUtil.GetCurrentScene() == ScenesEnum.GameInnScene)
        {
            NpcHandler.Instance.builderForWorker.RefreshWorkerData();
            NpcHandler.Instance.builderForFamily.RefreshFamilyData();
        }
    
    }

    /// <summary>
    /// 刷新背包数据
    /// </summary>
    public void RefreshBackpackData()
    {
        listItemData.Clear();
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        for (int i = 0; i < gameData.listItems.Count; i++)
        {
            ItemBean itemBean = gameData.listItems[i];
            ItemsInfoBean itemInfo = GameItemsHandler.Instance.manager.GetItemsById(itemBean.itemId);
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
                continue;
            listItemData.Add(itemBean);
        }
        gridVertical.SetCellCount(listItemData.Count);
        gridVertical.RefreshAllCells();
        if (listItemData.Count <= 0)
        {
            tvNull.gameObject.SetActive(true);
        }
        else
        {
            tvNull.gameObject.SetActive(false);
        }
        SortForItemsType(sortType);
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
        if (listCharacter.IsNull())
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

    #region 物品排序
    public void OnClickForSortWeapons()
    {
        SortForItemsType(GeneralEnum.Chef);
    }
    public void OnClickForSortHat()
    {
        SortForItemsType(GeneralEnum.Hat);
    }
    public void OnClickForSortClothes()
    {
        SortForItemsType(GeneralEnum.Clothes);
    }
    public void OnClickForSortShoes()
    {
        SortForItemsType(GeneralEnum.Shoes);
    }
    public void OnClickForSortBook()
    {
        SortForItemsType(GeneralEnum.Book);
    }
    public void OnClickForSortMenu()
    {
        SortForItemsType(GeneralEnum.Menu);
    }
    public void OnClickForSortMedicine()
    {
        SortForItemsType(GeneralEnum.Medicine);
    }
    public void OnClickForSortSkill()
    {
        SortForItemsType(GeneralEnum.SkillBook);
    }
    public void OnClickForSortGift()
    {
        SortForItemsType(GeneralEnum.Gift);
    }
    public void SortForItemsType(GeneralEnum itemsType)
    {
        sortType = itemsType;
        if (sortType == GeneralEnum.Null)
            return;
        List<GeneralEnum> listItemsType = null;
        if (itemsType == GeneralEnum.Chef || itemsType == GeneralEnum.Waiter || itemsType == GeneralEnum.Accost || itemsType == GeneralEnum.Accoutant || itemsType == GeneralEnum.Beater)
        {
            listItemsType = new List<GeneralEnum>() { GeneralEnum.Chef, GeneralEnum.Waiter, GeneralEnum.Accost, GeneralEnum.Accoutant, GeneralEnum.Beater };
        }
        else
        {
            listItemsType = new List<GeneralEnum>() { itemsType };
        }

        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        listItemData = listItemData.OrderByDescending(data =>
        {
            ItemsInfoBean itemsInfoBean = GameItemsHandler.Instance.manager.GetItemsById(data.itemId);
            if (listItemsType.Contains(itemsInfoBean.GetItemsType()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }).ToList();
        gridVertical.RefreshAllCells();
    }
    #endregion

    #region  搜索文本回调
    public void SearchTextStart(string text)
    {
        listItemData = listItemData.OrderByDescending(data =>
        {
            ItemsInfoBean itemsInfoBean = GameItemsHandler.Instance.manager.GetItemsById(data.itemId);
            if (itemsInfoBean.name.Contains(text))
            {
                return true;
            }
            else
            {
                return false;
            }
        }).ToList();
        gridVertical.RefreshAllCells();
    }
    #endregion
}