using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
public class UIGameBackpack : UIBaseOne
{
    public ScrollGridVertical gridVertical;

    public Text tvNull;
    public Button btClearUp;

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
        int itemNumber = uiGameManager.gameDataManager.gameData.listItems.Count;
        if (gridVertical)
        {
            gridVertical.SetCellCount(itemNumber);
        }
        if (itemNumber <= 0)
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
        ItemBean itemBean = uiGameManager.gameDataManager.gameData.listItems[index];
        ItemsInfoBean itemsInfoBean = uiGameManager.gameItemsManager.GetItemsById(itemBean.itemId);
        ItemGameBackpackCpt backpackCpt = itemCell.GetComponent<ItemGameBackpackCpt>();
        backpackCpt.SetData(itemsInfoBean, itemBean);
    }

    public void OnClickForClearUp()
    {
        uiGameManager.gameDataManager.gameData.listItems = uiGameManager.gameDataManager.gameData.listItems.OrderBy(data=> {
            ItemsInfoBean itemsInfoBean = uiGameManager.gameItemsManager.GetItemsById(data.itemId);
            return itemsInfoBean.items_type;
        }).ToList();
        RefreshUI();
    }

}