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
    /// 创建随机角色数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static CharacterBean CreateRandomData(WorkerEnum type)
    {
        CharacterBean characterData = new CharacterBean();
        //设置随机名字
        characterData.baseInfo.name = RandomUtil.GetRandomGenerateChineseWord(UnityEngine.Random.Range(2, 4));
        //生成随机能力
        switch (type)
        {
            case 0:
                characterData.attributes = CharacterAttributesBean.CreateRandomDataForComplex();
                break;
            case WorkerEnum.Chef:
                characterData.attributes = CharacterAttributesBean.CreateRandomDataForChef();
                break;
            case WorkerEnum.Waiter:
                characterData.attributes = CharacterAttributesBean.CreateRandomDataForWaiter();
                break;
            case WorkerEnum.Accounting:
                characterData.attributes = CharacterAttributesBean.CreateRandomDataForAccounting();
                break;
            case WorkerEnum.Accost:
                characterData.attributes = CharacterAttributesBean.CreateRandomDataForAccost();
                break;
            case WorkerEnum.Beater:
                characterData.attributes = CharacterAttributesBean.CreateRandomDataForBeater();
                break;
        }

        //根据能力生成工资
        characterData.baseInfo.CreatePriceByAttributes(characterData.attributes);
        return characterData;
    }

    /// <summary>
    /// 获取烹饪技能点数
    /// </summary>
    /// <param name="gameItemsManager"></param>
    /// <param name="totalAttributes"></param>
    /// <param name="selfAttributes"></param>
    /// <param name="equipAttributes"></param>
    public void GetAttributes(GameItemsManager gameItemsManager, 
        out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes)
    {
        selfAttributes = attributes;
        equipAttributes = equips.GetEquipAttributes(gameItemsManager);
        totalAttributes = new CharacterAttributesBean
        {
            cook = selfAttributes.cook + equipAttributes.cook,
            speed = selfAttributes.speed + equipAttributes.speed,
            account = selfAttributes.account + equipAttributes.account,
            charm = selfAttributes.charm + equipAttributes.charm,
            force = selfAttributes.force + equipAttributes.force,
            lucky = selfAttributes.lucky + equipAttributes.lucky,
            loyal = selfAttributes.loyal + equipAttributes.loyal,
        };
    }
}