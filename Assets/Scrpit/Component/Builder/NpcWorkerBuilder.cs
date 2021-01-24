using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NpcWorkerBuilder : BaseMonoBehaviour
{
    //添加的容器
    public GameObject objContainer;
    //工作模型
    public GameObject objWorkModel;

    public List<NpcAIWorkerCpt> listNpcWorker = new List<NpcAIWorkerCpt>();

    ;

    private void Awake()
    {
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
    }

    public void BuildAllWorker()
    {
        if (objContainer == null || objWorkModel == null || gameDataManager == null)
        {
            return;
        }
        ClearAllWork();
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        List<CharacterBean> listAllWork = gameData.listWorkerCharacter;

        if (gameData.userCharacter.baseInfo.GetWorkerStatus() == WorkerStatusEnum.Work)
        {
            //获取门的坐标 并在门周围生成NPC
            Vector3 doorPosition = InnHandler.Instance.GetRandomEntrancePosition();
            //向下3个单位
            doorPosition += new Vector3(0, -3f, 0);
            BuildWork(gameData.userCharacter, doorPosition);
        }
        for (int i = 0; i < listAllWork.Count; i++)
        {
            //获取门的坐标 并在门周围生成NPC
            Vector3 doorPosition = InnHandler.Instance.GetRandomEntrancePosition();
            //向下3个单位
            doorPosition += new Vector3(0, -3f, 0);
            CharacterBean itemData = listAllWork[i];
            if (itemData.baseInfo.GetWorkerStatus() == WorkerStatusEnum.Work)
            {
                BuildWork(itemData, doorPosition);
            }
        }
    }

    public void BuildWork(CharacterBean characterBean, Vector3 position)
    {
        GameObject workerObj = Instantiate(objWorkModel, objWorkModel.transform);
        workerObj.transform.SetParent(objContainer.transform);
        workerObj.SetActive(true);
        workerObj.transform.localScale = new Vector3(1, 1);
        workerObj.transform.position = (position + new Vector3(Random.Range(-1, 1), 0));

        NpcAIWorkerCpt npcAI = workerObj.GetComponent<NpcAIWorkerCpt>();
        npcAI.SetCharacterData(characterBean);
        npcAI.SetIntent(NpcAIWorkerCpt.WorkerIntentEnum.Idle);
        listNpcWorker.Add(npcAI);
    }

    /// <summary>
    /// 清空所有工作者
    /// </summary>
    public void ClearAllWork()
    {
        listNpcWorker.Clear();
        CptUtil.RemoveChild(objContainer);
    }

    /// <summary>
    /// 初始化工作者的数据
    /// </summary>
    public void InitWorkerData()
    {
        //初始化优先级
        for (int i = 0; i < listNpcWorker.Count; i++)
        {
            NpcAIWorkerCpt npcAI = listNpcWorker[i];
            npcAI.InitWorkerInfo();
        }
    }

    /// <summary>
    /// 刷新工作者数据
    /// </summary>
    public void RefreshWorkerData()
    {
        if (listNpcWorker == null)
            return;
        foreach (NpcAIWorkerCpt npcAIWorker in listNpcWorker)
        {
            npcAIWorker.SetCharacterData(npcAIWorker.characterData);
        }
    }

}