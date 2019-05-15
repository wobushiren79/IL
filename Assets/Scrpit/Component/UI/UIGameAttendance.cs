using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIGameAttendance : BaseUIComponent
{
    public Button btSubmit;

    public GameDataManager gameDataManager;

    public GameObject objListContent;
    public GameObject objItemWorkModle;

    private void Start()
    {
        if (btSubmit != null)
            btSubmit.onClick.AddListener(StartWork);
        InitData();
    }

    public void StartWork()
    {

    }

    public void InitData()
    {
        if (gameDataManager == null)
            return;
        List<CharacterBean> listData = new List<CharacterBean>();
        listData.Add(gameDataManager.gameData.userCharacter);
        listData.AddRange(gameDataManager.gameData.workCharacterList);
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
        ItemGameAttendanceCpt workerItem = objWorkerItem.GetComponent<ItemGameAttendanceCpt>();
        if (workerItem != null)
            workerItem.SetData(characterData);
    }
}