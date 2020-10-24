﻿using UnityEngine;
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
    public CharacterAttributesBean GetEnemyAttributesByLayer(CharacterBean characterData,int layer)
    {
        CharacterAttributesBean characterAttributes = new CharacterAttributesBean();
        if (layer % 10 == 0)
        {
            int addRate = layer / 10;
            characterAttributes.InitAttributes(
                characterData.attributes.life * 10 * addRate,
                characterData.attributes.cook * addRate,
                characterData.attributes.speed * addRate,
                characterData.attributes.account * addRate,
                characterData.attributes.charm * addRate,
                characterData.attributes.force * addRate,
                characterData.attributes.lucky * addRate);
        }
        else
        {
            int baseAttributes = layer + 4;
            characterAttributes.InitAttributes(
                baseAttributes * 10,
                baseAttributes,
                baseAttributes + Random.Range(-1, 2),
                baseAttributes,
                baseAttributes,
                baseAttributes,
                baseAttributes);
        }
        return characterAttributes;
    }



    public List<CharacterBean> GetCharacterDataByInfiniteTowersLayer(NpcTeamManager npcTeamManager, NpcInfoManager npcInfoManager, CharacterBodyManager characterBodyManager, long layer)
    {
        List<CharacterBean> listData = new List<CharacterBean>();

        if (layer % 10 == 0)
        {
            //Boss层数
            string bossTeamMembers = "";
            switch (layer)
            {
                case 10:
                case 20:
                case 30:
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersBossForLevel1, out bossTeamMembers);
                    break;
                case 40:
                case 50:
                case 60:
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersBossForLevel2, out bossTeamMembers);
                    break;
                case 70:
                case 80:
                case 90:
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersBossForLevel3, out bossTeamMembers);
                    break;
                default:
                    GameCommonInfo.baseDataController.GetBaseData(BaseDataTypeEnum.InfiniteTowersBossForLevel4, out bossTeamMembers);
                    break;
            }
            long randomBossTeam = StringUtil.SplitAndRandomForLong(bossTeamMembers, '|');
            NpcTeamBean bossTeamData = npcTeamManager.GetInfiniteTowerBossTeam(randomBossTeam);
            long[] membersIds = bossTeamData.GetTeamMembersId();
            long[] bossId = bossTeamData.GetTeamLeaderId();
            foreach (long itemMemberId in membersIds)
            {
                CharacterBean memberData = npcInfoManager.GetCharacterDataById(itemMemberId);
                if (memberData != null)
                {
                    memberData.body.CreateRandomBody(characterBodyManager);
                    listData.Add(memberData);
                }
            }
            foreach (long itemBossId in bossId)
            {
                CharacterBean bossData = npcInfoManager.GetCharacterDataById(itemBossId);
                if (bossData != null)
                {
                    bossData.body.CreateRandomBody(characterBodyManager);
                    listData.Insert((membersIds.Length / 2), bossData);
                }   
            }
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



}