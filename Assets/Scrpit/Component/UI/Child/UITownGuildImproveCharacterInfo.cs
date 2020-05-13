using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class UITownGuildImproveCharacterInfo : BaseUIChildComponent<UITownGuildImprove>, StoreInfoManager.ICallBack
{
    //游戏数据
    public GameDataBean gameData;

    //模型和容器
    public GameObject objImproveContent;
    public GameObject objImproveModel;
    public GameObject objImproveEmpty;
    
    private List<StoreInfoBean> mListLevelInfo;

    protected StoreInfoManager storeInfoManager;

    private void Awake()
    {
        storeInfoManager = Find<StoreInfoManager>(ImportantTypeEnum.StoreInfoManager);
    }


    public void InitData(GameDataBean gameData)
    {
        this.gameData = gameData;
        storeInfoManager.SetCallBack(this);
        storeInfoManager.GetStoreInfoForGuildImprove();
    }

    /// <summary>
    /// 创建角色提升item
    /// </summary>
    public void CreateCharacterImproveItem(
        WorkerEnum workerType, 
        CharacterBean characterData, 
        CharacterWorkerBaseBean workerData, 
        StoreInfoBean levelData)
    {
        GameObject objItem = Instantiate(objImproveContent, objImproveModel);
        ItemTownGuildImproveCharacterCpt improveCharacterCpt = objItem.GetComponent<ItemTownGuildImproveCharacterCpt>();
        improveCharacterCpt.SetData(workerType, characterData, workerData, levelData);
    }

    /// <summary>
    /// 检测是否要升级
    /// </summary>
    /// <param name="workerData"></param>
    /// <returns></returns>
    private bool CheckCanImprove(CharacterWorkerBaseBean workerData, out StoreInfoBean levelInfo)
    {
        workerData.GetWorkerExp(out long nextLevelExp, out long currentExp, out float levelProportion);

        //获取升级数据
        levelInfo = null;
        foreach (StoreInfoBean itemLevel in mListLevelInfo)
        {
            if (itemLevel.mark_type == workerData.workerLevel + 1)
            {
                levelInfo = itemLevel;
            }
        }
        //判断是否可以升级
        if (currentExp >= nextLevelExp)
        {
            //可以升级
            return true;
        }
        else
        {
            //不可以升级
            return false;
        }
    }


    #region 商店数据回调
    public void GetStoreInfoSuccess(StoreTypeEnum type, List<StoreInfoBean> listData)
    {
        mListLevelInfo = listData;
        objImproveEmpty.SetActive(false);
        List<CharacterBean> listCharacterData = gameData.GetAllCharacterData();
        CptUtil.RemoveChildsByActive(objImproveContent.transform);

        int itemNumer = 0;

        foreach (CharacterBean itemData in listCharacterData)
        {
            if (CheckCanImprove(itemData.baseInfo.chefInfo, out StoreInfoBean chefLevelInfo))
            {
                CreateCharacterImproveItem(WorkerEnum.Chef, itemData, itemData.baseInfo.chefInfo, chefLevelInfo);
                itemNumer++;
            }
            if (CheckCanImprove(itemData.baseInfo.waiterInfo, out StoreInfoBean waiterLevelInfo))
            {
                CreateCharacterImproveItem(WorkerEnum.Waiter, itemData, itemData.baseInfo.waiterInfo, waiterLevelInfo);
                itemNumer++;
            }
            if (CheckCanImprove(itemData.baseInfo.accountantInfo, out StoreInfoBean accountingLevelInfo))
            {
                CreateCharacterImproveItem(WorkerEnum.Accountant, itemData, itemData.baseInfo.accountantInfo, accountingLevelInfo);
                itemNumer++;
            }
            if (CheckCanImprove(itemData.baseInfo.accostInfo, out StoreInfoBean accostLevelInfo))
            {
                CreateCharacterImproveItem(WorkerEnum.Accost, itemData, itemData.baseInfo.accostInfo, accostLevelInfo);
                itemNumer++;
            }
            if (CheckCanImprove(itemData.baseInfo.beaterInfo, out StoreInfoBean beaterLevelInfo))
            {
                CreateCharacterImproveItem(WorkerEnum.Beater, itemData, itemData.baseInfo.beaterInfo, beaterLevelInfo);
                itemNumer++;
            }
        }
        if (itemNumer <= 0)
        {
            objImproveEmpty.SetActive(true);
            return;
        }
    }
    #endregion
}