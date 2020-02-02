using UnityEngine;
using UnityEditor;

public class NpcAIWokerFoBaseCpt : BaseMonoBehaviour
{
    //主控AI
    public NpcAIWorkerCpt npcAIWorker;

    protected GameItemsManager gameItemsManager;

    public virtual void Awake()
    {
        gameItemsManager = Find<GameItemsManager>( ImportantTypeEnum.GameItemsManager);
    }

    public virtual void Start()
    {
        npcAIWorker = GetComponent<NpcAIWorkerCpt>();
    }

}