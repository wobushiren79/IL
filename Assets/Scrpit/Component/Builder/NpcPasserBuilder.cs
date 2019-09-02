using UnityEngine;
using UnityEditor;

public class NpcPasserBuilder : NpcNormalBuilder
{


    public void BuilderPasserForInit(int npcNumber)
    {
        for (int i = 0; i < npcNumber; i++)
        {
            //获取随机坐标
            Vector3 npcPosition = GetRandomInitStartPosition();
            //生成NPC
            GameObject npcObj = BuildNpc(npcPosition);
            //设置意图
            NpcAIPasserCpt npcAI = npcObj.GetComponent<NpcAIPasserCpt>();
            if (npcAI != null)
                npcAI.SetIntent(NpcAIPasserCpt.PasserIntentEnum.GoToBuilding,SceneTownManager.TownBuildingEnum.Market);
        }
    }

}