using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class UIGameWorker : UIGameComponent
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
    }

    public void OpenMainUI()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForBack);
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMain));
    }

    public void InitData()
    {
        if (uiGameManager.gameDataManager == null)
            return;
        List<CharacterBean> listData = uiGameManager.gameDataManager.gameData.GetAllCharacterData();
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