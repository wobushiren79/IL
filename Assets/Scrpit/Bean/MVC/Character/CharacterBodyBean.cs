﻿using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class CharacterBodyBean
{
    public int sex;//性别 0未知，1男，2女，3中性
    public string skin;
    public ColorBean skinColor;//皮肤颜色

    public string hair;//发型
    public ColorBean hairColor;//发型颜色

    public string eye;//眼睛
    public ColorBean eyeColor;//眼睛颜色

    public string mouth;//嘴巴
    public ColorBean mouthColor;//嘴巴颜色

    public int face;//面向 

    //生日
    public TimeBean timeForBirthday = new TimeBean();
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

    public void CreateRandomBody(CharacterBodyManager characterBodyManager)
    {
        //随机生成性别
        sex = UnityEngine.Random.Range(1, 3);
        //随机生成头型

        hair = characterBodyManager.GetRandomHairStr();
        hairColor = ColorBean.Random();

        //随机生成眼睛
        CreateRandomEye(characterBodyManager);

        //随机生成嘴巴
        mouth = characterBodyManager.GetRandomMouthStr();
        mouthColor = ColorBean.Random();
    }

    public void CreateRandomEye(CharacterBodyManager characterBodyManager)
    {
        //随机生成眼睛
        eye = characterBodyManager.GetRandomEyeStr();
        eyeColor = ColorBean.Random();
    }
}