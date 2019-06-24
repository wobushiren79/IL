using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIGameWorker : BaseUIComponent
{
    public Button btBack;

    public GameDataManager gameDataManager;

    public GameObject objListContent;
    public GameObject objItemWorkModle;

    private void Start()
    {
        if (btBack != null)
            btBack.onClick.AddListener(OpenMainUI);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        InitData();
    }

    public void OpenMainUI()
    {
        uiManager.OpenUIAndCloseOtherByName("Main");
    }

    public void InitData()
    {
        if (gameDataManager == null)
            return;
        List<CharacterBean> listData = new List<CharacterBean>();
        listData.Add(gameDataManager.gameData.userCharacter);
        listData.AddRange(gameDataManager.gameData.workCharacterList);
        CptUtil.RemoveChildsByActive(objListContent.transform);
        for (int i = 0; i < listData.Count; i++)
        {
            CharacterBean itemData = listData[i];
            CreateWorkerItem(itemData);
        }
    }

    public void CreateWorkerItem(CharacterBean characterData)
    {
        if (objListContent == null || objItemWorkModle == null)
            return;
        GameObject objWorkerItem = Instantiate(objItemWorkModle, objListContent.transform);
        objWorkerItem.SetActive(true);
        ItemGameWorkerCpt workerItem = objWorkerItem.GetComponent<ItemGameWorkerCpt>();
        if (workerItem != null)
            workerItem.SetData(characterData);
    }
}