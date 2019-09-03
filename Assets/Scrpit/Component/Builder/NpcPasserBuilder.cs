using UnityEngine;
using UnityEditor;
using System.Collections;
public class NpcPasserBuilder : NpcNormalBuilder
{
    public SceneTownManager sceneTownManager;

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
            //随机获取一个要去的地方
            SceneTownManager.TownBuildingEnum buildingToGo = RandomUtil.GetRandomEnum<SceneTownManager.TownBuildingEnum>();
            npcAI.SetIntent(NpcAIPasserCpt.PasserIntentEnum.GoToBuilding, buildingToGo);
        }
    }
}