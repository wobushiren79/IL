using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;

public class NpcCustomerBuilder : NpcNormalBuilder,IBaseObserver
{
    private void Start()
    {
        gameTimeHandler.AddObserver(this);
        //StartBuildCustomer();
    }

    /// <summary>
    /// 初始化生成NPC
    /// </summary>
    /// <param name="npcNumber"></param>
    public void BuilderCustomerForInit(int npcNumber)
    {
        for (int i = 0; i < npcNumber; i++)
        {
            //获取随机坐标
            Vector3 npcPosition = GetRandomInitStartPosition();
            BuildCustomer(npcPosition);
        }
    }

    /// <summary>
    /// 开始建造NPC
    /// </summary>
    public void StartBuildCustomer()
    {
        isBuildNpc = true;
        //StartCoroutine(StartBuild());
    }
    /// <summary>
    /// 停止建造NPC
    /// </summary>
    public void StopBuildCustomer()
    {
        isBuildNpc = false;
        StopAllCoroutines();
    }

    private IEnumerator StartBuild()
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
        float rateWant =  gameDataManager.gameData.GetInnAttributesData().CalculationCustomerWantRate();
        if (eatProbability <= rateWant)
        {
             customerAI.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.Want);
        }
        else
        {
             customerAI.SetIntent(NpcAICustomerCpt.CustomerIntentEnum.Walk);
        }
    }

    /// <summary>
    /// 时间回调
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="observable"></param>
    /// <param name="type"></param>
    /// <param name="obj"></param>
    public void ObserbableUpdate<T>(T observable, int type, params System.Object[] obj) where T : Object
    {
        if((GameTimeHandler.NotifyTypeEnum)type== GameTimeHandler.NotifyTypeEnum.NewDay)
        {
            StopBuildCustomer();
            ClearNpc();
        }
        else if ((GameTimeHandler.NotifyTypeEnum)type == GameTimeHandler.NotifyTypeEnum.EndDay)
        {
            StopBuildCustomer();
            ClearNpc();
        }
    }
}