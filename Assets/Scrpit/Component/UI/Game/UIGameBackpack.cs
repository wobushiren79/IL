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
        if (textSearchView)
            textSearchView.SetCallBack(this);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        GameTimeHandler.Instance.SetTimeStatus(false);
        RefreshUI();
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        listItemData.Clear();
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        listItemData.AddRange(gameData.listItems);
        if (gridVertical)
        {
            gridVertical.SetCellCount(listItemData.Count);
        }
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




    /// <summary>
    /// 招募NPC排序点击
    /// </summary>
    public void OnClickForSortSpecial()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);

    }

    public void SortForItemsType(List<GeneralEnum> listItemsType)
    {
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