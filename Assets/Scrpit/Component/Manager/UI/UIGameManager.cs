using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class UIGameManager : BaseUIManager
{
    [Header("初始化")]
    public BaseSceneInit sceneInit;
    [Header("建造")]
    public NpcCustomerBuilder npcCustomerBuilder;
    public NpcEventBuilder npcEventBuilder;
    public NpcWorkerBuilder npcWorkerBuilder;

    private void Awake()
    {
        sceneInit = Find<BaseSceneInit>(ImportantTypeEnum.Init);

        npcCustomerBuilder = Find<NpcCustomerBuilder>(ImportantTypeEnum.NpcBuilder);
        npcEventBuilder = Find<NpcEventBuilder>(ImportantTypeEnum.NpcBuilder);
        npcWorkerBuilder = Find<NpcWorkerBuilder>(ImportantTypeEnum.NpcBuilder);

    }

}