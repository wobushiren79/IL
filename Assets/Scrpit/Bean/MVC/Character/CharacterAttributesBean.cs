using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

[Serializable]
public class CharacterAttributesBean
{
    public int loyal;//忠诚

    public int cook;// 厨力
    public int speed;// 速度
    public int account;// 计算
    public int charm;// 魅力
    public int force;// 武力
    public int lucky;// 幸运


    /// <summary>
    /// 获取随机属性 - 厨师
    /// </summary>
    /// <returns></returns>
    public static CharacterAttributesBean CreateRandomDataForChef()
    {
        return CreateRandomData(60, 100, 4, 5, 0, 2, 0, 2, 0, 2, 0, 2, 0, 2);
    }

    /// <summary>
    /// 获取随机属性 - 账房
    /// </summary>
    /// <returns></returns>
    public static CharacterAttributesBean CreateRandomDataForAccounting()
    {
        return CreateRandomData(60, 100, 0, 2, 0, 2, 4, 5, 0, 2, 0, 2, 0, 2);
    }

    /// <summary>
    /// 获取随机属性 - 伙计
    /// </summary>
    /// <returns></returns>
    public static CharacterAttributesBean CreateRandomDataForWaiter()
    {
        return CreateRandomData(60, 100, 0, 2, 4, 5, 0, 2, 2, 3, 0, 2, 0, 2);
    }

    /// <summary>
    /// 获取随机属性 - 吆喝
    /// </summary>
    /// <returns></returns>
    public static CharacterAttributesBean CreateRandomDataForAccost()
    {
        return CreateRandomData(60, 100, 0, 2, 2, 3, 0, 2, 4, 5, 0, 2, 0, 2);
    }

    /// <summary>
    /// 获取随机属性 - 打手
    /// </summary>
    /// <returns></returns>
    public static CharacterAttributesBean CreateRandomDataForBeater()
    {
        return CreateRandomData(60, 100, 0, 2, 0, 2, 0, 2, 0, 2, 4, 5, 0, 2);
    }

    /// <summary>
    /// 获取随机属性 - 综合
    /// </summary>
    /// <returns></returns>
    public static CharacterAttributesBean CreateRandomDataForComplex()
    {
        return CreateRandomData(60, 100, 1, 3, 1, 3, 1, 3, 1, 3, 1, 3, 1, 3);
    }

    public static CharacterAttributesBean CreateRandomData(
        int loyalMin, int loyalMax,
        int cookMin, int cookMax,
        int speedMin, int speedMax,
        int accountMin, int accountMax,
        int charmMin, int charmMax,
        int forceMin, int forceMax,
        int luckyMin, int luckyMax)
    {
        CharacterAttributesBean attributesBean = new CharacterAttributesBean();
        attributesBean.loyal = UnityEngine.Random.Range(loyalMin, loyalMax + 1);
        attributesBean.speed = UnityEngine.Random.Range(speedMin, speedMax + 1);
        attributesBean.account = UnityEngine.Random.Range(accountMin, accountMax + 1);
        attributesBean.charm = UnityEngine.Random.Range(charmMin, charmMax + 1);
        attributesBean.force = UnityEngine.Random.Range(forceMin, forceMax + 1);
        attributesBean.lucky = UnityEngine.Random.Range(luckyMin, luckyMax + 1);
        attributesBean.cook = UnityEngine.Random.Range(cookMin, cookMax + 1);
        return attributesBean;
    }
}