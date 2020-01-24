using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class UIGameWorker : BaseUIComponent
{
    public Button btBack;

    public GameObject objListContent;
    public GameObject objItemWorkModle;

    protected ControlHandler controlHandler;
    protected GameDataManager gameDataManager;
    protected AudioHandler audioHandler;

    private void Awake()
    {
        controlHandler = GetUIManager<UIGameManager>().controlHandler;
        gameDataManager = GetUIManager<UIGameManager>().gameDataManager;
        audioHandler = GetUIManager<UIGameManager>().audioHandler;
    }

    private void Start()
    {
        if (btBack != null)
            btBack.onClick.AddListener(OpenMainUI);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        InitData();
        if (controlHandler != null)
            controlHandler.StopControl();
    }

    public override void CloseUI()
    {
        base.CloseUI();
        if (controlHandler != null)
            controlHandler.RestoreControl();
    }

    public void OpenMainUI()
    {
        if (audioHandler != null)
            audioHandler.PlaySound(SoundEnum.ButtonForBack);
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.GameMain));
    }

    public void InitData()
    {
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