using UnityEngine;
using UnityEditor;

public abstract class BaseInteractiveCpt : BaseMonoBehaviour
{
    public CharacterInteractiveCpt characterInt;

    public bool canInteractive=false;

    public void Update()
    {
        if (canInteractive&& characterInt)
        {
            InteractiveDetection(characterInt);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionObj = collision.gameObject;
        CharacterInteractiveCpt characterInt = collisionObj.GetComponent<CharacterInteractiveCpt>();
        if (characterInt != null)
        {
            this.characterInt = characterInt;
            InteractiveStart(characterInt);
            canInteractive = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject collisionObj = collision.gameObject;
        CharacterInteractiveCpt characterInt = collisionObj.GetComponent<CharacterInteractiveCpt>();
        if (characterInt != null)
        {
            InteractiveEnd(characterInt);
            canInteractive = false;
            this.characterInt = null;
        }
    }

    public abstract void InteractiveStart(CharacterInteractiveCpt characterInt);

    public abstract void InteractiveEnd(CharacterInteractiveCpt characterInt);

    public abstract void InteractiveDetection(CharacterInteractiveCpt characterInt);
}