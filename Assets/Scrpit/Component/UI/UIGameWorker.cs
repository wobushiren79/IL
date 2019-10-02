using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIGameWorker : BaseUIComponent
{
    public Button btBack;

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
        if (GetUIMananger<UIGameManager>().controlHandler != null)
            GetUIMananger<UIGameManager>().controlHandler.StopControl();
    }

    public override void CloseUI()
    {
        base.CloseUI();
        if (GetUIMananger<UIGameManager>().controlHandler != null)
            GetUIMananger<UIGameManager>().controlHandler.RestoreControl();
    }

    public void OpenMainUI()
    {
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMain));
    }

    public void InitData()
    {
        GameDataManager gameDataManager = GetUIMananger<UIGameManager>().gameDataManager;
        if (gameDataManager == null)
            return;  
        List<CharacterBean> listData = gameDataManager.gameData.GetAllCharacterData();
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