using UnityEngine;
using UnityEditor;

public class NpcAIWorkerCpt : BaseNpcAI
{

    public enum WorkerIntentEnum
    {
        Idle,//空闲
        Cook,//做菜
    }

    //厨师AI控制
    public NpcAIWorkerForChefCpt aiForChef;

    public WorkerIntentEnum workerIntent = WorkerIntentEnum.Idle;//工作者的想法

    //是否开启厨师
    public bool isChef;


    private void Awake()
    {
        aiForChef = gameObject.AddComponent<NpcAIWorkerForChefCpt>();
    }

    /// <summary>
    /// 根据意图设置目的地
    /// </summary>
    public void SetDestinationByIntent(WorkerIntentEnum intent, BuildStoveCpt stoveCpt,MenuInfoBean menuInfo)
    {
        switch (intent)
        {
            case WorkerIntentEnum.Cook:
                aiForChef.SetCookData(stoveCpt, menuInfo);
                break;
        }
    }
}