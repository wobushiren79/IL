using UnityEngine;
using UnityEditor;

public class NpcAIWokerFoBaseCpt : BaseMonoBehaviour
{
    //主控AI
    public NpcAIWorkerCpt npcAIWorker;

    protected GameItemsManager gameItemsManager;
    protected GameDataManager gameDataManager;

    public virtual void Awake()
    {
        gameItemsManager = Find<GameItemsManager>( ImportantTypeEnum.GameItemsManager);
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
    }

    public virtual void Start()
    {
        npcAIWorker = GetComponent<NpcAIWorkerCpt>();
    }

}