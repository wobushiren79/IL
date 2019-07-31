using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using DG.Tweening;

public class UIGameBackpack : UIBaseOne
{
    public GameObject objItemContent;
    public GameObject objItemModel;

    public override void OpenUI()
    {
        base.OpenUI();
        CreateBackpackData();
    }

    public void CreateBackpackData()
    {
        CptUtil.RemoveChildsByActive(objItemContent.transform);
        UIGameManager uiGameManager = GetUIMananger<UIGameManager>();
        if (uiGameManager.gameItemsManager == null || uiGameManager.gameDataManager == null)
            return;
        for (int i = 0; i < uiGameManager.gameDataManager.gameData.itemsList.Count; i++)
        {
            ItemBean itemBean = uiGameManager.gameDataManager.gameData.itemsList[i];
            ItemsInfoBean itemsInfoBean = uiGameManager.gameItemsManager.GetItemsById(itemBean.itemId);
            if (itemsInfoBean == null)
                continue;
            GameObject objItem = Instantiate(objItemModel, objItemContent.transform);
            objItem.SetActive(true);
            ItemGameBackpackCpt backpackCpt = objItem.GetComponent<ItemGameBackpackCpt>();
            backpackCpt.SetSelectionBox(GetUIMananger<UIGameManager>().itemsSelectionBox);
            backpackCpt.SetPopupShowView(GetUIMananger<UIGameManager>().infoItemsPopup);
            backpackCpt.SetData(itemsInfoBean, itemBean);
            objItem.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBack).SetDelay(i * 0.05f).From();
        }
    }

}