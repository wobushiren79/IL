using UnityEngine;
using UnityEditor;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
using System;

public class NpcAIMiniGameCombatCpt : BaseNpcAI
{
    //角色数据
    public MiniGameCharacterForCombatBean characterMiniGameData;
    //角色血量
    public CharacterLifeCpt characterLifeCpt;
    //角色出血特效
    public ParticleSystem psBlood;
    //伤害特效
    public ParticleSystem psDamage;
    //战斗状态
    public SpriteRenderer srCombatStatus;
    //伤害数字
    public GameObject objDamageModel;

    public override void SetCharacterDead()
    {
        base.SetCharacterDead();
        SetExpression(CharacterExpressionCpt.CharacterExpressionEnum.Dead, -1); ;
        //关闭血量展示
        characterLifeCpt.gameObject.SetActive(false);
        //移除所有效果
        foreach (MiniGameCombatEffectBean combatEffectData in characterMiniGameData.listCombatEffect)
        {
            RemoveStatusIconByMarkId(combatEffectData.iconMarkId);
        }
        characterMiniGameData.listCombatEffect.Clear();
    }

    /// <summary>
    /// 设置NPC数据
    /// </summary>
    /// <param name="characterMiniGameData"></param>
    public void SetData(MiniGameCharacterForCombatBean characterMiniGameData)
    {
        this.characterMiniGameData = characterMiniGameData;
        SetCharacterData(characterMiniGameData.characterData);
        //更新血量显示
        characterLifeCpt.SetData(characterMiniGameData.characterCurrentLife, characterMiniGameData.characterMaxLife);
    }

    /// <summary>
    /// 受到攻击
    /// </summary>
    /// <param name="powerLevel">伤害等级</param>
    /// <param name="damage"></param>
    public void UnderAttack(float powerLevel, int damage)
    {
        characterMiniGameData.AddLife(-damage);
        Color colorDamage;
        if (powerLevel >= 1f)
        {
            colorDamage = Color.red;
        }
        else
        {
            colorDamage = Color.gray;
        }
        ShowTextInfo("-" + damage, colorDamage);
        //流血特效
        if (damage != 0)
        {
            psBlood.Play();
            transform.DOKill();
            transform.localScale = new Vector3(1, 1, 1);
            transform.DOShakeScale(1, 0.5f);
        }
        //伤害特效
        psDamage.Play();
        //更新血量显示
        characterLifeCpt.SetData(characterMiniGameData.characterCurrentLife, characterMiniGameData.characterMaxLife);
    }

    /// <summary>
    /// 展示文字
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public GameObject ShowTextInfo(string text, Color colorText)
    {
        GameObject objItemDamage = Instantiate(gameObject, objDamageModel);
        JumpTextCpt jumpText = objItemDamage.GetComponent<JumpTextCpt>();
        jumpText.SetData(text, colorText);
        return objItemDamage;
    }

    /// <summary>
    /// 开启AI
    /// </summary>
    public void OpenAI()
    {
        //相对友方列表
        List<NpcAIMiniGameCombatCpt> relativeOurList = null;
        //相对敌人列表
        List<NpcAIMiniGameCombatCpt> relativeEnemyList = null;
        //如果是友方AI
        if (characterMiniGameData.characterType == 1)
        {
            relativeOurList = MiniGameHandler.Instance.handlerForCombat.miniGameBuilder.GetCharacter(1);
            relativeEnemyList = MiniGameHandler.Instance.handlerForCombat.miniGameBuilder.GetCharacter(0);
        }
        //如果是敌方AI
        else if (characterMiniGameData.characterType == 0)
        {
            relativeOurList = MiniGameHandler.Instance.handlerForCombat.miniGameBuilder.GetCharacter(0);
            relativeEnemyList = MiniGameHandler.Instance.handlerForCombat.miniGameBuilder.GetCharacter(1);
        }
        float intentRate = UnityEngine.Random.Range(0f, 1f);
        if (intentRate <= 0.5f)
        {
            //一定概率攻击
            StartCoroutine(CoroutineForIntentFight(relativeOurList, relativeEnemyList));
        }
        else
        {
            //一定概率使用技能
            List<long> skillIds = characterMiniGameData.characterData.attributes.listSkills;
            //如果没有技能 默认攻击
            if (skillIds == null || skillIds.Count == 0)
            {
                StartCoroutine(CoroutineForIntentFight(relativeOurList, relativeEnemyList));
            }
            else
            {
                Action<List<SkillInfoBean>> callBack = (listData) =>
                {
                    if (!CheckUtil.ListIsNull(listData))
                        StartCoroutine(CoroutineForIntentSkill(listData[0], relativeOurList, relativeEnemyList));
                };
                SkillInfoHandler.Instance.manager.GetSkillById(RandomUtil.GetRandomDataByList(skillIds), callBack);
            }
        }
    }

    /// <summary>
    /// 协程意图-攻击
    /// </summary>
    public IEnumerator CoroutineForIntentFight(List<NpcAIMiniGameCombatCpt> relativeOurList, List<NpcAIMiniGameCombatCpt> relativeEnemyList)
    {
        MiniGameHandler.Instance.handlerForCombat.miniGameData.SetRoundActionCommand(MiniGameCombatCommand.Fight);
        MiniGameHandler.Instance.handlerForCombat.miniGameData.SetRoundActionPowerTest(0.8f);
        //选择血最少的
        NpcAIMiniGameCombatCpt targetNpc = null;
        //for (int i = 0; i < relativeEnemyList.Count; i++)
        //{
        //    NpcAIMiniGameCombatCpt itemNPC = relativeEnemyList[i];
        //    if (targetNpc == null)
        //    {
        //        targetNpc = itemNPC;
        //    }
        //    else
        //    {
        //        //选择血少的
        //        if (targetNpc.characterMiniGameData.characterCurrentLife > itemNPC.characterMiniGameData.characterCurrentLife)
        //        {
        //            targetNpc = itemNPC;
        //        }
        //        else if (targetNpc.characterMiniGameData.characterCurrentLife == itemNPC.characterMiniGameData.characterCurrentLife)
        //        {
        //            if (Random.Range(0, 2) == 1)
        //            {
        //                targetNpc = itemNPC;
        //            }
        //        }
        //    }
        //}
        //随机选一个
        targetNpc = RandomUtil.GetRandomDataByList(relativeEnemyList);
        MiniGameHandler.Instance.handlerForCombat.miniGameData.SetRoundTargetCharacter(targetNpc);
        yield return new WaitForSeconds(1);
        MiniGameHandler.Instance.handlerForCombat.RoundForAction();
    }

    /// <summary>
    ///协程意图-技能
    /// </summary>
    public IEnumerator CoroutineForIntentSkill(SkillInfoBean skillInfo, List<NpcAIMiniGameCombatCpt> relativeOurList, List<NpcAIMiniGameCombatCpt> relativeEnemyList)
    {
        if (skillInfo==null)
        {
            //如果没有技能则战斗
            StartCoroutine(CoroutineForIntentFight(relativeOurList, relativeEnemyList));     
        }
        else
        {
            MiniGameHandler.Instance.handlerForCombat.miniGameData.SetRoundActionCommand(MiniGameCombatCommand.Skill);
            MiniGameHandler.Instance.handlerForCombat.miniGameData.SetRoundActionSkill(skillInfo);
            EffectDetailsEnumTools.GetEffectRange(skillInfo.effect_details, out int impactNumber, out int impactType);

            List<NpcAIMiniGameCombatCpt> listSelectTargetNpc = new List<NpcAIMiniGameCombatCpt>();
            if (impactType == 0)
            {
                //如果选择是选择自己
                listSelectTargetNpc.Add(this);
            }
            else if (impactType == 1)
            {
                //如果选择是选择友方
                listSelectTargetNpc.AddRange(GetSelectTargetByNumber(impactNumber, relativeOurList));
            }
            else if (impactType == 2)
            {
                //如果选择是选择敌人
                listSelectTargetNpc.AddRange(GetSelectTargetByNumber(impactNumber, relativeEnemyList));
            }
            MiniGameHandler.Instance.handlerForCombat.miniGameData.SetRoundTargetCharacter(listSelectTargetNpc);
            yield return new WaitForSeconds(1f);
            MiniGameHandler.Instance.handlerForCombat.RoundForAction();

        }
    }

    protected List<NpcAIMiniGameCombatCpt> GetSelectTargetByNumber(int number,List<NpcAIMiniGameCombatCpt> listNpc)
    {
        if (number == 0)
        {
            return listNpc;
        }else
        {
           return RandomUtil.GetRandomDataByListForNumberNR(listNpc, number);
        }
    }


    /// <summary>
    /// 战斗效果执行
    /// </summary>
    public IEnumerator CombatEffectExecute()
    {
        for (int i = 0; i < characterMiniGameData.listCombatEffect.Count; i++)
        {
            MiniGameCombatEffectBean itemCombatEffect = characterMiniGameData.listCombatEffect[i];
            CombatEffectExecute(itemCombatEffect, out bool isCharacterDead);
            //如果角色已经死了 则不进行一下操作
            if (isCharacterDead)
                break;
            //检测是否需要延迟播放
            if (itemCombatEffect.CheckIsDelay())
            {
                yield return new WaitForSeconds(0.5f);
            }
    
            itemCombatEffect.durationForRound--;
            if (itemCombatEffect.durationForRound <= 0)
            {
                //移除图标
                RemoveStatusIconByMarkId(itemCombatEffect.iconMarkId);
                //删除数据
                characterMiniGameData.listCombatEffect.Remove(itemCombatEffect);
                i--;
            }
        }
    }

    /// <summary>
    /// 战斗效果执行
    /// </summary>
    /// <param name="itemCombatEffect"></param>
    public void CombatEffectExecute(MiniGameCombatEffectBean itemCombatEffect,out bool isDead)
    {
        //完成数据
        itemCombatEffect.CompleteEffect(characterMiniGameData);
        //检测角色血量
        MiniGameHandler.Instance.handlerForCombat.CheckCharacterLife();

        if (characterMiniGameData.characterCurrentLife <= 0)
        {
            isDead = true;
        }
        else
        {
            isDead = false;
        }
    }

    /// <summary>
    /// 增加效果
    /// </summary>
    /// <param name="effectTypeData"></param>
    /// <param name="effectDetailsData"></param>
    public void AddCombatEffect(NpcAIMiniGameCombatCpt actionCharacter,string effectTypeData, string effectDetailsData,Sprite spIcon)
    {
        List<EffectTypeBean> listTypeData = EffectTypeEnumTools.GetListEffectData(effectTypeData);
        EffectDetailsEnumTools.GetEffectDetailsForCombat(effectDetailsData, out string effectPSName, out int durationForRound);
        MiniGameCombatEffectBean gameCombatEffectData = new MiniGameCombatEffectBean();
        gameCombatEffectData.listEffectTypeData = listTypeData;
        gameCombatEffectData.durationForRound = durationForRound;
        gameCombatEffectData.effectPSName = effectPSName;
        gameCombatEffectData.iconMarkId = SystemUtil.GetUUID(SystemUtil.UUIDTypeEnum.N);
        gameCombatEffectData.actionCharacter = actionCharacter;
        gameCombatEffectData.targetCharacter = this;

        foreach (EffectTypeBean itemType in listTypeData)
        {
            //设置技能的备用图标
            EffectTypeEnumTools.GetEffectDetails(itemType, spIcon);
            //如果是持续 则需要加上BUFF图标
            if (durationForRound > 0)
            {
                Sprite spEffect;
                if (itemType.spIconRemark!=null)
                {
                    spEffect = itemType.spIconRemark;
                }
                else
                {
                    spEffect = itemType.spIcon;
                }
                AddStatusIconForEffect(spEffect, Color.white, gameCombatEffectData.iconMarkId);
            }
        }
        //播放粒子特效
        MiniGameHandler.Instance.handlerForCombat.miniGameBuilder.CreateCombatEffect(effectPSName, transform.position + new Vector3(0, 0.5f));
        if (durationForRound > 0)
        {
            characterMiniGameData.listCombatEffect.Add(gameCombatEffectData);
        }
        else
        {
            //回合数小于0的立即执行
            CombatEffectExecute(gameCombatEffectData, out bool isDead);
        }
    }
}