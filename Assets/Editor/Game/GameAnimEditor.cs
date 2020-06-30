using UnityEngine;
using UnityEditor;

public class GameAnimEditor : AnimEditor
{

    [MenuItem("Custom/Anim/CreateAnimForMask")]
    public static void CreateAnimForMask()
    {
        Texture2D[] listText = Selection.GetFiltered<Texture2D>(SelectionMode.DeepAssets);
        foreach (Texture2D itemPicTex in listText)
        {
            CreateAnimForTex(itemPicTex, "Assets/Anim/Animation/Equip/Mask", 5);
        }
    }

    [MenuItem("Custom/Anim/CreateAnimForHat")]
    public static void CreateAnimForHat()
    {
        Texture2D[] listText = Selection.GetFiltered<Texture2D>(SelectionMode.DeepAssets);
        foreach (Texture2D itemPicTex in listText)
        {
            CreateAnimForTex(itemPicTex, "Assets/Anim/Animation/Equip/Hat", 5);
        }
    }

    [MenuItem("Custom/Anim/CreateAnimForClothes")]
    public static void CreateAnimForClothes()
    {
        Texture2D[] listText = Selection.GetFiltered<Texture2D>(SelectionMode.DeepAssets);
        foreach (Texture2D itemPicTex in listText)
        {
            CreateAnimForTex(itemPicTex, "Assets/Anim/Animation/Equip/Clothes", 5);
        }
    }

    [MenuItem("Custom/Anim/CreateAnimForShoes")]
    public static void CreateAnimForShoes()
    {
        Texture2D[] listText = Selection.GetFiltered<Texture2D>(SelectionMode.DeepAssets);
        foreach (Texture2D itemPicTex in listText)
        {
            CreateAnimForTex(itemPicTex, "Assets/Anim/Animation/Equip/Shoes", 5);
        }
    }

    [MenuItem("Custom/Anim/CreateAnimForItems")]
    public static void CreateAnimForItems()
    {
        Texture2D[] listText = Selection.GetFiltered<Texture2D>(SelectionMode.DeepAssets);
        foreach (Texture2D itemPicTex in listText)
        {
            CreateAnimForTex(itemPicTex, "Assets/Anim/Animation/Equip/Items", 5);
        }
    }
}