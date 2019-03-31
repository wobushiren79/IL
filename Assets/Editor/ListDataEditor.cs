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
        AddList("Assets/Texture/Character/character_clothes.png", dressManager.listIconClothes);
    }

    [MenuItem("Custom/List/AddDressShoes")]
    public static void AddDressShoes()
    {
        GameObject Target = Selection.gameObjects[0];
        CharacterDressManager dressManager = Target.GetComponent<CharacterDressManager>();
        AddList("Assets/Texture/Character/character_shoes.png", dressManager.listIconShoes);
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
