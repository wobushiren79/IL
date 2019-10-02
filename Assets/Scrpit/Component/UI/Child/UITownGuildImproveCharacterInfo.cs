using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class UITownGuildImproveCharacterInfo : BaseMonoBehaviour
{
    //游戏数据
    public GameDataBean gameData;

    //模型和容器
    public GameObject objImproveContent;
    public GameObject objImproveModel;
    public GameObject objImproveEmpty;

    public void InitData(GameDataBean gameData)
    {
        this.gameData = gameData;
        objImproveEmpty.SetActive(false);
        List<CharacterBean> listCharacterData = gameData.GetAllCharacterData();
        CptUtil.RemoveChildsByActive(objImproveContent.transform);

        int itemNumer = 0;
        foreach (CharacterBean itemData in listCharacterData)
        {
            if (CheckCanImprove(itemData.baseInfo.chefInfo))
            {
                CreateCharacterImproveItem(WorkerEnum.Chef, itemData, itemData.baseInfo.chefInfo);
                itemNumer++;
            }
            if (CheckCanImprove(itemData.baseInfo.waiterInfo))
            {
                CreateCharacterImproveItem(WorkerEnum.Waiter, itemData, itemData.baseInfo.waiterInfo);
                itemNumer++;
            }
            if (CheckCanImprove(itemData.baseInfo.accountingInfo))
            {
                CreateCharacterImproveItem(WorkerEnum.Accounting, itemData, itemData.baseInfo.accountingInfo);
                itemNumer++;
            }
            if (CheckCanImprove(itemData.baseInfo.accostInfo))
            {
                CreateCharacterImproveItem(WorkerEnum.Accost, itemData, itemData.baseInfo.accostInfo);
                itemNumer++;
            }
            if (CheckCanImprove(itemData.baseInfo.beaterInfo))
            {
                CreateCharacterImproveItem(WorkerEnum.Beater, itemData, itemData.baseInfo.beaterInfo);
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
    public void CreateCharacterImproveItem(WorkerEnum workerType, CharacterBean characterData, CharacterWorkerBaseBean workerData)
    {
        GameObject objItem = Instantiate(objImproveContent, objImproveModel);
        ItemTownGuildImproveCharacterCpt improveCharacterCpt = objItem.GetComponent<ItemTownGuildImproveCharacterCpt>();
        improveCharacterCpt.SetData(workerType, characterData, workerData);
    }

    /// <summary>
    /// 检测是否要升级
    /// </summary>
    /// <param name="workerData"></param>
    /// <returns></returns>
    private bool CheckCanImprove(CharacterWorkerBaseBean workerData)
    {
        workerData.GetWorkerExp(out float nextLevelExp, out float currentExp, out float levelProportion);
        if (levelProportion == 1)
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
}