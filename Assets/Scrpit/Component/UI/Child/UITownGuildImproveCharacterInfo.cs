using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class UITownGuildImproveCharacterInfo : BaseMonoBehaviour, IStoreInfoView
{
    //游戏数据
    public GameDataBean gameData;

    //模型和容器
    public GameObject objImproveContent;
    public GameObject objImproveModel;
    public GameObject objImproveEmpty;

    private StoreInfoController mStoreInfoController;
    private List<StoreInfoBean> mListLevelInfo;
    private void Awake()
    {
        mStoreInfoController = new StoreInfoController(this, this);
        mStoreInfoController.GetGuildImproveForCharacter();
    }

    public void InitData(GameDataBean gameData)
    {
        this.gameData = gameData;
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
            if (CheckCanImprove(itemData.baseInfo.accountingInfo, out StoreInfoBean accountingLevelInfo))
            {
                CreateCharacterImproveItem(WorkerEnum.Accounting, itemData, itemData.baseInfo.accountingInfo, accountingLevelInfo);
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
        workerData.GetWorkerExp(out float nextLevelExp, out float currentExp, out float levelProportion);

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
    public void GetAllStoreInfoSuccess(List<StoreInfoBean> listData)
    {

    }

    public void GetAllStoreInfoFail()
    {

    }

    public void GetStoreInfoByTypeSuccess(StoreTypeEnum type, List<StoreInfoBean> listData)
    {
        mListLevelInfo = listData;
    }

    public void GetStoreInfoByTypeFail(StoreTypeEnum type)
    {
    }
    #endregion
}