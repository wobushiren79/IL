using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class UIGameEquip : BaseUIComponent
{
    public Button btBack;

    public GameObject objItemContent;
    public GameObject objItemModel;

    public GameDataManager gameDataManager;
    public GameItemsManager gameItemsManager;

    private void Start()
    {
        if (btBack != null)
            btBack.onClick.AddListener(OpenWorkUI);
    }

    public void SetCharacterData(CharacterBean characterData)
    {

    }

    public override void OpenUI()
    {
        base.OpenUI();
        CreateBackpackData();
    }


    public void OpenWorkUI()
    {
        uiManager.OpenUIAndCloseOtherByName("Worker");
    }

    public void CreateBackpackData()
    {
        CptUtil.RemoveChildsByActive(objItemContent.transform);
        if (gameItemsManager == null || gameDataManager == null)
            return;
        for (int i = 0; i < gameDataManager.gameData.itemsList.Count; i++)
        {
            ItemBean itemBean = gameDataManager.gameData.itemsList[i];
            ItemsInfoBean itemsInfoBean = gameItemsManager.GetItemsById(itemBean.itemId);
            if (itemsInfoBean == null)
                continue;
            if (itemsInfoBean.items_type != 1
                && itemsInfoBean.items_type != 2
                && itemsInfoBean.items_type != 3
                && itemsInfoBean.items_type != 11)
                continue;
            GameObject objItem = Instantiate(objItemModel, objItemContent.transform);
            objItem.SetActive(true);
            ItemGameBackpackCpt backpackCpt = objItem.GetComponent<ItemGameBackpackCpt>();
            backpackCpt.SetData(itemsInfoBean, itemBean);
            objItem.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBack).SetDelay(i * 0.05f).From();
        }
    }
}