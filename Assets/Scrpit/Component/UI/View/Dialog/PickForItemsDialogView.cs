using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
public class PickForItemsDialogView : DialogView, ItemGameBackpackPickCpt.ICallBack
{
    protected GameDataManager gameDataManager;
    protected GameItemsManager gameItemsManager;

    public ScrollGridVertical gridVertical;

    public Text tvNull;

    private ItemBean mSelectedItems;
    private ItemsInfoBean mSelectedItemsInfo;

    protected PopupItemsSelection.SelectionTypeEnum itemSelectionType;
    protected List<GeneralEnum> listPickType = new List<GeneralEnum>();
    protected List<ItemBean> listItems = new List<ItemBean>();

    public override void Awake()
    {
        base.Awake();
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        gameItemsManager = Find<GameItemsManager>(ImportantTypeEnum.GameItemsManager);

        if (gridVertical != null)
            gridVertical.AddCellListener(OnCellForItems);
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
        for (int i = 0; i < listItems.Count; i++)
        {
            ItemBean itemData=listItems[i];
            if (itemData==null|| itemData.itemNumber==0)
            {
                listItems.RemoveAt(i);
                i--;
            }
        }
        gridVertical.SetCellCount(listItems.Count);
        gridVertical.RefreshAllCells();
    }

    public void OnCellForItems(ScrollGridCell itemCell)
    {
        ItemBean itemData= listItems[itemCell.index];
        ItemsInfoBean itemsInfo = gameItemsManager.GetItemsById(itemData.itemId);
        ItemGameBackpackPickCpt itemBackpack = itemCell.GetComponent<ItemGameBackpackPickCpt>();
        itemBackpack.SetCallBack(this);
        itemBackpack.SetData(itemsInfo, itemData);
        itemBackpack.SetSelectionType(itemSelectionType);
    }

    public void SetData(List<GeneralEnum> listPickType, PopupItemsSelection.SelectionTypeEnum  itemSelectionType)
    {
        this.listPickType = listPickType;
        this.itemSelectionType = itemSelectionType;

    }

    public void CreateItems()
    {
        List<ItemBean> listAllItems = gameDataManager.gameData.listItems;
        listItems.Clear();
        if (listAllItems == null)
            return;
        bool hasData = false;
        for(int i=0;i< listAllItems.Count; i++)
        {
            ItemBean itemData = listAllItems[i];
            ItemsInfoBean itemsInfo = gameItemsManager.GetItemsById(itemData.itemId);
            if (!CheckUtil.ListIsNull(listPickType))
            {
                //如果没有该类型
                if (!listPickType.Contains(itemsInfo.GetItemsType()))
                {
                    continue;
                }
            }
            listItems.Add(itemData);
            hasData = true;
        }

        if (!hasData)
            tvNull.gameObject.SetActive(true);
        else
            tvNull.gameObject.SetActive(false);

        gridVertical.SetCellCount(listItems.Count);
    }

    /// <summary>
    /// 获取选择的物品
    /// </summary>
    /// <param name="itemsInfo"></param>
    /// <param name="itemBean"></param>
    public void GetSelectedItems(out ItemsInfoBean itemsInfo,out ItemBean itemBean)
    {
        itemsInfo = mSelectedItemsInfo;
        itemBean = mSelectedItems;
    }

    #region 选择回调
    /// <summary>
    /// 选择赠送
    /// </summary>
    /// <param name="itemsInfo"></param>
    public void ItemsSelection(ItemsInfoBean itemsInfo,ItemBean itemBean)
    {
        mSelectedItemsInfo = itemsInfo;
        mSelectedItems = itemBean;
        SubmitOnClick();
    }
    #endregion
}