using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using System.Linq;
using System.Collections;

public class UIGameWorker : UIGameComponent
{
    public Text tvNumber;
    public Button btBack;

    public GameObject objListContent;
    public GameObject objItemWorkModle;

    public Button btSortDef;
    public Button btSortLevelUp;
    public Button btSortWorker;

    public List<CharacterBean> listCharacterData = new List<CharacterBean>();

    private void Start()
    {
        if (btBack != null)
            btBack.onClick.AddListener(OpenMainUI);
        if (btSortDef != null)
            btSortDef.onClick.AddListener(OnClickForSortDef);
        if (btSortLevelUp != null)
            btSortLevelUp.onClick.AddListener(OnClickForSortLevelUp);
        if (btSortWorker != null)
            btSortWorker.onClick.AddListener(OnClickForSortWorker);
    }

    private void Update()
    {
        if (uiGameManager.gameDataManager != null && tvNumber != null)
        {
            tvNumber.text = uiGameManager.gameDataManager.gameData.listWorkerCharacter.Count + "/" + uiGameManager.gameDataManager.gameData.workerNumberLimit;
        }
    }

    public override void OpenUI()
    {
        base.OpenUI();
        workerForSort = WorkerEnum.Chef;
        List<CharacterBean> listData = uiGameManager.gameDataManager.gameData.GetAllCharacterData();
        listCharacterData.Clear();
        listCharacterData.AddRange(listData);
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
        StopAllCoroutines();
        CptUtil.RemoveChildsByActive(objListContent.transform);
        StartCoroutine(CoroutineForCreateWorkerList());

    }

    public IEnumerator CoroutineForCreateWorkerList()
    {
        for (int i = 0; i < listCharacterData.Count; i++)
        {
            CharacterBean itemData = listCharacterData[i];
            CreateWorkerItem(itemData);
            yield return new WaitForEndOfFrame();
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

    /// <summary>
    /// 默认排序点击
    /// </summary>
    public void OnClickForSortDef()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        List<CharacterBean> listData = uiGameManager.gameDataManager.gameData.GetAllCharacterData();
        listCharacterData.Clear();
        listCharacterData.AddRange(listData);
        InitData();
    }

    /// <summary>
    /// 是否升级排序点击
    /// </summary>
    public void OnClickForSortLevelUp()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        this.listCharacterData = this.listCharacterData.OrderByDescending(
            (data) =>
            { 
                int levelupNumber = 0;
                List<CharacterWorkerBaseBean> listWorker = data.baseInfo.GetAllWorkerInfo();
                foreach (CharacterWorkerBaseBean itemData in listWorker)
                {
                    itemData.GetWorkerExp(out long nextLevelExp, out long currentExp, out float levePro);
                    if (currentExp >= nextLevelExp)
                    {
                        levelupNumber++;
                    }
                }
                return levelupNumber;
            }).ToList();
        InitData();
    }

    public WorkerEnum workerForSort = WorkerEnum.Chef;
    /// <summary>
    /// 是否升级排序点击
    /// </summary>
    public void OnClickForSortWorker()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        this.listCharacterData = this.listCharacterData.OrderByDescending(
            (data) =>
            {
                int worker = 0;
                CharacterWorkerBaseBean workerData =  data.baseInfo.GetWorkerInfoByType(workerForSort);
                if (workerData.isWorking)
                {
                    worker++;
                }
                return worker;
            }).ToList();
        InitData();
        int intWorker= (int)workerForSort + 1;
        if (intWorker > 5)
        {
            intWorker = 1;
        }
        workerForSort = (WorkerEnum)intWorker;
    }
}