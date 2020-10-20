using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneInfiniteTowersManager : BaseMonoBehaviour
{
    public Transform tfLayerForNormal_1;
    public Transform tfLayerForBoss_1;

    public List<SignForInfiniteTowersCpt> listSignForLayer = new List<SignForInfiniteTowersCpt>();

    /// <summary>
    /// 设置层数
    /// </summary>
    /// <param name="layer"></param>
    public void SetSignForLayer(long layer)
    {
        foreach (SignForInfiniteTowersCpt itemSign in listSignForLayer)
        {
            itemSign.SetData(layer + GameCommonInfo.GetUITextById(83));
        }
    }

    /// <summary>
    /// 获取普通战斗地点
    /// </summary>
    /// <returns></returns>
    public Vector3 GetNormalCombatPosition()
    {
        return tfLayerForNormal_1.position;
    }

    /// <summary>
    /// 获取BOSS战斗地点
    /// </summary>
    /// <returns></returns>
    public Vector3 GetBossCombatPosition()
    {
        return tfLayerForBoss_1.position;
    }

    /// <summary>
    /// 根据层数获取战斗地点
    /// </summary>
    /// <param name="layer"></param>
    /// <returns></returns>
    public Vector3 GetCombatPostionByLayer(long layer)
    {
        if (layer % 10 == 0)
        {
            return GetBossCombatPosition();
        }
        else
        {
            return GetNormalCombatPosition();
        }
    }

    /// <summary>
    /// 根据层数获取敌人能力
    /// </summary>
    /// <param name="layer"></param>
    /// <returns></returns>
    public CharacterAttributesBean GetEnemyAttributesByLayer(int layer)
    {
        CharacterAttributesBean characterAttributes = new CharacterAttributesBean();
        int baseAttributes = layer + 4;
        characterAttributes.InitAttributes(
            baseAttributes * 10,
            baseAttributes,
            baseAttributes + Random.Range(-1, 2),
            baseAttributes,
            baseAttributes,
            baseAttributes,
            baseAttributes);
        return characterAttributes;
    }

    /// <summary>
    /// 根据层数获取奖励
    /// </summary>
    /// <param name="layer"></param>
    /// <param name="normalBuildRate"></param>
    /// <returns></returns>
    public List<RewardTypeBean> GetRewardByLayer(int layer,int totalLucky)
    {
        List<RewardTypeBean> listReward = new List<RewardTypeBean>();
        long addExp = 0;
        long addMoneyS = 0;

        //获取稀有物品概率
        float normalBuildRate = 0.25f + 0.0025f * (totalLucky / 3f);
        float rateRate = 0.05f + 0.0005f * totalLucky;

        if (layer % 10 == 0)
        {
            //添加经验奖励
            addExp = layer * 10;
            //添加金钱奖励
            addMoneyS = layer * 100;
            //BOSS奖励
            string rewardForNormalStr = "";
            string rewardForRareStr = "";
            string rewardForNormalBuildStr = "";
            string rewardForRareBuildStr = "";
            switch (layer)
            {
                case 10:
                case 20:
                case 30:
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersNormalRewardForLevel1, out rewardForNormalStr);
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersRareRewardForLevel1, out rewardForRareStr);
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersNormalBuildRewardForLevel1, out rewardForNormalBuildStr);
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersRareBuildRewardForLevel1, out rewardForRareBuildStr);
                    break;
                case 40:
                case 50:
                case 60:
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersNormalRewardForLevel2, out rewardForNormalStr);
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersRareRewardForLevel2, out rewardForRareStr);
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersNormalBuildRewardForLevel2, out rewardForNormalBuildStr);
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersRareBuildRewardForLevel2, out rewardForRareBuildStr);
                    break;
                case 70:
                case 80:
                case 90:
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersNormalRewardForLevel3, out rewardForNormalStr);
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersRareRewardForLevel3, out rewardForRareStr);
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersNormalBuildRewardForLevel3, out rewardForNormalBuildStr);
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersRareBuildRewardForLevel3, out rewardForRareBuildStr);
                    break;
                case 100:
                default:
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersNormalRewardForLevel4, out rewardForNormalStr);
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersRareRewardForLevel4, out rewardForRareStr);
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersNormalBuildRewardForLevel4, out rewardForNormalBuildStr);
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersRareBuildRewardForLevel4, out rewardForRareBuildStr);
                    break;
            }
            if (CheckUtil.StringIsNull(rewardForNormalStr))
            {
                //必定随机获得一个物品
                RewardTypeBean rewardForItems = GetRandomRewardForData(RewardTypeEnum.AddItems, rewardForNormalStr);
                listReward.Add(rewardForItems);
                //有一定概率获得建筑物
                float randomTemp = Random.Range(0f, 1f);
                if (!CheckUtil.StringIsNull(rewardForNormalBuildStr) && randomTemp <= normalBuildRate)
                {
                    RewardTypeBean rewardForBuild = GetRandomRewardForData(RewardTypeEnum.AddBuildItems, rewardForNormalBuildStr);
                    listReward.Add(rewardForBuild);
                }
            }
            if (CheckUtil.StringIsNull(rewardForRareStr))
            {
                //有一定概率获得稀有物品
                float randomTemp = Random.Range(0f, 1f);
                if (randomTemp <= rateRate)
                {
                    RewardTypeBean rewardForItems = GetRandomRewardForData(RewardTypeEnum.AddItems, rewardForRareStr);
                    listReward.Add(rewardForItems);
                }
            }
        }
        else
        {
            //添加经验奖励
            addExp = layer;
            //添加金钱奖励
            addMoneyS = layer * 10;
        }

        RewardTypeBean rewardForExp = new RewardTypeBean(RewardTypeEnum.AddBeaterExp, addExp + "");
        listReward.Add(rewardForExp);
        RewardTypeBean rewardForMoneyS = new RewardTypeBean(RewardTypeEnum.AddMoneyS, addMoneyS + "");
        listReward.Add(rewardForMoneyS);

        return listReward;
    }

    /// <summary>
    /// 获取运气获取敌人装备
    /// </summary>
    /// <returns></returns>
    public List<RewardTypeBean> GetEnemyEquip(List<CharacterBean> listEnemyData,int layer, int totalLucky)
    {
        List<RewardTypeBean> listReward = new List<RewardTypeBean>();
        if (layer % 10 == 0)
        {
            float getRate = 0.1f + 0.001f * totalLucky;
            float randomRate = Random.Range(0f, 1f);
            if(randomRate <= getRate)
            {
                CharacterBean randomEnemy = RandomUtil.GetRandomDataByList(listEnemyData);
                if (randomEnemy.equips.hatId != 0)
                {
                    listReward.Add(new RewardTypeBean(RewardTypeEnum.AddItems, randomEnemy.equips.hatId + ""));
                }
                if (randomEnemy.equips.clothesId != 0)
                {
                    listReward.Add(new RewardTypeBean(RewardTypeEnum.AddItems, randomEnemy.equips.clothesId + ""));
                }
                if (randomEnemy.equips.shoesId != 0)
                {
                    listReward.Add(new RewardTypeBean(RewardTypeEnum.AddItems, randomEnemy.equips.shoesId + ""));
                }
            }
        }
        return listReward;
    }

    public List<CharacterBean> GetCharacterDataByInfiniteTowersLayer(NpcInfoManager npcInfoManager, CharacterBodyManager characterBodyManager, long layer)
    {
        List<CharacterBean> listData = new List<CharacterBean>();

        if (layer % 10 == 0)
        {
            //Boss层数

        }
        else
        {
            //普通层数
            List<CharacterBean> listCharacter = npcInfoManager.GetCharacterDataByType(NpcTypeEnum.GuestTeam);
            List<CharacterBean> listTempCharacter = new List<CharacterBean>();
            if (!CheckUtil.ListIsNull(listCharacter))
                for (int i = 0; i < listCharacter.Count; i++)
                {
                    CharacterBean itemCharacter = listCharacter[i];

                    //判断层数
                    int level = int.Parse(itemCharacter.npcInfoData.remark);
                    int layerLevel = Mathf.FloorToInt(layer / 10f);
                    if (layerLevel <= 15 && level == layerLevel)
                    {
                        listTempCharacter.Add(itemCharacter);
                    }
                }
            CharacterBean baseCharacterData = null;
            if (listTempCharacter.Count == 0)
            {
                baseCharacterData = RandomUtil.GetRandomDataByList(listCharacter);
            }
            else
            {
                baseCharacterData = RandomUtil.GetRandomDataByList(listTempCharacter);
            }

            //随机生成身体数据
            CharacterBean characterOne = ClassUtil.DeepCopyByReflect(baseCharacterData);
            characterOne.body.CreateRandomBody(characterBodyManager);
            listData.Add(characterOne);
            CharacterBean characterTwo = ClassUtil.DeepCopyByReflect(baseCharacterData);
            characterTwo.body.CreateRandomBody(characterBodyManager);
            listData.Add(characterTwo);
            CharacterBean characterThree = ClassUtil.DeepCopyByReflect(baseCharacterData);
            characterThree.body.CreateRandomBody(characterBodyManager);
            listData.Add(characterThree);

        }

        return listData;
    }


    protected RewardTypeBean GetRandomRewardForData(RewardTypeEnum rewardType, string rewardListStr)
    {
        if (CheckUtil.StringIsNull(rewardListStr))
        {
            return null;
        }
        List<string> listReward = StringUtil.SplitBySubstringForListStr(rewardListStr, '|');
        string randomReward = RandomUtil.GetRandomDataByList(listReward);
        RewardTypeBean rewardData = new RewardTypeBean(rewardType, randomReward + "");
        return rewardData;
    }
}