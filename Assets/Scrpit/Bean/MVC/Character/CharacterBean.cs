using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class CharacterBean
{
    //角色基础信息
    public CharacterBaseBean baseInfo = new CharacterBaseBean();
    //角色属性
    public CharacterAttributesBean attributes = new CharacterAttributesBean();
    //角色身体属性
    public CharacterBodyBean body = new CharacterBodyBean();
    //角色装备属性
    public CharacterEquipBean equips = new CharacterEquipBean();

    /// <summary>
    /// 创建随机角色数据 - 综合
    /// </summary>
    /// <returns></returns>
    public static CharacterBean CreateRandomDataForComplex()
    {
        return CreateRandomData(0);
    }


    /// <summary>
    /// 创建随机角色数据--厨师
    /// </summary>
    /// <returns></returns>
    public static CharacterBean CreateRandomDataForChef()
    {
        return CreateRandomData(1);
    }

    /// <summary>
    /// 创建随机角色数据--账房
    /// </summary>
    /// <returns></returns>
    public static CharacterBean CreateRandomDataForAccounting()
    {
        return CreateRandomData(2);
    }

    /// <summary>
    /// 获取随机属性 - 伙计
    /// </summary>
    /// <returns></returns>
    public static CharacterBean CreateRandomDataForWaiter()
    {
        return CreateRandomData(3);
    }

    /// <summary>
    /// 获取随机属性 - 吆喝
    /// </summary>
    /// <returns></returns>
    public static CharacterBean CreateRandomDataForAccost()
    {
        return CreateRandomData(4);
    }

    /// <summary>
    /// 获取随机属性 - 打手
    /// </summary>
    /// <returns></returns>
    public static CharacterBean CreateRandomDataForBeater()
    {
        return CreateRandomData(5);
    }

    /// <summary>
    /// 创建随机角色数据
    /// </summary>
    /// <param name="type">0综合 1厨师 2账房</param>
    /// <returns></returns>
    public static CharacterBean CreateRandomData(int type)
    {
        CharacterBean characterData = new CharacterBean();
        //设置随机名字
        characterData.baseInfo.name = RandomUtil.GetRandomGenerateChineseWord(2);
        //生成随机能力
        switch (type)
        {
            case 0:
                characterData.attributes = CharacterAttributesBean.CreateRandomDataForComplex();
                break;
            case 1:
                characterData.attributes = CharacterAttributesBean.CreateRandomDataForChef();
                break;
            case 2:
                characterData.attributes = CharacterAttributesBean.CreateRandomDataForAccounting();
                break;
            case 3:
                characterData.attributes = CharacterAttributesBean.CreateRandomDataForWaiter();
                break;
            case 4:
                characterData.attributes = CharacterAttributesBean.CreateRandomDataForAccost();
                break;
            case 5:
                characterData.attributes = CharacterAttributesBean.CreateRandomDataForBeater();
                break;
        }
        //根据能力生成工资
        characterData.baseInfo.CreatePriceByAttributes(characterData.attributes);
        return characterData;
    }
}