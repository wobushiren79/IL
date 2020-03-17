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

    protected PopupItemsSelection.SelectionTypeEnum itemSelectionType;
    protected List<GeneralEnum> listPickType = new List<GeneralEnum>();

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

    public void SetData(List<GeneralEnum> listPickType, PopupItemsSelection.SelectionTypeEnum  itemSelectionType)
    {
        this.listPickType = listPickType;
        this.itemSelectionType = itemSelectionType;
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
            ItemsInfoBean itemsInfo = gameItemsManager.GetItemsById(itemData.itemId);
            if (!CheckUtil.ListIsNull(listPickType))
            {
                //如果没有该类型
                if(!listPickType.Contains(itemsInfo.GetItemsType()))
                {
                    continue;
                }
            }

            GameObject objItem = Instantiate(objItemsContainer, objItemsModel);
            ItemGameBackpackPickCpt itemBackpack = objItem.GetComponent<ItemGameBackpackPickCpt>();
            itemBackpack.SetCallBack(this);    
            itemBackpack.SetData(itemsInfo, itemData);
            itemBackpack.SetSelectionType(itemSelectionType);
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
    public void ItemsSelection(ItemsInfoBean itemsInfo,ItemBean itemBean)
    {
        mSelectedItemsInfo = itemsInfo;
        mSelectedItems = itemBean;
        SubmitOnClick();
    }
    #endregion
}