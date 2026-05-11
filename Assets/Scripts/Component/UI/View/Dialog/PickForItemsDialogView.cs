using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class PickForItemsDialogView : DialogView, ItemGameBackpackPickCpt.ICallBack
{
    public ScrollGridVertical gridVertical;

    public Text tvNull;

    public Button ui_ItemShowSortDetails_Weapons;
    public Button ui_ItemShowSortDetails_Hat;
    public Button ui_ItemShowSortDetails_Clothes;
    public Button ui_ItemShowSortDetails_Shoes;
    public Button ui_ItemShowSortDetails_Book;
    public Button ui_ItemShowSortDetails_Menu;
    public Button ui_ItemShowSortDetails_Medicine;
    public Button ui_ItemShowSortDetails_Skill;
    public Button ui_ItemShowSortDetails_Gift;

    private ItemBean mSelectedItems;
    private ItemsInfoBean mSelectedItemsInfo;

    protected ItemsSelectionDialogView.SelectionTypeEnum itemSelectionType;
    protected List<GeneralEnum> listPickType = new List<GeneralEnum>();
    protected List<ItemBean> listItemData = new List<ItemBean>();

    protected GeneralEnum sortType = GeneralEnum.Null;

    public override void Awake()
    {
        base.Awake();

        if (gridVertical != null)
            gridVertical.AddCellListener(OnCellForItems);

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

    public override void InitData()
    {
        base.InitData();
        CreateItems();
    }

    /// <summary>
    /// 刷新UI
    /// </summary>
    public void RefreshUI()
    {
        for (int i = 0; i < listItemData.Count; i++)
        {
            ItemBean itemData = listItemData[i];
            if (itemData == null || itemData.itemNumber == 0)
            {
                listItemData.RemoveAt(i);
                i--;
            }
        }
        gridVertical.SetCellCount(listItemData.Count);
        gridVertical.RefreshAllCells();
    }

    public void OnCellForItems(ScrollGridCell itemCell)
    {
        ItemBean itemData = listItemData[itemCell.index];
        ItemsInfoBean itemsInfo = GameItemsHandler.Instance.manager.GetItemsById(itemData.itemId);
        ItemGameBackpackPickCpt itemBackpack = itemCell.GetComponent<ItemGameBackpackPickCpt>();
        itemBackpack.SetCallBack(this);
        itemBackpack.SetData(itemsInfo, itemData);
        itemBackpack.SetSelectionType(itemSelectionType);
    }

    public void SetData(List<GeneralEnum> listPickType, ItemsSelectionDialogView.SelectionTypeEnum itemSelectionType)
    {
        this.listPickType = listPickType;
        this.itemSelectionType = itemSelectionType;
        //设置排序按钮
        if (!listPickType.IsNull())
        {
            if (!listPickType.Contains(GeneralEnum.Chef)
                && !listPickType.Contains(GeneralEnum.Waiter)
                && !listPickType.Contains(GeneralEnum.Accost)
                && !listPickType.Contains(GeneralEnum.Accoutant)
                && !listPickType.Contains(GeneralEnum.Beater))
            {
                ui_ItemShowSortDetails_Weapons.gameObject.SetActive(false);
            }
            if (!listPickType.Contains(GeneralEnum.Hat))
            {
                ui_ItemShowSortDetails_Hat.gameObject.SetActive(false);
            }
            if (!listPickType.Contains(GeneralEnum.Clothes))
            {
                ui_ItemShowSortDetails_Clothes.gameObject.SetActive(false);
            }
            if (!listPickType.Contains(GeneralEnum.Shoes))
            {
                ui_ItemShowSortDetails_Shoes.gameObject.SetActive(false);
            }
            if (!listPickType.Contains(GeneralEnum.Book))
            {
                ui_ItemShowSortDetails_Book.gameObject.SetActive(false);
            }
            if (!listPickType.Contains(GeneralEnum.Menu))
            {
                ui_ItemShowSortDetails_Menu.gameObject.SetActive(false);
            }
            if (!listPickType.Contains(GeneralEnum.Medicine))
            {
                ui_ItemShowSortDetails_Medicine.gameObject.SetActive(false);
            }
            if (!listPickType.Contains(GeneralEnum.SkillBook))
            {
                ui_ItemShowSortDetails_Skill.gameObject.SetActive(false);
            }
            if (!listPickType.Contains(GeneralEnum.Gift))
            {
                ui_ItemShowSortDetails_Gift.gameObject.SetActive(false);
            }
        }
    }

    public void CreateItems()
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        List<ItemBean> listAllItems = gameData.listItems;
        listItemData.Clear();
        if (listAllItems == null)
            return;
        bool hasData = false;
        for (int i = 0; i < listAllItems.Count; i++)
        {
            ItemBean itemData = listAllItems[i];
            ItemsInfoBean itemsInfo = GameItemsHandler.Instance.manager.GetItemsById(itemData.itemId);
            if (!listPickType.IsNull())
            {
                //如果没有该类型
                if (!listPickType.Contains(itemsInfo.GetItemsType()))
                {
                    continue;
                }
            }
            listItemData.Add(itemData);
            hasData = true;
        }

        if (!hasData)
            tvNull.gameObject.SetActive(true);
        else
            tvNull.gameObject.SetActive(false);
        SortForItemsType(sortType);
        gridVertical.SetCellCount(listItemData.Count);
    }

    /// <summary>
    /// 获取选择的物品
    /// </summary>
    /// <param name="itemsInfo"></param>
    /// <param name="itemBean"></param>
    public void GetSelectedItems(out ItemsInfoBean itemsInfo, out ItemBean itemBean)
    {
        itemsInfo = mSelectedItemsInfo;
        itemBean = mSelectedItems;
    }

    #region 选择回调
    /// <summary>
    /// 选择赠送
    /// </summary>
    /// <param name="itemsInfo"></param>
    public void ItemsSelection(ItemsInfoBean itemsInfo, ItemBean itemBean)
    {
        mSelectedItemsInfo = itemsInfo;
        mSelectedItems = itemBean;
        SubmitOnClick();
    }
    #endregion

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
}