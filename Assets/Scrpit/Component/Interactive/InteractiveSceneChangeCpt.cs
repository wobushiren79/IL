using UnityEngine;
using UnityEditor;

public class InteractiveSceneChangeCpt : BaseInteractiveCpt
{
    public string interactiveContent;
    public string sceneChangeName;

    public override void InteractiveDetection()
    {
        if (Input.GetButtonDown("Interactive_E"))
        {
            SceneUtil.SceneChange(sceneChangeName);
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