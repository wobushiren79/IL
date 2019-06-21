using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ListDataEditor : Editor
{
    [MenuItem("Custom/List/AddDressHat")]
    public static void AddDressHat()
    {
        GameObject Target = Selection.gameObjects[0];
        CharacterDressManager dressManager = Target.GetComponent<CharacterDressManager>();
        AddList("Assets/Texture/Character/character_hat.png", dressManager.listIconHat);
    }

    [MenuItem("Custom/List/AddDressClothes")]
    public static void AddDressClothes()
    {
        GameObject Target = Selection.gameObjects[0];
        CharacterDressManager dressManager = Target.GetComponent<CharacterDressManager>();
        dressManager.listIconClothes.Clear();
        AddList("Assets/Texture/Character/character_clothes.png", dressManager.listIconClothes);
        AddList("Assets/Texture/Character/character_clothes_work.png", dressManager.listIconClothes);
    }

    [MenuItem("Custom/List/AddDressShoes")]
    public static void AddDressShoes()
    {
        GameObject Target = Selection.gameObjects[0];
        CharacterDressManager dressManager = Target.GetComponent<CharacterDressManager>();
        dressManager.listIconShoes.Clear();
        AddList("Assets/Texture/Character/character_shoes.png", dressManager.listIconShoes);
        AddList("Assets/Texture/Character/character_shoes_work.png", dressManager.listIconShoes);
    }

    [MenuItem("Custom/List/AddBodyHair")]
    public static void AddBodyHair()
    {
        GameObject Target = Selection.gameObjects[0];
        CharacterBodyManager bodyManager = Target.GetComponent<CharacterBodyManager>();
        AddList("Assets/Texture/Character/character_hair.png", bodyManager.listIconBodyHair);
    }

    [MenuItem("Custom/List/AddBodyEye")]
    public static void AddBodyEye()
    {
        GameObject Target = Selection.gameObjects[0];
        CharacterBodyManager bodyManager = Target.GetComponent<CharacterBodyManager>();
        AddList("Assets/Texture/Character/character_eye.png", bodyManager.listIconBodyEye);
    }

    [MenuItem("Custom/List/AddBodyMouth")]
    public static void AddBodyMouth()
    {
        GameObject Target = Selection.gameObjects[0];
        CharacterBodyManager bodyManager = Target.GetComponent<CharacterBodyManager>();
        AddList("Assets/Texture/Character/character_mouth.png", bodyManager.listIconBodyMouth);
    }

    public static void AddList(string paths,List<IconBean> listData)
    {
        string temp = paths;
        Object[] objs = AssetDatabase.LoadAllAssetsAtPath(temp);
        objs.ToList().ForEach(obj =>
        {
            IconBean iconBean = new IconBean();
            iconBean.key = obj.name;
            iconBean.value = obj as Sprite;
            listData.Add(iconBean);
        });
    }
}
