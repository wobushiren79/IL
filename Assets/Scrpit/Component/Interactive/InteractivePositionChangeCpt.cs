using UnityEngine;
using UnityEditor;

public class InteractivePositionChangeCpt : BaseInteractiveCpt
{
    public string interactiveContent;

    [Header("转换的地点")]
    public TownBuildingEnum positionChange;
    [Header("0：外 1：里")]
    public int OutOrIn = 0;

    private GameObject mInteractiveObj;

    protected EventHandler eventHandler;
    protected SceneTownManager sceneTownManager;
    protected AudioHandler audioHandler;
    private void Awake()
    {
        eventHandler = Find<EventHandler>(ImportantTypeEnum.EventHandler);
        sceneTownManager = Find<SceneTownManager>(ImportantTypeEnum.SceneManager);
        audioHandler = Find<AudioHandler>(ImportantTypeEnum.AudioHandler);
    }

    public override void InteractiveDetection(CharacterInteractiveCpt characterInt)
    {
        if (Input.GetButtonDown(InputInfo.Interactive_E))
        {
            if (mInteractiveObj != null)
            {
                AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
                sceneTownManager.GetBuildingDoorPosition(positionChange,out Vector2 outDoorPosition,out Vector2 inDoorPosition);
                //本身是在外 要转换到里
                int checkOutOrIn = 0;
                if (OutOrIn == 0)
                {
                    checkOutOrIn = 1;
                    //关闭环境音效
                    audioHandler.PauseEnvironment();
                    mInteractiveObj.transform.position = inDoorPosition;
                }
                //本身是在里 要转换到外
                else
                {
                    checkOutOrIn = 0;
                    //开启环境音效
                    audioHandler.RestoreEnvironment();
                    mInteractiveObj.transform.position = outDoorPosition;
                }
                //检测故事
                eventHandler.EventTriggerForStory(positionChange, checkOutOrIn);
            }        
        }
    }

    public override void InteractiveEnd(CharacterInteractiveCpt characterInt)
    {
        this.mInteractiveObj = null;
        characterInt.CloseInteractive();
    }

    public override void InteractiveStart(CharacterInteractiveCpt characterInt)
    {
        this.mInteractiveObj = characterInt.gameObject;
        characterInt.ShowInteractive(interactiveContent);
    }
}