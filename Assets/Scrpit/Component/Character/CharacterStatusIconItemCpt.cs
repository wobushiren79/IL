using UnityEngine;
using UnityEditor;

public class CharacterStatusIconItemCpt : BaseMonoBehaviour
{
    public CharacterStatusIconBean statusIconData;
    public SpriteRenderer srIcon;

    public void SetData(CharacterStatusIconBean statusIconData)
    {
        this.statusIconData = statusIconData;
        srIcon.sprite = statusIconData.spIcon;
        srIcon.color = statusIconData.spColor;
    }

}