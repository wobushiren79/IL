using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ImageEditor : Editor
{

    [MenuItem("Custom/Image/Single")]
    public static void Single()
    {
        BaseSpriteEditor(SpriteImportMode.Single, 0, 0);
    }

    [MenuItem("Custom/Image/Multiple_5x1")]
    public static void Multiple5x1()
    {
        BaseSpriteEditor(SpriteImportMode.Multiple, 5, 1);
    }

    [MenuItem("Custom/Image/Multiple_5x2")]
    public static void Multiple5x2()
    {
        BaseSpriteEditor(SpriteImportMode.Multiple, 5,2);
    }



    static void BaseSpriteEditor(SpriteImportMode spriteType,int cNumber, int rNumber)
    {
        Object[] objs = GetSelectedTextures();

        Selection.objects = new Object[0];

        if (objs.Length <= 0)
        {
            LogUtil.LogError("没有选中图片");
            return;
        }
        for (int i = 0; i < objs.Length; i++)
        {
            Texture2D itemTexture = (Texture2D)objs[i];
            string path = AssetDatabase.GetAssetPath(itemTexture);

            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;

            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.spriteImportMode = spriteType;
            textureImporter.filterMode = FilterMode.Point;
            textureImporter.maxTextureSize = 8192;
            textureImporter.compressionQuality = 100;
            textureImporter.isReadable = true;

            if (cNumber == 0 && rNumber == 0)
                continue;
            List<SpriteMetaData> newData = new List<SpriteMetaData>();

            float cItemSize = itemTexture.width / cNumber;
            float rItemSize = itemTexture.height / rNumber;
            int position = 0;
            for (int c = 0; c < cNumber; c++)
            {
                for (int r = rNumber; r > 0; r--)
                {
                    SpriteMetaData smd = new SpriteMetaData();
                    smd.pivot = new Vector2(0.5f, 0.5f);
                    smd.alignment = 9;
                    smd.name = itemTexture.name + "_" + position;
                    smd.rect = new Rect(c * cItemSize, (r - 1) * rItemSize, cItemSize, rItemSize);
                    newData.Add(smd);
                    position++;
                }
            }

            textureImporter.spritesheet = newData.ToArray();
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }
    }

    static Object[] GetSelectedTextures()
    {
        return Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets);
    }
}
