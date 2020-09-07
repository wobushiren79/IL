using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    [MenuItem("Custom/List/AddAudio")]
    public static void AddAudio()
    {
        GameObject Target = Selection.gameObjects[0];
        AudioManager audioManager = Target.GetComponent<AudioManager>();
        audioManager.listMusicData.Clear();
        audioManager.listSoundData.Clear();
        audioManager.listEnvironmentData.Clear();
        AddAudioBeanDictionaryByFolder("Assets/Audio/Music/", audioManager.listMusicData);
        AddAudioBeanDictionaryByFolder("Assets/Audio/Sound/", audioManager.listSoundData);
        AddAudioBeanDictionaryByFolder("Assets/Audio/Environment/", audioManager.listEnvironmentData);
    }
    [MenuItem("Custom/List/AddDressMask")]
    public static void AddDressMask()
    {
        GameObject Target = Selection.gameObjects[0];
        CharacterDressManager dressManager = Target.GetComponent<CharacterDressManager>();
        dressManager.listIconMask.Clear();
        AddIconBeanDictionaryByFolder("Assets/Texture/Character/Dress/Mask/", dressManager.listIconMask);


        dressManager.listMaskAnim.Clear();
        AddAnimBeanDictionaryByFolder("Assets/Anim/Animation/Equip/Mask/", dressManager.listMaskAnim);
    }

    [MenuItem("Custom/List/AddDressHat")]
    public static void AddDressHat()
    {
        GameObject Target = Selection.gameObjects[0];
        CharacterDressManager dressManager = Target.GetComponent<CharacterDressManager>();
        dressManager.listIconHat.Clear();
        AddIconBeanDictionaryByFolder("Assets/Texture/Character/Dress/Hat/", dressManager.listIconHat);

        dressManager.listHatAnim.Clear();
        AddAnimBeanDictionaryByFolder("Assets/Anim/Animation/Equip/Hat/", dressManager.listHatAnim);
    }

    [MenuItem("Custom/List/AddDressClothes")]
    public static void AddDressClothes()
    {
        GameObject Target = Selection.gameObjects[0];
        CharacterDressManager dressManager = Target.GetComponent<CharacterDressManager>();
        dressManager.listIconClothes.Clear();
        AddIconBeanDictionaryByFolder("Assets/Texture/Character/Dress/Clothes/", dressManager.listIconClothes);

        dressManager.listClothesAnim.Clear();
        AddAnimBeanDictionaryByFolder("Assets/Anim/Animation/Equip/Clothes/", dressManager.listClothesAnim);
    }

    [MenuItem("Custom/List/AddDressShoes")]
    public static void AddDressShoes()
    {
        GameObject Target = Selection.gameObjects[0];
        CharacterDressManager dressManager = Target.GetComponent<CharacterDressManager>();
        dressManager.listIconShoes.Clear();
        AddIconBeanDictionaryByFolder("Assets/Texture/Character/Dress/Shoes/", dressManager.listIconShoes);

        dressManager.listShoesAnim.Clear();
        AddAnimBeanDictionaryByFolder("Assets/Anim/Animation/Equip/Shoes/", dressManager.listShoesAnim);
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
        foodManager.listFoodAnim.Clear();
        AddIconBeanDictionaryByFolder("Assets/Texture/Food/", foodManager.listFoodIcon);
        AddAnimBeanDictionaryByFolder("Assets/Anim/Animation/Food/", foodManager.listFoodAnim);
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
        AddIconBeanDictionaryByFolder("Assets/Texture/Items/Accountant/", itemsManager.listItemsIcon);
        AddIconBeanDictionaryByFolder("Assets/Texture/Items/Accost/", itemsManager.listItemsIcon);
        AddIconBeanDictionaryByFolder("Assets/Texture/Items/Beater/", itemsManager.listItemsIcon);
        AddIconBeanDictionaryByFolder("Assets/Texture/Items/Medicine/", itemsManager.listItemsIcon);
        AddIconBeanDictionaryByFolder("Assets/Texture/Element/Ingredients/", itemsManager.listItemsIcon);

        itemsManager.listItemsAnim.Clear();
        AddAnimBeanDictionaryByFolder("Assets/Anim/Animation/Equip/Items/", itemsManager.listItemsAnim);
    }


    [MenuItem("Custom/List/AddBuildItemData")]
    public static void AddBuildItemData()
    {
        GameObject Target = Selection.gameObjects[0];
        InnBuildManager innBuildManager = Target.GetComponent<InnBuildManager>();
        innBuildManager.listFurnitureIcon.Clear();
        AddIconBeanDictionaryByFolder("Assets/Texture/InnBuild/TableAndChair/", innBuildManager.listFurnitureIcon);
        AddIconBeanDictionaryByFolder("Assets/Texture/InnBuild/Stove/", innBuildManager.listFurnitureIcon);
        AddIconBeanDictionaryByFolder("Assets/Texture/InnBuild/Counter/", innBuildManager.listFurnitureIcon);
        AddIconBeanDictionaryByFolder("Assets/Texture/InnBuild/Door/", innBuildManager.listFurnitureIcon);
        AddIconBeanDictionaryByFolder("Assets/Texture/InnBuild/Decoration/", innBuildManager.listFurnitureIcon);
        AddIconBeanDictionaryByFolder("Assets/Texture/InnBuild/Bed/", innBuildManager.listFurnitureIcon);
        AddIconBeanDictionaryByFolder("Assets/Texture/InnBuild/Stairs/", innBuildManager.listFurnitureIcon);

        innBuildManager.listFloorIcon.Clear();
        AddIconBeanDictionaryByFolder("Assets/Texture/Tile/Floor/", innBuildManager.listFloorIcon);
        innBuildManager.listWallIcon.Clear();
        AddIconBeanDictionaryByFolder("Assets/Texture/Tile/Wall/", innBuildManager.listWallIcon);

        innBuildManager.listFloorTile.Clear();
        AddTileBeanDictionaryByFolder("Assets/Tile/Tiles/Floor/", innBuildManager.listFloorTile);
        innBuildManager.listWallTile.Clear();
        AddTileBeanDictionaryByFolder("Assets/Tile/Tiles/Wall/", innBuildManager.listWallTile);

        innBuildManager.listFurnitureCpt.Clear();
        AddGameObjectDictionaryByFolder("Assets/Prefabs/BuildItem/Counter/", innBuildManager.listFurnitureCpt);
        AddGameObjectDictionaryByFolder("Assets/Prefabs/BuildItem/Decoration/", innBuildManager.listFurnitureCpt);
        AddGameObjectDictionaryByFolder("Assets/Prefabs/BuildItem/Door/", innBuildManager.listFurnitureCpt);
        AddGameObjectDictionaryByFolder("Assets/Prefabs/BuildItem/Floor/", innBuildManager.listFurnitureCpt);
        AddGameObjectDictionaryByFolder("Assets/Prefabs/BuildItem/Other/", innBuildManager.listFurnitureCpt);
        AddGameObjectDictionaryByFolder("Assets/Prefabs/BuildItem/Stove/", innBuildManager.listFurnitureCpt);
        AddGameObjectDictionaryByFolder("Assets/Prefabs/BuildItem/Table/", innBuildManager.listFurnitureCpt);
        AddGameObjectDictionaryByFolder("Assets/Prefabs/BuildItem/Wall/", innBuildManager.listFurnitureCpt);
        AddGameObjectDictionaryByFolder("Assets/Prefabs/BuildItem/Bed/", innBuildManager.listFurnitureCpt);
        AddGameObjectDictionaryByFolder("Assets/Prefabs/BuildItem/Stairs/", innBuildManager.listFurnitureCpt);
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
        AddIconBeanDictionaryByFolder("Assets/Texture/Common/UI/", iconDataManager.listIcon);
    }

    [MenuItem("Custom/List/AddEffect")]
    public static void AddEffect()
    {
        GameObject Target = Selection.gameObjects[0];
        EffectManager combatEffectManager = Target.GetComponent<EffectManager>();
        combatEffectManager.listEffectPS.Clear();
        AddGameObjectDictionaryByFolder("Assets/Prefabs/Effects/Common/", combatEffectManager.listEffectPS);
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
    /// <summary>
    /// 根据文件夹下所有文件添加字典
    /// </summary>
    /// <param name="folderPath"></param>
    /// <param name="mapFood"></param>
    public static void AddAnimBeanDictionaryByFolder(string folderPath, AnimBeanDictionary map)
    {
        FileInfo[] files = FileUtil.GetFilesByPath(folderPath);
        foreach (FileInfo item in files)
        {
            Object[] objs = AssetDatabase.LoadAllAssetsAtPath(folderPath + item.Name);
            objs.ToList().ForEach(obj =>
            {
                if (obj as AnimationClip != null)
                {
                    map.Add(obj.name, obj as AnimationClip);
                }
            });
        }
    }
    /// <summary>
    ///  根据文件夹下所有文件添加字典
    /// </summary>
    /// <param name="folderPath"></param>
    /// <param name="map"></param>
    public static void AddTileBeanDictionaryByFolder(string folderPath, TileBeanDictionary map)
    {
        FileInfo[] files = FileUtil.GetFilesByPath(folderPath);
        foreach (FileInfo item in files)
        {
            Object[] objs = AssetDatabase.LoadAllAssetsAtPath(folderPath + item.Name);
            objs.ToList().ForEach(obj =>
            {
                if (obj as TileBase != null)
                {
                    map.Add(obj.name, obj as TileBase);
                }
            });
        }
    }

    /// <summary>
    /// 根据文件夹下所有文件添加字典
    /// </summary>
    /// <param name="folderPath"></param>
    /// <param name="mapFood"></param>
    public static void AddAudioBeanDictionaryByFolder(string folderPath, AudioBeanDictionary map)
    {
        FileInfo[] files = FileUtil.GetFilesByPath(folderPath);
        foreach (FileInfo item in files)
        {
            Object[] objs = AssetDatabase.LoadAllAssetsAtPath(folderPath + item.Name);
            objs.ToList().ForEach(obj =>
            {
                if (obj as AudioClip != null)
                {
                    map.Add(obj.name, obj as AudioClip);
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
                map.Add(obj.name+"", obj as GameObject);
            }
        }
    }
}
