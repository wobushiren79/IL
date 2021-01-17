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
    protected IconDataManager iconDataManager;

    public virtual void Awake()
    {
        gameItemsManager = Find<GameItemsManager>(ImportantTypeEnum.GameItemsManager);
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        audioHandler = Find<AudioHandler>(ImportantTypeEnum.AudioHandler);
        toastManager = Find<ToastManager>(ImportantTypeEnum.ToastManager);
        iconDataManager = Find<IconDataManager>(ImportantTypeEnum.UIManager);

        npcAIWorker = GetComponent<NpcAIWorkerCpt>();
    }

    public virtual void Start()
    {
   
    }

    public virtual void OnDestroy()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// 升级提示
    /// </summary>
    /// <param name="worker"></param>
    public void ToastForLevelUp(WorkerEnum worker)
    {
        long toastId = 1121;
        string iconStr = "";
        switch (worker)
        {
            case WorkerEnum.Chef:
                iconStr = "worker_chef_1";
                toastId = 1121;
                break;
            case WorkerEnum.Waiter:
                iconStr = "worker_waiter_1";
                toastId = 1122;
                break;
            case WorkerEnum.Accountant:
                iconStr = "worker_accountant_1";
                toastId = 1123;
                break;
            case WorkerEnum.Accost:
                iconStr = "worker_accost_1";
                toastId = 1124;
                break;
            case WorkerEnum.Beater:
                iconStr = "worker_beater_1";
                toastId = 1125;
                break;
        }
        AudioHandler.Instance.PlaySound(AudioSoundEnum.Reward);
        string levelupStr = string.Format(GameCommonInfo.GetUITextById(toastId), npcAIWorker.characterData.baseInfo.name);
        ToastHandler.Instance.ToastHint(iconDataManager.GetIconSpriteByName(iconStr), levelupStr);
    }

}