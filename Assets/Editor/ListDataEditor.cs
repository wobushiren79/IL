using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ListDataEditor : Editor
{
    [MenuItem("Custom/List/AddCharacterData")]
    public static void AddCharacterData()
    {
        AddDressMask();
        AddDressHat();
        AddDressClothes();
        AddDressShoes();

        AddBodyHair();
        AddBodyEye();
        AddBodyMouth();
    }

    [MenuItem("Custom/List/AddDressMask")]
    public static void AddDressMask()
    {
        GameObject Target = Selection.gameObjects[0];
        CharacterDressManager dressManager = Target.GetComponent<CharacterDressManager>();
        dressManager.listIconMask.Clear();
        AddIconBeanDictionaryByFolder("Assets/Texture/Character/Dress/Mask/", dressManager.listIconMask);
    }

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
        AddIconBeanDictionaryByFolder("Assets/Texture/Character/Hair/", bodyManager.listIconBodyHair);
        //AddIconBeanDictionaryByFile("Assets/Texture/Character/character_hair.png", bodyManager.listIconBodyHair);
    }

    [MenuItem("Custom/List/AddBodyEye")]
    public static void AddBodyEye()
    {
        GameObject Target = Selection.gameObjects[0];
        CharacterBodyManager bodyManager = Target.GetComponent<CharacterBodyManager>();
        bodyManager.listIconBodyEye.Clear();
        AddIconBeanDictionaryByFolder("Assets/Texture/Character/Eye/", bodyManager.listIconBodyEye);
        //AddIconBeanDictionaryByFile("Assets/Texture/Character/character_eye.png", bodyManager.listIconBodyEye);
    }

    [MenuItem("Custom/List/AddBodyMouth")]
    public static void AddBodyMouth()
    {
        GameObject Target = Selection.gameObjects[0];
        CharacterBodyManager bodyManager = Target.GetComponent<CharacterBodyManager>();
        bodyManager.listIconBodyMouth.Clear();
        AddIconBeanDictionaryByFolder("Assets/Texture/Character/Mouth/", bodyManager.listIconBodyMouth);
        //AddIconBeanDictionaryByFile("Assets/Texture/Character/character_mouth.png", bodyManager.listIconBodyMouth);
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
        AddIconBeanDictionaryByFolder("Assets/Texture/Items/Chef/", itemsManager.listItemsIcon);
        AddIconBeanDictionaryByFolder("Assets/Texture/Items/Waiter/", itemsManager.listItemsIcon);
        AddIconBeanDictionaryByFolder("Assets/Texture/Items/Accounting/", itemsManager.listItemsIcon);
        AddIconBeanDictionaryByFolder("Assets/Texture/Items/Accost/", itemsManager.listItemsIcon);
        AddIconBeanDictionaryByFolder("Assets/Texture/Items/Beater/", itemsManager.listItemsIcon);
    }


    [MenuItem("Custom/List/AddFurnitureIcon")]
    public static void AddFurnitureIcon()
    {
        GameObject Target = Selection.gameObjects[0];
        InnBuildManager innBuildManager = Target.GetComponent<InnBuildManager>();
        innBuildManager.listFurnitureIcon.Clear();
        AddIconBeanDictionaryByFolder("Assets/Texture/InnBuild/TableAndChair/", innBuildManager.listFurnitureIcon);
        AddIconBeanDictionaryByFolder("Assets/Texture/InnBuild/Stove/", innBuildManager.listFurnitureIcon);
        AddIconBeanDictionaryByFolder("Assets/Texture/InnBuild/Counter/", innBuildManager.listFurnitureIcon);
        AddIconBeanDictionaryByFolder("Assets/Texture/InnBuild/Door/", innBuildManager.listFurnitureIcon);
    }

    //[MenuItem("Custom/List/AddFurnitureCpt")]
    //public static void AddFurnitureCpt()
    //{
    //    GameObject Target = Selection.gameObjects[0];
    //    InnBuildManager innBuildManager = Target.GetComponent<InnBuildManager>();
    //    innBuildManager.listFurnitureCpt.Clear();
    //    AddGameObjectDictionaryByFolder("Assets/Prefabs/BuildItem/Table/", innBuildManager.listFurnitureCpt);
    //    AddGameObjectDictionaryByFolder("Assets/Prefabs/BuildItem/Stove/", innBuildManager.listFurnitureCpt);
    //    AddGameObjectDictionaryByFolder("Assets/Prefabs/BuildItem/Counter/", innBuildManager.listFurnitureCpt);
    //    AddGameObjectDictionaryByFolder("Assets/Prefabs/BuildItem/Door/", innBuildManager.listFurnitureCpt);
    //    AddGameObjectDictionaryByFolder("Assets/Prefabs/BuildItem/Other/", innBuildManager.listFurnitureCpt);
    //}

    [MenuItem("Custom/List/AddIcon")]
    public static void AddUI()
    {
        GameObject Target = Selection.gameObjects[0];
        IconDataManager iconDataManager = Target.GetComponent<IconDataManager>();
        iconDataManager.listIcon.Clear();
        //AddIconBeanDictionaryByFolder("Assets/Texture/Background/", iconDataManager.listIcon);
        //AddIconBeanDictionaryByFolder("Assets/Texture/Common/", iconDataManager.listIcon);
        AddIconBeanDictionaryByFolder("Assets/Texture/Element/Ingredients/", iconDataManager.listIcon);
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

    public static void AddGameObjectDictionaryByFolder(string folderPath, GameObjectDictionary map)
    {
        FileInfo[] files = FileUtil.GetFilesByPath(folderPath);
        foreach (FileInfo item in files)
        {
            Object obj = AssetDatabase.LoadMainAssetAtPath(folderPath + item.Name);
            if (obj as GameObject != null)
            {
                BaseBuildItemCpt buildItemCpt = ((GameObject)obj).GetComponent<BaseBuildItemCpt>();
                map.Add(buildItemCpt.buildItemData.id, obj as GameObject);
            }
        }
    }
}
