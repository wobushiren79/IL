using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
public class PickForItemsDialogView : DialogView, ItemGameBackpackPickCpt.ICallBack
{
    protected GameDataManager gameDataManager;
    protected GameItemsManager gameItemsManager;

    public GameObject objItemsContainer;
    public GameObject objItemsModel;

    public Text tvNull;

    private ItemBean mSelectedItems;
    private ItemsInfoBean mSelectedItemsInfo;


    public override void Awake()
    {
        base.Awake();
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        gameItemsManager = Find<GameItemsManager>(ImportantTypeEnum.GameItemsManager);
    }

    public override void InitData()
    {
        base.InitData();
        CreateItems();
    }

    public void CreateItems()
    {
        CptUtil.RemoveChildsByActive(objItemsContainer);
        List<ItemBean> listItems = gameDataManager.gameData.listItems;
        if (listItems == null)
            return;
        bool hasData = false;
        foreach (ItemBean itemData in listItems)
        {
            GameObject objItem = Instantiate(objItemsContainer, objItemsModel);
            ItemGameBackpackPickCpt itemBackpack = objItem.GetComponent<ItemGameBackpackPickCpt>();
            itemBackpack.SetCallBack(this);
            ItemsInfoBean itemsInfo = gameItemsManager.GetItemsById(itemData.itemId);
            itemBackpack.SetData(itemsInfo, itemData);
            hasData = true;
        }
        if (!hasData)
            tvNull.gameObject.SetActive(true);
        else
            tvNull.gameObject.SetActive(false);
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
    public void ItemsSelectionForGift(ItemsInfoBean itemsInfo,ItemBean itemBean)
    {
        mSelectedItemsInfo = itemsInfo;
        mSelectedItems = itemBean;
        SubmitOnClick();
    }
    #endregion
}