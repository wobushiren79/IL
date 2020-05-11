using UnityEngine;
using UnityEditor;

public class NpcAIWokerFoBaseCpt : BaseMonoBehaviour
{
    //主控AI
    public NpcAIWorkerCpt npcAIWorker;

    protected GameItemsManager gameItemsManager;
    protected GameDataManager gameDataManager;
    protected AudioHandler audioHandler;
    protected ToastManager toastManager;

    public virtual void Awake()
    {
        gameItemsManager = Find<GameItemsManager>( ImportantTypeEnum.GameItemsManager);
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        audioHandler = Find<AudioHandler>(ImportantTypeEnum.AudioHandler);
        toastManager = Find<ToastManager>(ImportantTypeEnum.ToastManager);
    }

    public virtual void Start()
    {
        npcAIWorker = GetComponent<NpcAIWorkerCpt>();
    }

}