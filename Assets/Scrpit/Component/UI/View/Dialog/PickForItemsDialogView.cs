using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PickForItemsDialogView : DialogView
{
    protected GameDataManager gameDataManager;
    protected GameItemsManager gameItemsManager;

    public GameObject objItemsContainer;
    public GameObject objItemsModel;

    private void Awake()
    {
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
        foreach (ItemBean itemData in listItems)
        {
            GameObject objItem = Instantiate(objItemsContainer, objItemsModel);
            ItemGameBackpackCpt itemBackpack = objItem.GetComponent<ItemGameBackpackCpt>();
            ItemsInfoBean itemsInfo = gameItemsManager.GetItemsById(itemData.itemId);
            itemBackpack.SetData(itemsInfo, itemData);
        }
    }
}