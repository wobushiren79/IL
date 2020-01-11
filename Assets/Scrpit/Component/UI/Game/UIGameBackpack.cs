using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class UIGameBackpack : UIBaseOne
{
    public GameObject objItemContent;
    public GameObject objItemModel;

    public Text tvNull;

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
        bool hasData = false;
        for (int i = 0; i < uiGameManager.gameDataManager.gameData.listItems.Count; i++)
        {
            ItemBean itemBean = uiGameManager.gameDataManager.gameData.listItems[i];
            ItemsInfoBean itemsInfoBean = uiGameManager.gameItemsManager.GetItemsById(itemBean.itemId);
            if (itemsInfoBean == null)
                continue;
            GameObject objItem = Instantiate(objItemModel, objItemContent.transform);
            objItem.SetActive(true);
            ItemGameBackpackCpt backpackCpt = objItem.GetComponent<ItemGameBackpackCpt>();
            backpackCpt.SetData(itemsInfoBean, itemBean);
            //objItem.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBack).SetDelay(i * 0.05f).From();
            hasData = true;
        }
        if (!hasData)
            tvNull.gameObject.SetActive(true);
        else
            tvNull.gameObject.SetActive(false);
    }

}