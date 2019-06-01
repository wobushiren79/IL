using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class CharacterInteractiveCpt : BaseMonoBehaviour
{
    public GameObject interactiveObj;
    public TextMesh tvContent;
    public SpriteRenderer srIcon;

    public void ShowInteractive(string content)
    {
        interactiveObj.SetActive(true);
        tvContent.text = content;
    }

    public void CloseInteractive()
    {
        interactiveObj.SetActive(false);
    }

}