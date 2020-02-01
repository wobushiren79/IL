using UnityEngine;
using UnityEditor;

public class CharacterStatusIconItemCpt : BaseMonoBehaviour
{
    public CharacterStatusIconBean statusIconData;

    public void SetData(CharacterStatusIconBean statusIconData)
    {
        this.statusIconData = statusIconData;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = statusIconData.spIcon;
        spriteRenderer.color = statusIconData.spColor;
    }

}