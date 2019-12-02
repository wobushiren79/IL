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
    //NPC相关数据
    public NpcInfoBean npcInfoData = new NpcInfoBean();

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
            life = 100 + selfAttributes.life + equipAttributes.life,
            cook = selfAttributes.cook + equipAttributes.cook,
            speed = selfAttributes.speed + equipAttributes.speed,
            account = selfAttributes.account + equipAttributes.account,
            charm = selfAttributes.charm + equipAttributes.charm,
            force = selfAttributes.force + equipAttributes.force,
            lucky = selfAttributes.lucky + equipAttributes.lucky,
            loyal = selfAttributes.loyal + equipAttributes.loyal,
        };
    }

    public void GetAttributes(GameItemsManager gameItemsManager, out CharacterAttributesBean totalAttributes)
    {
        GetAttributes(gameItemsManager, out totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
    }

    /// <summary>
    ///  计算吆喝成功概率
    /// </summary>
    /// <returns></returns>
    public bool CalculationAccostRate(GameItemsManager gameItemsManager)
    {
        //获取数据
        GetAttributes(gameItemsManager,
        out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
        float randomRate = UnityEngine.Random.Range(0f, 1f);
        float successRate = 0.5f + totalAttributes.charm * 0.04f + totalAttributes.lucky * 0.01f;
        if (successRate >= randomRate)
            return true;
        else
            return false;
    }

    /// <summary>
    /// 计算吆喝聊天时间
    /// </summary>
    /// <returns></returns>
    public float CalculationAccostTalkTime(GameItemsManager gameItemsManager)
    {
        //默认10秒
        float talkTime = 10.1f;
        //获取数据
        GetAttributes(gameItemsManager,
        out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
        talkTime = 10.1f - totalAttributes.charm * 0.1f;
        //数据修正 
        if (talkTime <= 0.1f)
        {
            //最低不能小于0.5秒
            talkTime = 0.1f;
        }
        return talkTime;
    }

    /// <summary>
    /// 计算制作食物时间
    /// </summary>
    /// <returns></returns>
    public float CalculationChefMakeFoodTime(GameItemsManager gameItemsManager, float foodTime)
    {
        //获取数据
        GetAttributes(gameItemsManager,
        out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
        foodTime -= (foodTime * totalAttributes.cook * 0.01f);
        //时间修正
        if (foodTime <= 0.1f)
        {
            foodTime = 0.1f;
        }
        return foodTime;
    }

    /// <summary>
    /// 计算食物等级
    /// </summary>
    /// <returns></returns>
    public int CalculationChefFoodLevel(GameItemsManager gameItemsManager)
    {
        //获取数据
        GetAttributes(gameItemsManager,
        out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
        //完美食物
        float randomTemp = UnityEngine.Random.Range(0f, 1f);
        float foodRate = totalAttributes.cook * 0.0015f + totalAttributes.lucky * 0.0005f;
        if (randomTemp <= foodRate)
        {
            return 2;
        }
        //好食物
        randomTemp = UnityEngine.Random.Range(0f, 1f);
        foodRate = totalAttributes.cook * 0.006f + totalAttributes.lucky * 0.001f;
        if (randomTemp <= foodRate)
        {
            return 2;
        }
        //普通食物
        randomTemp = UnityEngine.Random.Range(0f, 1f);
        foodRate = 0.5f + totalAttributes.cook * 0.005f;
        if (randomTemp <= foodRate)
        {
            return 0;
        }
        //不好食物
        return -1;
    }

    /// <summary>
    /// 计算清理时间
    /// </summary>
    /// <returns></returns>
    public float CalculationWaiterCleanTime(GameItemsManager gameItemsManager)
    {
        float cleanTime = 10;
        //获取数据
        GetAttributes(gameItemsManager,
        out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
        cleanTime -= (totalAttributes.speed * 0.1f);
        if (cleanTime <= 0.1f)
        {
            cleanTime = 0.1f;
        }
        return cleanTime;
    }

    /// <summary>
    /// 计算账房结算
    /// </summary>
    public bool CalculationAccountingCheck(GameItemsManager gameItemsManager, out float moreRate)
    {
        //获取数据
        moreRate = 0;
        GetAttributes(gameItemsManager,
        out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
        float successRate = 0.5f + (totalAttributes.account * 0.005f);
        float randomTemp = UnityEngine.Random.Range(0f, 1f);
        if (randomTemp <= successRate)
        {
            //没有出错
            //计算额外获取百分比
            moreRate = (totalAttributes.account * 0.007f) + (totalAttributes.charm * 0.003f);
            if (moreRate < 0)
                moreRate = 0;
            return false;
        }
        else
        {
            //出错
            moreRate = 0.5f - (totalAttributes.account * 0.004f) - (totalAttributes.charm * 0.001f);
            if (moreRate < 0)
                moreRate = 0;
            return true;
        }
    }

    /// <summary>
    /// 计算结算时间
    /// </summary>
    /// <param name="gameItemsManager"></param>
    /// <returns></returns>
    public float CalculationAccountingTime(GameItemsManager gameItemsManager)
    {
        float time = 10;
        GetAttributes(gameItemsManager,
        out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
        time -= totalAttributes.account * 0.1f;
        if (time < 0.1f)
            time = 0.1f;
        return time;
    }

    /// <summary>
    /// 计算不同食物等级的加成汇率
    /// </summary>
    /// <param name="foodLevel"></param>
    /// <param name="foodLevelRate"></param>
    public float CalculationAccountingFoodLevel(int foodLevel)
    {
        float foodLevelRate = 0;
        switch (foodLevel)
        {
            case -1:
                foodLevelRate = -0.5f;
                break;
            case 0:
                foodLevelRate = 0;
                break;
            case 1:
                foodLevelRate = 1.5f;
                break;
            case 2:
                foodLevelRate = 1f;
                break;
        }
        return foodLevelRate;
    }

    /// <summary>
    /// 计算打手打架事件
    /// </summary>
    /// <returns></returns>
    public float CalculationBeaterFightTime(GameItemsManager gameItemsManager)
    {
        GetAttributes(gameItemsManager,
        out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);

        float fightTime = 10;
        fightTime -= (totalAttributes.force * 0.1f);
        if (fightTime < 0.1f)
        {
            fightTime = 0.1f;
        }
        return fightTime;
    }

    /// <summary>
    /// 计算打手攻击力
    /// </summary>
    /// <param name="gameItemsManager"></param>
    /// <returns></returns>
    public int CalculationBeaterDamage(GameItemsManager gameItemsManager)
    {
        GetAttributes(gameItemsManager,
        out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
        return totalAttributes.force;
    }

    /// <summary>
    /// 计算打手休息时间
    /// </summary>
    /// <param name="gameItemsManager"></param>
    /// <returns></returns>
    public float CalculationBeaterRestTime(GameItemsManager gameItemsManager)
    {
        GetAttributes(gameItemsManager,
        out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
        float restTime = 3 + (57 - 0.57f * totalAttributes.force);
        return restTime;
    }
}