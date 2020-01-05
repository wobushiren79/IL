using UnityEngine;
using UnityEditor;

public class InteractiveSceneChangeCpt : BaseInteractiveCpt
{
    
    public string interactiveContent;
    //需要跳转的场景
    public ScenesEnum changeScene;

    public override void InteractiveDetection(CharacterInteractiveCpt characterInt)
    {
        if (Input.GetButtonDown(InputInfo.Interactive_E))
        {
            SceneUtil.SceneChange(changeScene);
        }
    }

    public override void InteractiveEnd(CharacterInteractiveCpt characterInt)
    {
        characterInt.CloseInteractive();
    }

    public override void InteractiveStart(CharacterInteractiveCpt characterInt)
    {
        characterInt.ShowInteractive(interactiveContent);
    }
}