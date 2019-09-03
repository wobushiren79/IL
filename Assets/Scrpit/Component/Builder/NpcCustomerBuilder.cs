using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;

public class NpcCustomerBuilder : NpcNormalBuilder
{
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
            BuildCustomer(npcPosition);
        }
    }

    public IEnumerator StartBuild()
    {
        while (isBuildNpc)
        {
            //if (gameTimeHandler.hour >= 11 && gameTimeHandler.hour <= 13)
            //{
            //    buildInterval = 1;
            //}
            //else if (gameTimeHandler.hour >= 17 && gameTimeHandler.hour <= 19)
            //{
            //    buildInterval = 1;
            //}
            //else if (gameTimeHandler.hour < 11)
            //{
            //    buildInterval = 1 + (10 - gameTimeHandler.hour);
            //}
            //else if (gameTimeHandler.hour > 13 && gameTimeHandler.hour < 17)
            //{
            //    buildInterval = 2.5f;
            //}
            //else if(gameTimeHandler.hour > 19)
            //{
            //    buildInterval = 2 + (gameTimeHandler.hour - 17);
            //}
            yield return new WaitForSeconds(buildInterval);
            Vector3 npcPosition = GetRandomStartPosition();
            BuildCustomer(npcPosition);
        }
    }

    public void BuildCustomer(Vector3 npcPosition)
    {
        //如果大于构建上线则不再创建新NPC
        if (objContainer.transform.childCount > buildMaxNumber)
            return;
        //生成NPC
        GameObject npcObj = BuildNpc(npcPosition);
        //设置意图
        NpcAICustomerCpt customerAI = npcObj.GetComponent<NpcAICustomerCpt>();
        //想要吃饭概率
        float eatProbability = Random.Range(0f, 1f);
        if (eatProbability > 0.9f)
        {
             customerAI.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.Want);
        }
        else
        {
             customerAI.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.Walk);
        }
    }

}