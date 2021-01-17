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
        if (uiGameManager.gameTimeHandler != null)
            uiGameManager.gameTimeHandler.SetTimeStatus(false);
        RefreshUI();
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        listItemData.Clear();
        listItemData.AddRange(uiGameManager.gameDataManager.gameData.listItems);
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

    public void OnClickForClearUp()
    {
        uiGameManager.gameDataManager.gameData.listItems = uiGameManager.gameDataManager.gameData.listItems.OrderBy(data=> {
            ItemsInfoBean itemsInfoBean = GameItemsHandler.Instance.manager.GetItemsById(data.itemId);
            return itemsInfoBean.items_type;
        }).ToList();
        RefreshUI();
    }

    #region  搜索文本回调
    public void SearchTextStart(string text)
    {
        listItemData = listItemData.OrderByDescending(data => {
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