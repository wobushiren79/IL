using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NpcWorkerBuilder : BaseMonoBehaviour
{
    //添加的容器
    public GameObject objContainer;
    //工作模型
    public GameObject objWorkModel;

    public GameDataManager gameDataManager;

    public List<NpcAIWorkerCpt> npcWorkerList = new List<NpcAIWorkerCpt>();

    public void BuildAllWorker()
    {
        if (objContainer == null || objWorkModel == null || gameDataManager == null)
        {
            return;
        }
        List<CharacterBean> listAllWork = gameDataManager.gameData.workCharacterList;
        BuildWork(gameDataManager.gameData.userCharacter);
        for (int i = 0; i < listAllWork.Count; i++)
        {
            CharacterBean itemData = listAllWork[i];
            BuildWork(itemData);
        }
    }

    public void BuildWork(CharacterBean characterBean)
    {
        GameObject workerObj = Instantiate(objWorkModel, objWorkModel.transform);
        workerObj.transform.SetParent(objContainer.transform);
        workerObj.SetActive(true);
        workerObj.transform.localScale = new Vector3(2, 2);
        float npcPositionX = Random.Range(0, 1f);
        float npcPositionY = Random.Range(-1f, 0f);
        workerObj.transform.position = new Vector3(npcPositionX, npcPositionY);

        NpcAIWorkerCpt npcAI = workerObj.GetComponent<NpcAIWorkerCpt>();
        npcAI.SetCharacterData(characterBean);
        if (characterBean.baseInfo != null)
        {
            CharacterBaseBean characterBase = characterBean.baseInfo;
            npcAI.isChef = characterBase.isChef;
            npcAI.isAccounting = characterBase.isAccounting;
            npcAI.isWaiter = characterBase.isWaiter;
        }
        npcWorkerList.Add(npcAI);
    }

    public void RefreshWorkStatus()
    {
        if (npcWorkerList == null)
            return;
        for (int i = 0; i < npcWorkerList.Count; i++)
        {
            NpcAIWorkerCpt npcAI = npcWorkerList[i];
            CharacterBaseBean characterBase = npcAI.characterData.baseInfo;
            npcAI.isChef = characterBase.isChef;
            npcAI.isAccounting = characterBase.isAccounting;
            npcAI.isWaiter = characterBase.isWaiter;
        }
    }

    public void ClearAllWork()
    {
        npcWorkerList.Clear();
        CptUtil.RemoveChild(objContainer.transform);
    }
}