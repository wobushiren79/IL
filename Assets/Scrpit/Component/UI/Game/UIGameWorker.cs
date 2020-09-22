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

    public ScrollGridVertical gridVertical;

    public Button btSortDef;
    public Button btSortLevelUp;

    public Button btFilterChef;
    public Button btFilterWaiter;
    public Button btFilterAccountant;
    public Button btFilterAccost;
    public Button btFilterBeater;

    public List<CharacterBean> listCharacterData = new List<CharacterBean>();

    public override void Awake()
    {
        base.Awake();
        if (gridVertical != null)
            gridVertical.AddCellListener(OnCellForItem);
    }

    private void Start()
    {
        if (btBack != null)
            btBack.onClick.AddListener(OpenMainUI);
        if (btSortDef != null)
            btSortDef.onClick.AddListener(OnClickForSortDef);
        if (btSortLevelUp != null)
            btSortLevelUp.onClick.AddListener(OnClickForSortLevelUp);

        if (btFilterChef != null)
            btFilterChef.onClick.AddListener(OnClickForChef);
        if (btFilterWaiter != null)
            btFilterWaiter.onClick.AddListener(OnClickForWaiter);
        if (btFilterAccountant != null)
            btFilterAccountant.onClick.AddListener(OnClickForAccountant);
        if (btFilterAccost != null)
            btFilterAccost.onClick.AddListener(OnClickForAccost);
        if (btFilterBeater != null)
            btFilterBeater.onClick.AddListener(OnClickForBeater);

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
        gridVertical.RefreshAllCells();
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        for (int  i = 0 ; i< listCharacterData.Count;i++)
        {
            CharacterBean characterItem=  listCharacterData[i];
            if (characterItem == null|| characterItem.baseInfo.characterId == null)
            {
                listCharacterData.RemoveAt(i);
                i--;
            }
        }
        InitData();
    }


    public void InitUI()
    {
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
        gridVertical.SetCellCount(listCharacterData.Count);

    }

    /// <summary>
    /// 列表Item回调
    /// </summary>
    /// <param name="itemCell"></param>
    public void OnCellForItem(ScrollGridCell itemCell)
    {
        CharacterBean characterData = listCharacterData[itemCell.index];
        ItemGameWorkerCpt workerItem = itemCell.GetComponent<ItemGameWorkerCpt>();
        if (workerItem != null)
        {
            workerItem.SetData(characterData);
        }
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

    public void OnClickForChef()
    {
        OnClickForWorker(WorkerEnum.Chef);

    }
    public void OnClickForWaiter()
    {
        OnClickForWorker(WorkerEnum.Waiter);
    }
    public void OnClickForAccountant()
    {
        OnClickForWorker(WorkerEnum.Accountant);
    }
    public void OnClickForAccost()
    {
        OnClickForWorker(WorkerEnum.Accost);
    }
    public void OnClickForBeater()
    {
        OnClickForWorker(WorkerEnum.Beater);
    }
    /// <summary>
    /// 是否升级排序点击
    /// </summary>
    public void OnClickForWorker(WorkerEnum worker)
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        this.listCharacterData = this.listCharacterData.OrderByDescending(
            (data) =>
            {
                int workNumber = 0;
                CharacterWorkerBaseBean workerData = data.baseInfo.GetWorkerInfoByType(worker);
                if (worker== WorkerEnum.Chef|| worker == WorkerEnum.Beater|| worker == WorkerEnum.Accountant)
                {
                    if (workerData.isWorking)
                    {
                        workNumber++;
                    }
                }
                else if (worker == WorkerEnum.Waiter)
                {
                    CharacterWorkerForWaiterBean waiterData= workerData as CharacterWorkerForWaiterBean;
                    if (waiterData.isWorkingCleanBed)
                    {
                        workNumber++;
                    }
                    if (waiterData.isWorkingForCleanTable)
                    {
                        workNumber++;
                    }
                    if (waiterData.isWorkingForSend)
                    {
                        workNumber++;
                    }
                }
                else if (worker == WorkerEnum.Accost)
                {
                    CharacterWorkerForAccostBean accostData = workerData as CharacterWorkerForAccostBean;
                    if (accostData.isWorkingForSolicit)
                    {
                        workNumber++;
                    }
                    if (accostData.isWorkingForGuide)
                    {
                        workNumber++;
                    }
                }
                return workNumber;
            }).ToList();
        InitData();
    }
}