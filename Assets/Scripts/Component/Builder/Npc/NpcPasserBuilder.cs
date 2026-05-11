using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class NpcPasserBuilder : NpcNormalBuilder
{
    public List<NpcAIPasserCpt> listPasser = new List<NpcAIPasserCpt>();

    private void Start()
    {
        isBuildNpc = true;
        StartCoroutine(StartBuild());
    }

    /// <summary>
    /// 初始化生成NPC
    /// </summary>
    /// <param name="npcNumber"></param>
    public void BuilderPasserForInit(int npcNumber)
    {
        CptUtil.RemoveChildsByActive(objContainer);
        listPasser.Clear();
        for (int i = 0; i < npcNumber; i++)
        {
            //获取随机坐标
            Vector3 npcPosition = GetRandomInitStartPosition();
            BuilderPasser(npcPosition);
        }
    }

    public IEnumerator StartBuild()
    {
        while (isBuildNpc)
        {
            yield return new WaitForSeconds(buildInterval);
            SceneTownManager sceneTownManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneTownManager>();
            Vector3 npcPosition = sceneTownManager.GetRandomTownDoorPosition();
            BuilderPasser(npcPosition);
        }
    }

    public void BuilderPasser(Vector3 npcPosition)
    {
        //如果大于构建上线则不再创建新NPC
        if (objContainer.transform.childCount > buildMaxNumber)
            return;
        //生成NPC
        GameObject npcObj = BuildNpc(npcPosition);
        //设置意图
        NpcAIPasserCpt npcAI = npcObj.GetComponent<NpcAIPasserCpt>();
        if (npcAI != null)
        {
            //设置天前所在地
            npcAI.SetLocation(TownBuildingEnum.Town);
            //随机获取一个要去的地方
            npcAI.SetRandomBuildingToGo();
        }
        listPasser.Add(npcAI);
    }

    /// <summary>
    /// 获取所有路人
    /// </summary>
    /// <returns></returns>
    public List<NpcAIPasserCpt> GetAllPasser()
    {
        return listPasser;
    }


}