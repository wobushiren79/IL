using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class CharacterBodyBean
{
    public int sex;//性别 0未知，1男，2女，3中性
    public ColorBean skinColor;//皮肤颜色

    public string hair;//发型
    public ColorBean hairColor;//发型颜色

    public string eye;//眼睛
    public ColorBean eyeColor;//眼睛颜色

    public string mouth;//嘴巴
    public ColorBean mouthColor;//嘴巴颜色

    public int face;//面向 

    public CharacterBodyBean()
    {
        sex = 1;
        eye = "character_eye_0";
        mouth = "character_mouth_0";
        skinColor = ColorBean.White();
        hairColor = ColorBean.White();
        eyeColor = ColorBean.White();
        mouthColor = ColorBean.White();
    }

    public static void CreateRandomBodyByManager(CharacterBodyBean characterBody, CharacterBodyManager characterBodyManager)
    {
        //随机生成性别
        characterBody.sex = UnityEngine.Random.Range(1, 3);
        //随机生成头型
        IconBean itemHair = RandomUtil.GetRandomDataByIconBeanDictionary(characterBodyManager.listIconBodyHair);
        characterBody.hair = itemHair.key;
        characterBody.hairColor = ColorBean.Random();

        //随机生成眼睛
        IconBean itemEye = RandomUtil.GetRandomDataByIconBeanDictionary(characterBodyManager.listIconBodyEye);
        characterBody.eye = itemEye.key;
        characterBody.eyeColor = ColorBean.Random();

        //随机生成嘴巴
        IconBean itemMouth = RandomUtil.GetRandomDataByIconBeanDictionary(characterBodyManager.listIconBodyMouth);
        characterBody.mouth = itemMouth.key;
        characterBody.mouthColor = ColorBean.Random();

    }
}