using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class CharacterInteractiveCpt : BaseMonoBehaviour
{
    public TextMesh tvContent;
    public SpriteRenderer srIcon;

    public void SetChangeLocation(string location)
    {
        gameObject.SetActive(true);
        tvContent.text = location;
    }

    public void CloseInteractive()
    {
        gameObject.SetActive(false);
    }

}