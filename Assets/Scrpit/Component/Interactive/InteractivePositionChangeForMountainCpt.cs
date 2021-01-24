using UnityEngine;
using UnityEditor;

public class InteractivePositionChangeForMountainCpt : BaseInteractiveCpt
{
    public string interactiveContent;

    [Header("转换的地点")]
    public MountainBuildingEnum positionChange;
    [Header("0：外 1：里")]
    public int OutOrIn = 0;

    private GameObject mInteractiveObj;

    protected SceneMountainManager sceneTownManager;

    private void Awake()
    {
        sceneTownManager = Find<SceneMountainManager>(ImportantTypeEnum.SceneManager);
    }

    public override void InteractiveDetection(CharacterInteractiveCpt characterInt)
    {
        if (Input.GetButtonDown(InputInfo.Interactive_E))
        {
            if (mInteractiveObj != null)
            {
                AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
                sceneTownManager.GetBuildingDoorPosition(positionChange, out Vector2 outDoorPosition, out Vector2 inDoorPosition);
                //本身是在外 要转换到里
                //int checkOutOrIn = 0;
                if (OutOrIn == 0)
                {
                    //checkOutOrIn = 1;
                    //关闭环境音效
                    AudioHandler.Instance.PauseEnvironment();
                    mInteractiveObj.transform.position = inDoorPosition;
                }
                //本身是在里 要转换到外
                else
                {
                    //checkOutOrIn = 0;
                    //开启环境音效
                    AudioHandler.Instance.RestoreEnvironment();
                    mInteractiveObj.transform.position = outDoorPosition;
                }
                //检测故事
                //eventHandler.EventTriggerForStory(positionChange, checkOutOrIn);
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