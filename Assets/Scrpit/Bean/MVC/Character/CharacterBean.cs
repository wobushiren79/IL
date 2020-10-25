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
    /// 创建随机敌人数据
    /// </summary>
    /// <param name="characterBodyManager"></param>
    /// <returns></returns>
    public static CharacterBean CreateRandomEnemyData(CharacterBodyManager characterBodyManager,int baseLife, int baseAttributes,int equipLevel)
    {
        CharacterBean characterData = new CharacterBean();
        characterData.baseInfo.characterId = SystemUtil.GetUUID(SystemUtil.UUIDTypeEnum.N);
        characterData.baseInfo.characterType = (int)NpcTypeEnum.Other;
        //设置随机名字
        characterData.baseInfo.name = RandomUtil.GetRandomGenerateChineseWord(UnityEngine.Random.Range(2, 4));

        //随机身体数据
        characterData.body.CreateRandomBody(characterBodyManager);

        //根据性别装备服装
        GetRandomDressByLevel(equipLevel, out long randomHat, out long randomClothes, out long randomShoes);
        characterData.equips.clothesId = randomClothes;
        characterData.equips.shoesId = randomShoes;
        //生成随机能力
        characterData.attributes.CreateRandomData(
            baseLife - 50, baseLife + 50,
            0, 1,
            baseAttributes - 3, baseAttributes + 3,
            baseAttributes - 3, baseAttributes + 3,
            baseAttributes - 3, baseAttributes + 3,
            baseAttributes - 3, baseAttributes + 3,
            baseAttributes - 3, baseAttributes + 3,
            baseAttributes - 3, baseAttributes + 3);
        return characterData;
    }

    /// <summary>
    /// 创建随机工作者角色数据  用于黄金台招人
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static CharacterBean CreateRandomWorkerData(CharacterBodyManager characterBodyManager)
    {
        CharacterBean characterData = new CharacterBean();
        characterData.baseInfo.characterId = SystemUtil.GetUUID(SystemUtil.UUIDTypeEnum.N);
        characterData.baseInfo.characterType = (int)NpcTypeEnum.RecruitNormal;
        //设置随机名字
        characterData.baseInfo.name = RandomUtil.GetRandomGenerateChineseWord(UnityEngine.Random.Range(2, 4));
        //生成随机能力
        characterData.attributes.CreateRandomData(
            10, 50,
            50, 100,
            1, 5,
            1, 5,
            1, 5,
            1, 5,
            1, 5,
            1, 5);
        //随机身体数据
        characterData.body.CreateRandomBody(characterBodyManager);
        //根据性别装备服装
        GetRandomDressByLevel(0, out long randomHat, out long randomClothes, out long randomShoes);
        characterData.equips.clothesId = randomClothes;
        characterData.equips.shoesId = randomShoes;
        //根据能力生成工资
        characterData.attributes.CreatePriceByAttributes(out long priceL, out long priceM, out long priceS);
        characterData.baseInfo.priceL = priceL;
        characterData.baseInfo.priceM = priceM;
        characterData.baseInfo.priceS = priceS;
        return characterData;
    }

    /// <summary>
    /// 根据重金创建随机工作者角色数据
    /// 属性生产规则：
    /// 1.总计金钱越多能力越高 最高10
    /// 2.如果有银或者金 则可能抽中稀有角色能力上限15
    /// </summary>
    /// <param name="characterBodyManager"></param>
    /// <param name="findPriceL"></param>
    /// <param name="findPriceM"></param>
    /// <param name="findPriceS"></param>
    /// <returns></returns>
    public static CharacterBean CreateRandomWorkerDataByPrice(CharacterBodyManager characterBodyManager, long findPriceL, long findPriceM, long findPriceS)
    {
        CharacterBean characterData = new CharacterBean();
        //设置ID
        characterData.baseInfo.characterId = SystemUtil.GetUUID(SystemUtil.UUIDTypeEnum.N);
        //设置随机名字
        characterData.baseInfo.name = RandomUtil.GetRandomGenerateChineseWord(UnityEngine.Random.Range(2, 4));

        //生成随机能力
        long totalPoint = (findPriceS + findPriceM * 1000 + findPriceL * 10000) / 100;
        int maxLife = 20;
        int minLife = 10;

        int maxCook = 1;
        int minCook = 1;

        int maxSpeed = 1;
        int minSpeed = 1;

        int maxAccount = 1;
        int minAccount = 1;

        int maxCharm = 1;
        int minCharm = 1;

        int maxForce = 1;
        int minForce = 1;

        int maxLucky = 1;
        int minLucky = 1;
        //稀有角色
        float getRatePro = totalPoint * 0.001f + findPriceL * 0.05f + findPriceM * 0.002f;

        //抽中稀有
        if (UnityEngine.Random.Range(0f, 1f) <= getRatePro)
        {
            characterData.baseInfo.characterType = (int)NpcTypeEnum.RecruitRare;

            maxLife = 200;
            minLife = 100;

            maxCook = 15;
            minCook = 10;

            maxSpeed = 15;
            minSpeed = 10;

            maxAccount = 15;
            minAccount = 10;

            maxCharm = 15;
            minCharm = 10;

            maxForce = 15;
            minForce = 10;

            maxLucky = 15;
            minLucky = 10;
        }
        //没有抽中稀有
        else
        {
            characterData.baseInfo.characterType = (int)NpcTypeEnum.RecruitNormal;
            while (totalPoint > 0)
            {
                int type = UnityEngine.Random.Range(1, 8);
                switch (type)
                {
                    case 1:
                        maxLife += 10;
                        break;
                    case 2:
                        ChangeAttributes(maxCook, minCook, out maxCook, out minCook);
                        break;
                    case 3:
                        ChangeAttributes(maxSpeed, minSpeed, out maxSpeed, out minSpeed);
                        break;
                    case 4:
                        ChangeAttributes(maxAccount, minAccount, out maxAccount, out minAccount);
                        break;
                    case 5:
                        ChangeAttributes(maxCharm, minCharm, out maxCharm, out minCharm);
                        break;
                    case 6:
                        ChangeAttributes(maxForce, minForce, out maxForce, out minForce);
                        break;
                    case 7:
                        ChangeAttributes(maxLucky, minLucky, out maxLucky, out minLucky);
                        break;
                }
                totalPoint--;
            }
        }
        //if (findPriceL >= 10000)
        //{
        //    maxLife = 200;
        //    minLife = 200;

        //    maxCook = 15;
        //    minCook = 15;

        //    maxSpeed = 15;
        //    minSpeed = 15;

        //    maxAccount = 15;
        //    minAccount = 15;

        //    maxCharm = 15;
        //    minCharm = 15;

        //    maxForce = 15;
        //    minForce = 15;

        //    maxLucky = 15;
        //    minLucky = 15;
        //}

        //生成随机能力
        characterData.attributes.CreateRandomData(
            minLife, maxLife,
            50, 100,
            minCook, maxCook,
            minSpeed, maxSpeed,
            minAccount, maxAccount,
            minCharm, maxCharm,
            minForce, maxForce,
            minLucky, maxLucky);
        //随机身体数据
        characterData.body.CreateRandomBody(characterBodyManager);
        //根据性别装备服装
        if (characterData.body.sex == 1)
        {
            characterData.equips.clothesId = 210039;
        }
        else if (characterData.body.sex == 2)
        {
            characterData.equips.clothesId = 210040;
        }
        characterData.equips.shoesId = 310039;
        //根据能力生成工资
        characterData.attributes.CreatePriceByAttributes(out long priceL, out long priceM, out long priceS);
        characterData.baseInfo.priceL = priceL;
        characterData.baseInfo.priceM = priceM;
        characterData.baseInfo.priceS = priceS;
        return characterData;
    }

    /// <summary>
    /// 改变属性
    /// </summary>
    private static void ChangeAttributes(int maxAttribute, int minAttribute, out int outMaxAttribute, out int outMinAttribute)
    {
        maxAttribute++;
        if (maxAttribute > 10)
        {
            maxAttribute = 10;
            minAttribute++;
            if (minAttribute > 5)
                minAttribute = 5;
        }
        outMaxAttribute = maxAttribute;
        outMinAttribute = minAttribute;
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
            life = selfAttributes.life + equipAttributes.life,
            cook = selfAttributes.cook + equipAttributes.cook,
            speed = selfAttributes.speed + equipAttributes.speed,
            account = selfAttributes.account + equipAttributes.account,
            charm = selfAttributes.charm + equipAttributes.charm,
            force = selfAttributes.force + equipAttributes.force,
            lucky = selfAttributes.lucky + equipAttributes.lucky,
            loyal = selfAttributes.loyal + equipAttributes.loyal,
        };
    }

    /// <summary>
    /// 通过不同的等级随机获取衣服
    /// </summary>
    /// <param name="level"></param>
    public static void GetRandomDressByLevel(int level, out long randomHat, out long randomClothes, out long randomShoes)
    {
        string randomHatListStr = "";
        string randomClothesListStr = "";
        string randomShoesListStr = "";
        switch (level)
        {
            case 1:
                GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.HatForLevel1, out randomHatListStr);
                GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.ClothesForLevel1, out randomClothesListStr);
                GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.ShoesForLevel1, out randomShoesListStr);
                break;
            case 2:
                GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.HatForLevel2, out randomHatListStr);
                GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.ClothesForLevel2, out randomClothesListStr);
                GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.ShoesForLevel2, out randomShoesListStr);
                break;
            case 3:
                GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.HatForLevel3, out randomHatListStr);
                GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.ClothesForLevel3, out randomClothesListStr);
                GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.ShoesForLevel3, out randomShoesListStr);
                break;
            default:
                GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.HatForLevel0, out randomHatListStr);
                GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.ClothesForLevel0, out randomClothesListStr);
                GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.ShoesForLevel0, out randomShoesListStr);
                break;
        }
        randomHat = StringUtil.SplitAndRandomForLong(randomHatListStr, ',');
        randomClothes = StringUtil.SplitAndRandomForLong(randomClothesListStr, ',');
        randomShoes = StringUtil.SplitAndRandomForLong(randomShoesListStr, ',');
    }

    public void GetAttributes(GameItemsManager gameItemsManager, out CharacterAttributesBean totalAttributes)
    {
        GetAttributes(gameItemsManager, out totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
    }

    /// <summary>
    /// 计算员工请假概率
    /// </summary>
    /// <param name="gameItemsManager"></param>
    /// <param name="gameDataManager"></param>
    /// <returns></returns>
    public bool CalculationWorkerVacation(GameItemsManager gameItemsManager, GameDataManager gameDataManager)
    {
        //获取数据
        GetAttributes(gameItemsManager,
        out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
        float randomRate = UnityEngine.Random.Range(0f, 1f);
        float successRate = 0;
        //如果是掌柜
        if (this == gameDataManager.gameData.userCharacter)
        {
            successRate = 1;
        }
        else
        {
            successRate = 0.7f + totalAttributes.loyal * 0.003f;
        }

        if (successRate >= randomRate)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    /// <summary>
    /// 计算员工发呆概率和时间
    /// </summary>
    /// <param name="rate"></param>
    /// <param name="dazeTime"></param>
    public bool CalculationWorkerDaze(GameItemsManager gameItemsManager, GameDataManager gameDataManager)
    {
        //获取数据
        GetAttributes(gameItemsManager,
        out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
        float randomRate = UnityEngine.Random.Range(0f, 1f);
        float successRate = 0;
        //如果是掌柜
        if (this == gameDataManager.gameData.userCharacter)
        {
            successRate = 1;
        }
        else
        {
            successRate = 0.5f + totalAttributes.loyal * 0.005f;
        }

        if (successRate >= randomRate)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// 计算菜单研究经验加成
    /// </summary>
    /// <returns></returns>
    public long CalculationMenuResearchAddExp(GameItemsManager gameItemsManager)
    {
        //获取数据
        GetAttributes(gameItemsManager,
        out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
        return totalAttributes.cook;
    }
    /// <summary>
    /// 计算床的研究经验加成
    /// </summary>
    /// <param name="gameItemsManager"></param>
    /// <returns></returns>
    public long CalculationBedResearchAddExp(GameItemsManager gameItemsManager)
    {
        //获取数据
        GetAttributes(gameItemsManager,
        out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
        return totalAttributes.charm;
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
        float successRate = 0.5f + totalAttributes.charm * 0.004f + totalAttributes.lucky * 0.001f;
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
        float talkTime = 6.1f;
        //获取数据
        GetAttributes(gameItemsManager,
        out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
        talkTime = 6.1f - totalAttributes.charm * 0.06f;
        //数据修正 
        if (talkTime <= 0.1f)
        {
            //最低不能小于0.1秒
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
        if (foodTime < 0.3f)
        {
            foodTime = 0.3f;
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
        float cleanTime = 6;
        //获取数据
        GetAttributes(gameItemsManager,
        out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
        cleanTime -= (totalAttributes.speed * 0.06f);
        if (cleanTime < 0.3f)
        {
            cleanTime = 0.3f;
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
        float successRate = 0.7f + (totalAttributes.account * 0.003f);
        float randomTemp = UnityEngine.Random.Range(0f, 1f);
        if (randomTemp <= successRate)
        {
            //没有出错
            //计算额外获取百分比
            moreRate = 0f;
            return false;
        }
        else
        {
            //出错
            moreRate = 0.1f;
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
        float time = 6;
        GetAttributes(gameItemsManager,
        out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
        time -= totalAttributes.account * 0.06f;
        if (time < 0.3f)
            time = 0.3f;
        return time;
    }

    /// <summary>
    /// 计算打手打架时间
    /// </summary>
    /// <returns></returns>
    public float CalculationBeaterFightTime(GameItemsManager gameItemsManager)
    {
        GetAttributes(gameItemsManager,
        out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);

        float fightTime = 6;
        fightTime -= (totalAttributes.force * 0.05f);
        if (fightTime < 0.3f)
        {
            fightTime = 0.3f;
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
        return totalAttributes.force * 10;
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


    /// <summary>
    /// 计算派遣斗技场是否胜利
    /// </summary>
    /// <param name="gameItemsManager"></param>
    /// <returns></returns>
    public bool CalculationArenaSendWin(GameItemsManager gameItemsManager,MiniGameEnum miniGameType)
    {
        GetAttributes(gameItemsManager,
        out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
        int addAttributes = 0;
        switch (miniGameType)
        {
            case MiniGameEnum.Cooking:
                addAttributes = totalAttributes.cook;
                break;
            case MiniGameEnum.Barrage:
                addAttributes = totalAttributes.speed;
                break;
            case MiniGameEnum.Account:
                addAttributes = totalAttributes.account;
                break;
            case MiniGameEnum.Debate:
                addAttributes = totalAttributes.charm;
                break;
            case MiniGameEnum.Combat:
                addAttributes = totalAttributes.force;
                break;
        }
        float winRate = 0.25f + totalAttributes.lucky * 0.0025f + addAttributes * 0.0025f;

        if(UnityEngine.Random.Range(0f, 1f)> winRate)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// 计算派遣斗技场是否胜利
    /// </summary>
    /// <param name="gameItemsManager"></param>
    /// <returns></returns>
    public bool CalculationGuildSendWin(GameItemsManager gameItemsManager, MiniGameEnum miniGameType)
    {
        GetAttributes(gameItemsManager,
        out CharacterAttributesBean totalAttributes, out CharacterAttributesBean selfAttributes, out CharacterAttributesBean equipAttributes);
        int addAttributes = 0;
        switch (miniGameType)
        {
            case MiniGameEnum.Cooking:
                addAttributes = totalAttributes.cook;
                break;
            case MiniGameEnum.Barrage:
                addAttributes = totalAttributes.speed;
                break;
            case MiniGameEnum.Account:
                addAttributes = totalAttributes.account;
                break;
            case MiniGameEnum.Debate:
                addAttributes = totalAttributes.charm;
                break;
            case MiniGameEnum.Combat:
                addAttributes = totalAttributes.force;
                break;
        }
        float winRate = 0.25f + totalAttributes.lucky * 0.0025f + addAttributes * 0.0025f;

        if (UnityEngine.Random.Range(0f, 1f) > winRate)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

}