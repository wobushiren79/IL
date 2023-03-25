using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class UIGameBackpack : UIBaseOne, TextSearchView.ICallBack
{
    public ScrollGridVertical gridVertical;

    public TextSearchView textSearchView;
    public Text tvNull;
    public Button btClearUp;

    public Button ui_ItemShowSortDetails_Weapons;
    public Button ui_ItemShowSortDetails_Hat;
    public Button ui_ItemShowSortDetails_Clothes;
    public Button ui_ItemShowSortDetails_Shoes;
    public Button ui_ItemShowSortDetails_Book;
    public Button ui_ItemShowSortDetails_Menu;
    public Button ui_ItemShowSortDetails_Medicine;
    public Button ui_ItemShowSortDetails_Skill;
    public Button ui_ItemShowSortDetails_Gift;

    protected List<ItemBean> listItemData = new List<ItemBean>();

    protected GeneralEnum sortType = GeneralEnum.Null;
    public override void Awake()
    {
        base.Awake();
        if (btClearUp)
        {
            btClearUp.onClick.AddListener(OnClickForClearUp);
        }
        if (gridVertical)
        {
            gridVertical.AddCellListener(OnCellForItems);
        }
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
        if (textSearchView)
            textSearchView.SetCallBack(this);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        GameTimeHandler.Instance.SetTimeStatus(false);
        RefreshUI();
    }

    public override void RefreshUI(bool isOpenInit = false)
    {
        base.RefreshUI(isOpenInit);
        listItemData.Clear();
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        listItemData.AddRange(gameData.listItems);
        if (gridVertical)
        {
            gridVertical.SetCellCount(listItemData.Count);
        }
        SortForItemsType(sortType);
        if (listItemData.Count <= 0)
            tvNull.gameObject.SetActive(true);
        else
            tvNull.gameObject.SetActive(false);
    }

    /// <summary>
    /// 数据回掉
    /// </summary>
    /// <param name="itemCell"></param>
    public void OnCellForItems(ScrollGridCell itemCell)
    {
        int index = itemCell.index;
        ItemBean itemBean = listItemData[index];
        ItemsInfoBean itemsInfoBean = GameItemsHandler.Instance.manager.GetItemsById(itemBean.itemId);
        ItemGameBackpackCpt backpackCpt = itemCell.GetComponent<ItemGameBackpackCpt>();
        backpackCpt.SetData(itemsInfoBean, itemBean);
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
        if (itemsType== GeneralEnum.Chef|| itemsType == GeneralEnum.Waiter || itemsType == GeneralEnum.Accost || itemsType == GeneralEnum.Accoutant || itemsType == GeneralEnum.Beater)
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

    public void OnClickForClearUp()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        gameData.listItems = gameData.listItems
            .OrderBy(data =>
        {
            ItemsInfoBean itemsInfoBean = GameItemsHandler.Instance.manager.GetItemsById(data.itemId);
            return itemsInfoBean.items_type;
        })
            .ToList();
        RefreshUI();
    }

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