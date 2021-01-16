using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using System.Linq;
using System.Collections;

public class UIGameWorker : BaseUIComponent
{
    public Text tvNumber;
    public Button btBack;

    public ScrollGridVertical gridVertical;

    public Button btSortDef;
    public Button btSortLevelUp;
    public Button btSortLoyalty;

    public Button btFilterChef;
    public Button btFilterWaiterCleanBed;
    public Button btFilterWaiterCleanTable;
    public Button btFilterWaiterSend;
    public Button btFilterAccountant;
    public Button btFilterAccostSolicit;
    public Button btFilterAccostGuide;
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
        if (btSortLoyalty != null)
            btSortLoyalty.onClick.AddListener(OnClickForSortLoyalty);

        if (btFilterChef != null)
            btFilterChef.onClick.AddListener(OnClickForChef);
        if (btFilterWaiterCleanBed != null)
            btFilterWaiterCleanBed.onClick.AddListener(OnClickForWaiterCleanBed);
        if (btFilterWaiterCleanTable != null)
            btFilterWaiterCleanTable.onClick.AddListener(OnClickForWaiterCleanTable);
        if (btFilterWaiterSend != null)
            btFilterWaiterSend.onClick.AddListener(OnClickForWaiterSend);

        if (btFilterAccountant != null)
            btFilterAccountant.onClick.AddListener(OnClickForAccountant);

        if (btFilterAccostSolicit != null)
            btFilterAccostSolicit.onClick.AddListener(OnClickForAccostSolicit);
        if (btFilterAccostGuide != null)
            btFilterAccostGuide.onClick.AddListener(OnClickForAccostGuide);

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
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForBack);
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
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
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
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
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

    /// <summary>
    /// 忠诚排序点击
    /// </summary>
    public void OnClickForSortLoyalty()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        this.listCharacterData = this.listCharacterData.OrderBy(
            (data) =>
            {
                return data.attributes.loyal;
            }).ToList();
        InitData();
    }

    public void OnClickForChef()
    {
        OnClickForWorker(WorkerEnum.Chef, WorkerDetilsEnum.ChefForCook);

    }
    public void OnClickForWaiterCleanBed()
    {
        OnClickForWorker(WorkerEnum.Waiter, WorkerDetilsEnum.WaiterForCleanBed);
    }
    public void OnClickForWaiterCleanTable()
    {
        OnClickForWorker(WorkerEnum.Waiter, WorkerDetilsEnum.WaiterForCleanTable);
    }
    public void OnClickForWaiterSend()
    {
        OnClickForWorker(WorkerEnum.Waiter, WorkerDetilsEnum.WaiterForSend);
    }
    public void OnClickForAccountant()
    {
        OnClickForWorker(WorkerEnum.Accountant, WorkerDetilsEnum.AccountantForPay);
    }
    public void OnClickForAccostSolicit()
    {
        OnClickForWorker(WorkerEnum.Accost, WorkerDetilsEnum.AccostForSolicit);
    }
    public void OnClickForAccostGuide()
    {
        OnClickForWorker(WorkerEnum.Accost, WorkerDetilsEnum.AccostForGuide);
    }

    public void OnClickForBeater()
    {
        OnClickForWorker(WorkerEnum.Beater, WorkerDetilsEnum.BeaterForDrive);
    }
    /// <summary>
    /// 是否升级排序点击
    /// </summary>
    public void OnClickForWorker(WorkerEnum worker,WorkerDetilsEnum workerDetils)
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
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
                    if (workerDetils == WorkerDetilsEnum.WaiterForCleanBed && waiterData.isWorkingCleanBed)
                    {
                        workNumber++;
                    }
                    if (workerDetils == WorkerDetilsEnum.WaiterForCleanTable && waiterData.isWorkingForCleanTable)
                    {
                        workNumber++;
                    }
                    if (workerDetils == WorkerDetilsEnum.WaiterForSend && waiterData.isWorkingForSend)
                    {
                        workNumber++;
                    }
                }
                else if (worker == WorkerEnum.Accost)
                {
                    CharacterWorkerForAccostBean accostData = workerData as CharacterWorkerForAccostBean;
                    if (workerDetils == WorkerDetilsEnum.AccostForSolicit && accostData.isWorkingForSolicit)
                    {
                        workNumber++;
                    }
                    if (workerDetils == WorkerDetilsEnum.AccostForGuide && accostData.isWorkingForGuide)
                    {
                        workNumber++;
                    }
                }
                return workNumber;
            }).ToList();
        InitData();
    }
}