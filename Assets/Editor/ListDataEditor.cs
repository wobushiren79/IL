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
        AddIconBeanDictionaryByFile("Assets/Texture/Character/character_hat.png", dressManager.listIconHat);
    }

    [MenuItem("Custom/List/AddDressClothes")]
    public static void AddDressClothes()
    {
        GameObject Target = Selection.gameObjects[0];
        CharacterDressManager dressManager = Target.GetComponent<CharacterDressManager>();
        dressManager.listIconClothes.Clear();
        AddIconBeanDictionaryByFile("Assets/Texture/Character/character_clothes.png", dressManager.listIconClothes);
        AddIconBeanDictionaryByFile("Assets/Texture/Character/character_clothes_work.png", dressManager.listIconClothes);
    }

    [MenuItem("Custom/List/AddDressShoes")]
    public static void AddDressShoes()
    {
        GameObject Target = Selection.gameObjects[0];
        CharacterDressManager dressManager = Target.GetComponent<CharacterDressManager>();
        dressManager.listIconShoes.Clear();
        AddIconBeanDictionaryByFile("Assets/Texture/Character/character_shoes.png", dressManager.listIconShoes);
        AddIconBeanDictionaryByFile("Assets/Texture/Character/character_shoes_work.png", dressManager.listIconShoes);
    }

    [MenuItem("Custom/List/AddBodyHair")]
    public static void AddBodyHair()
    {
        GameObject Target = Selection.gameObjects[0];
        CharacterBodyManager bodyManager = Target.GetComponent<CharacterBodyManager>();
        AddIconBeanDictionaryByFile("Assets/Texture/Character/character_hair.png", bodyManager.listIconBodyHair);
    }

    [MenuItem("Custom/List/AddBodyEye")]
    public static void AddBodyEye()
    {
        GameObject Target = Selection.gameObjects[0];
        CharacterBodyManager bodyManager = Target.GetComponent<CharacterBodyManager>();
        AddIconBeanDictionaryByFile("Assets/Texture/Character/character_eye.png", bodyManager.listIconBodyEye);
    }

    [MenuItem("Custom/List/AddBodyMouth")]
    public static void AddBodyMouth()
    {
        GameObject Target = Selection.gameObjects[0];
        CharacterBodyManager bodyManager = Target.GetComponent<CharacterBodyManager>();
        AddIconBeanDictionaryByFile("Assets/Texture/Character/character_mouth.png", bodyManager.listIconBodyMouth);
    }


    [MenuItem("Custom/List/AddFoodIcon")]
    public static void AddFood()
    {
        GameObject Target = Selection.gameObjects[0];
        InnFoodManager foodManager = Target.GetComponent<InnFoodManager>();
        AddIconBeanDictionaryByFolder("Assets/Texture/Food/", foodManager.listFoodIcon);
    }

    /// <summary>
    /// 根据指定文件添加字典
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="map"></param>
    public static void AddIconBeanDictionaryByFile(string filePath, IconBeanDictionary map)
    {
        Object[] objs = AssetDatabase.LoadAllAssetsAtPath(filePath);
        objs.ToList().ForEach(obj =>
        {
            if (obj as Sprite != null)
            {
                map.Add(obj.name, obj as Sprite);
            }
        });
    }

    /// <summary>
    /// 根据文件夹下所有文件添加字典
    /// </summary>
    /// <param name="folderPath"></param>
    /// <param name="mapFood"></param>
    public static void AddIconBeanDictionaryByFolder(string folderPath, IconBeanDictionary map)
    {
        map.Clear();
        FileInfo[] files = FileUtil.GetFilesByPath(folderPath);
        foreach (FileInfo item in files)
        {
            Object[] objs = AssetDatabase.LoadAllAssetsAtPath(folderPath + item.Name);
            objs.ToList().ForEach(obj =>
            {
                if (obj as Sprite != null)
                {
                    map.Add(obj.name, obj as Sprite);
                }
            });
        }
    }
}
