using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class AnimEditor : Editor
{
    /// <summary>
    /// 创建图片的动画
    /// </summary>
    /// <param name="itemPicTex"></param>
    /// <param name="numberForS">1秒多少张</param>
     public static void CreateAnimForTex(Texture2D itemPicTex,string animPath, int numberForS)
    {
        AnimationClip clip = new AnimationClip();
        //设置帧率为30
        clip.frameRate = 30;

        //属性参数
        EditorCurveBinding curveBinding = new EditorCurveBinding();
        curveBinding.type = typeof(SpriteRenderer);
        curveBinding.path = "";
        curveBinding.propertyName = "m_Sprite";

        //帧数相关
        ObjectReferenceKeyframe[] keyFrames = null;

        string path = AssetDatabase.GetAssetPath(itemPicTex);
        Object[] objs = AssetDatabase.LoadAllAssetsAtPath(path);
        keyFrames = new ObjectReferenceKeyframe[objs.Length];
        //动画长度是按秒为单位，1/10就表示1秒切10张图片，根据项目的情况可以自己调节
        float frameTime = 1 / (float)numberForS;
        int i = 0;
        objs.ToList().ForEach(obj =>
        {
            if (obj as Sprite != null)
            {
                Sprite itemSprite = obj as Sprite;
                keyFrames[i] = new ObjectReferenceKeyframe();
                keyFrames[i].time = frameTime * i;
                keyFrames[i].value = itemSprite;
                i++;
            }
        });
        //最后结束再加一个第一帧
        keyFrames[i] = new ObjectReferenceKeyframe();
        keyFrames[i].time = frameTime * i;
        keyFrames[i].value = keyFrames[0].value;

        //设置idle文件为循环动画
        AnimationClipSettings clipSetting = AnimationUtility.GetAnimationClipSettings(clip);
        clipSetting.loopTime = !clip.isLooping;
        AnimationUtility.SetAnimationClipSettings(clip, clipSetting);
        //设置参数
        AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyFrames);
        AssetDatabase.CreateAsset(clip, animPath + "/" + itemPicTex.name + ".anim");
        AssetDatabase.SaveAssets();
    }

  
}