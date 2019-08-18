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
        dressManager.listIconHat.Clear();
        AddIconBeanDictionaryByFolder("Assets/Texture/Character/Dress/Hat/", dressManager.listIconHat);
    }

    [MenuItem("Custom/List/AddDressClothes")]
    public static void AddDressClothes()
    {
        GameObject Target = Selection.gameObjects[0];
        CharacterDressManager dressManager = Target.GetComponent<CharacterDressManager>();
        dressManager.listIconClothes.Clear();
        AddIconBeanDictionaryByFolder("Assets/Texture/Character/Dress/Clothes/", dressManager.listIconClothes);
    }

    [MenuItem("Custom/List/AddDressShoes")]
    public static void AddDressShoes()
    {
        GameObject Target = Selection.gameObjects[0];
        CharacterDressManager dressManager = Target.GetComponent<CharacterDressManager>();
        dressManager.listIconShoes.Clear();
        AddIconBeanDictionaryByFolder("Assets/Texture/Character/Dress/Shoes/", dressManager.listIconShoes);
    }

    [MenuItem("Custom/List/AddBodyHair")]
    public static void AddBodyHair()
    {
        GameObject Target = Selection.gameObjects[0];
        CharacterBodyManager bodyManager = Target.GetComponent<CharacterBodyManager>();
        bodyManager.listIconBodyHair.Clear();
        AddIconBeanDictionaryByFile("Assets/Texture/Character/character_hair.png", bodyManager.listIconBodyHair);
    }

    [MenuItem("Custom/List/AddBodyEye")]
    public static void AddBodyEye()
    {
        GameObject Target = Selection.gameObjects[0];
        CharacterBodyManager bodyManager = Target.GetComponent<CharacterBodyManager>();
        bodyManager.listIconBodyEye.Clear();
        AddIconBeanDictionaryByFile("Assets/Texture/Character/character_eye.png", bodyManager.listIconBodyEye);
    }

    [MenuItem("Custom/List/AddBodyMouth")]
    public static void AddBodyMouth()
    {
        GameObject Target = Selection.gameObjects[0];
        CharacterBodyManager bodyManager = Target.GetComponent<CharacterBodyManager>();
        bodyManager.listIconBodyMouth.Clear();
        AddIconBeanDictionaryByFile("Assets/Texture/Character/character_mouth.png", bodyManager.listIconBodyMouth);
    }


    [MenuItem("Custom/List/AddFoodIcon")]
    public static void AddFood()
    {
        GameObject Target = Selection.gameObjects[0];
        InnFoodManager foodManager = Target.GetComponent<InnFoodManager>();
        foodManager.listFoodIcon.Clear();
        AddIconBeanDictionaryByFolder("Assets/Texture/Food/", foodManager.listFoodIcon);
    }

    [MenuItem("Custom/List/AddItems")]
    public static void AddItems()
    {
        GameObject Target = Selection.gameObjects[0];
        GameItemsManager itemsManager = Target.GetComponent<GameItemsManager>();
        itemsManager.listItemsIcon.Clear();
        AddIconBeanDictionaryByFolder("Assets/Texture/Items/", itemsManager.listItemsIcon);
    }


    [MenuItem("Custom/List/AddFurniture")]
    public static void AddFurniture()
    {
        GameObject Target = Selection.gameObjects[0];
        InnBuildManager innBuildManager = Target.GetComponent<InnBuildManager>();
        innBuildManager.listFurnitureIcon.Clear();
        AddIconBeanDictionaryByFolder("Assets/Texture/InnBuild/", innBuildManager.listFurnitureIcon);
    }

    [MenuItem("Custom/List/AddUI")]
    public static void AddUI()
    {
        GameObject Target = Selection.gameObjects[0];
        UIDataManager uiDataManager = Target.GetComponent<UIDataManager>();
        uiDataManager.listUIIcon.Clear();
        AddIconBeanDictionaryByFolder("Assets/Texture/Background/", uiDataManager.listUIIcon);
        AddIconBeanDictionaryByFolder("Assets/Texture/Common/", uiDataManager.listUIIcon);
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
