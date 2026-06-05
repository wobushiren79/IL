using UnityEngine;
using UnityEditor;

public class GameAnimEditor : AnimEditor
{

    [MenuItem("游戏/动画/CreateAnimForUI")]
    public static void CreateAnimForUI()
    {
        Texture2D[] listText = Selection.GetFiltered<Texture2D>(SelectionMode.DeepAssets);
        foreach (Texture2D itemPicTex in listText)
        {
            CreateAnimForImage(itemPicTex, "Assets/Anim/Animation", 5);
        }
    }

    [MenuItem("游戏/动画/CreateAnimForMask")]
    public static void CreateAnimForMask()
    {
        Texture2D[] listText = Selection.GetFiltered<Texture2D>(SelectionMode.DeepAssets);
        foreach (Texture2D itemPicTex in listText)
        {
            CreateAnimForSpriteRenderer(itemPicTex, "Assets/Anim/Animation/Equip/Mask", 5);
        }
    }

    [MenuItem("游戏/动画/CreateAnimForHat")]
    public static void CreateAnimForHat()
    {
        Texture2D[] listText = Selection.GetFiltered<Texture2D>(SelectionMode.DeepAssets);
        foreach (Texture2D itemPicTex in listText)
        {
            CreateAnimForSpriteRenderer(itemPicTex, "Assets/Anim/Animation/Equip/Hat", 5);
        }
    }

    [MenuItem("游戏/动画/CreateAnimForClothes")]
    public static void CreateAnimForClothes()
    {
        Texture2D[] listText = Selection.GetFiltered<Texture2D>(SelectionMode.DeepAssets);
        foreach (Texture2D itemPicTex in listText)
        {
            CreateAnimForSpriteRenderer(itemPicTex, "Assets/Anim/Animation/Equip/Clothes", 5);
        }
    }

    [MenuItem("游戏/动画/CreateAnimForShoes")]
    public static void CreateAnimForShoes()
    {
        Texture2D[] listText = Selection.GetFiltered<Texture2D>(SelectionMode.DeepAssets);
        foreach (Texture2D itemPicTex in listText)
        {
            CreateAnimForSpriteRenderer(itemPicTex, "Assets/Anim/Animation/Equip/Shoes", 5);
        }
    }

    [MenuItem("游戏/动画/CreateAnimForItems")]
    public static void CreateAnimForItems()
    {
        Texture2D[] listText = Selection.GetFiltered<Texture2D>(SelectionMode.DeepAssets);
        foreach (Texture2D itemPicTex in listText)
        {
            CreateAnimForSpriteRenderer(itemPicTex, "Assets/Anim/Animation/Equip/Items", 5);
        }
    }

    [MenuItem("游戏/动画/CreateAnimForFood")]
    public static void CreateAnimForFood()
    {
        Texture2D[] listText = Selection.GetFiltered<Texture2D>(SelectionMode.DeepAssets);
        foreach (Texture2D itemPicTex in listText)
        {
            CreateAnimForSpriteRenderer(itemPicTex, "Assets/Anim/Animation/Food/", 5);
        }
    }
}