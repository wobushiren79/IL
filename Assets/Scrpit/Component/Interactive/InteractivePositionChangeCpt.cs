using UnityEngine;
using UnityEditor;

public class InteractivePositionChangeCpt : BaseInteractiveCpt
{
    public string interactiveContent;
    public GameObject positionChangeObj;

    private GameObject mInteractiveObj;

    public override void InteractiveDetection()
    {
        if (Input.GetButtonDown("Interactive_E"))
        {
            if (mInteractiveObj != null)
                mInteractiveObj.transform.position = positionChangeObj.transform.position;
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