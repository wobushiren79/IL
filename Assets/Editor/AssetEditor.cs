using System.IO;
using UnityEditor;
using UnityEngine;

public class AssetEditor : Editor
{

    [MenuItem("Custom/Asset/BuildAssetBundle")]
    public static void BuildAssetBundle()
    {

        //根据BuildSetting里面所激活的平台进行打包 设置过AssetBundleName的都会进行打包
        BuildPipeline.BuildAssetBundles("Assets/AssetBundles", BuildAssetBundleOptions.UncompressedAssetBundle, EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.Refresh();
        Debug.Log("打包完成");

    }

    [MenuItem("Custom/Asset/ReNamePuzzlesAsset")]
    public static void ReAssetName()
    {
        Object[] objs = Selection.objects;
        Selection.objects = new Object[0];
        for (int i = 0; i < objs.Length; i++)
        {
            string path = AssetDatabase.GetAssetPath(objs[i]);
            FileInfo dir = new FileInfo(path);
            string parent = dir.Directory.Name;
            AssetImporter.GetAtPath(path).assetBundleName = "puzzlespic/"+ parent + "/" + objs[i].name;
            if (i % 10 == 0)
            {
                bool isCancel = EditorUtility.DisplayCancelableProgressBar("修改中", path, (float)i / objs.Length);
                if (isCancel)
                {
                    EditorUtility.ClearProgressBar();
                    break;
                }
            }
        }
        EditorUtility.ClearProgressBar();
    }


    /// <summary>
    /// 清除之前设置过的AssetBundleName，避免产生不必要的资源也打包
    /// 之前说过，只要设置了AssetBundleName的，都会进行打包，不论在什么目录下
    /// </summary>
    [MenuItem("Custom/Asset/ClearAssetBundlesName")]
    public static void ClearAssetBundlesName()
    {
        int length = AssetDatabase.GetAllAssetBundleNames().Length;
        Debug.Log(length);
        string[] oldAssetBundleNames = new string[length];
        for (int i = 0; i < length; i++)
        {
            oldAssetBundleNames[i] = AssetDatabase.GetAllAssetBundleNames()[i];
        }

        for (int j = 0; j < oldAssetBundleNames.Length; j++)
        {
            AssetDatabase.RemoveAssetBundleName(oldAssetBundleNames[j], true);
        }
        length = AssetDatabase.GetAllAssetBundleNames().Length;
        Debug.Log(length);
    }

}
