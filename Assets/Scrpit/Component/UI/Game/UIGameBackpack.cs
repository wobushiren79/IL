using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
public class UIGameBackpack : UIBaseOne
{
    public GameObject objItemContent;
    public GameObject objItemModel;

    public Text tvNull;

    public override void OpenUI()
    {
        base.OpenUI();
        StartCoroutine(CreateBackpackData());
    }

    public IEnumerator CreateBackpackData()
    {
        CptUtil.RemoveChildsByActive(objItemContent.transform);
        if (uiGameManager.gameItemsManager == null || uiGameManager.gameDataManager == null)
            yield return null;
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
            if (i % ProjectConfigInfo.ITEM_REFRESH_NUMBER == 0)
                yield return new WaitForEndOfFrame();
            hasData = true;
        }
        if (!hasData)
            tvNull.gameObject.SetActive(true);
        else
            tvNull.gameObject.SetActive(false);
    }


}