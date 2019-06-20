using UnityEngine;
using UnityEditor;

public abstract class BaseInteractiveCpt : BaseMonoBehaviour
{
    public Collider2D colliderInteractive;

    public bool canInteractive=false;

    public void Update()
    {
        if (canInteractive)
        {
            InteractiveDetection();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionObj = collision.gameObject;
        CharacterInteractiveCpt characterInt = collisionObj.GetComponent<CharacterInteractiveCpt>();
        if (characterInt != null)
        {
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
        }
    }

    public abstract void InteractiveStart(CharacterInteractiveCpt characterInt);

    public abstract void InteractiveEnd(CharacterInteractiveCpt characterInt);

    public abstract void InteractiveDetection();
}