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

    private void Awake()
    {
        eventHandler = Find<EventHandler>(ImportantTypeEnum.EventHandler);
        sceneTownManager = Find<SceneTownManager>(ImportantTypeEnum.SceneManager);
    }

    public override void InteractiveDetection(CharacterInteractiveCpt characterInt)
    {
        if (Input.GetButtonDown(InputInfo.Interactive_E))
        {
            if (mInteractiveObj != null)
            {
                sceneTownManager.GetBuildingDoorPosition(positionChange,out Vector2 outDoorPosition,out Vector2 inDoorPosition);
                if (OutOrIn==0)
                {
                    mInteractiveObj.transform.position = inDoorPosition;
                }
                else
                {
                    mInteractiveObj.transform.position = outDoorPosition;
                }
                //检测故事
                eventHandler.EventTriggerForStory(positionChange, OutOrIn);
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