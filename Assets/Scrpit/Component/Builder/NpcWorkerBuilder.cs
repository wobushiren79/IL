using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NpcWorkerBuilder : BaseMonoBehaviour
{
    //添加的容器
    public GameObject objContainer;
    //工作模型
    public GameObject objWorkModel;

    public InnHandler innHandler;

    public GameDataManager gameDataManager;

    public List<NpcAIWorkerCpt> npcWorkerList = new List<NpcAIWorkerCpt>();

    public void BuildAllWorker()
    {
        if (objContainer == null || objWorkModel == null || gameDataManager == null)
        {
            return;
        }
        List<CharacterBean> listAllWork = gameDataManager.gameData.workCharacterList;
        if (gameDataManager.gameData.userCharacter.baseInfo.isAttendance)
        {
            BuildWork(gameDataManager.gameData.userCharacter);
        }
        for (int i = 0; i < listAllWork.Count; i++)
        {
            CharacterBean itemData = listAllWork[i];
            if (itemData.baseInfo.isAttendance)
            {
                BuildWork(itemData);
            }
        }
    }

    public void BuildWork(CharacterBean characterBean)
    {
        GameObject workerObj = Instantiate(objWorkModel, objWorkModel.transform);
        workerObj.transform.SetParent(objContainer.transform);
        workerObj.SetActive(true);
        workerObj.transform.localScale = new Vector3(2, 2);
        //获取门的坐标 并在门周围生成NPC
        Vector3 doorPosition = innHandler.GetRandomEntrancePosition();
        workerObj.transform.position = doorPosition;

        NpcAIWorkerCpt npcAI = workerObj.GetComponent<NpcAIWorkerCpt>();
        npcAI.SetCharacterData(characterBean);
        npcWorkerList.Add(npcAI);
    }

    /// <summary>
    /// 清空所有工作者
    /// </summary>
    public void ClearAllWork()
    {
        npcWorkerList.Clear();
        CptUtil.RemoveChild(objContainer.transform);
    }

    /// <summary>
    /// 初始化工作者的数据
    /// </summary>
    public void InitWorkerData()
    {
        //初始化优先级
        for (int i = 0; i < npcWorkerList.Count; i++)
        {
            NpcAIWorkerCpt npcAI = npcWorkerList[i];
            npcAI.InitWorkerInfo();
        }
    }
}