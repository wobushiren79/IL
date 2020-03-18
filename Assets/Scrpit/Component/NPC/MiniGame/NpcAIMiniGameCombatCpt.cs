using UnityEngine;
using UnityEditor;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
public class NpcAIMiniGameCombatCpt : BaseNpcAI
{
    //战斗游戏处理
    protected MiniGameCombatHandler gameCombatHandler;
    //战斗游戏创建
    protected MiniGameCombatBuilder gameCombatBuilder;

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

    public override void Awake()
    {
        base.Awake();
        gameCombatHandler = Find<MiniGameCombatHandler>(ImportantTypeEnum.MiniGameHandler);
        gameCombatBuilder = FindInChildren<MiniGameCombatBuilder>(ImportantTypeEnum.MiniGameBuilder);
    }

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
    }

    /// <summary>
    /// 受到攻击
    /// </summary>
    /// <param name="powerLevel">伤害等级</param>
    /// <param name="damage"></param>
    public void UnderAttack(float powerLevel, int damage)
    {
        characterMiniGameData.ChangeLife(-damage);
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
            relativeOurList = gameCombatHandler.miniGameBuilder.GetCharacter(1);
            relativeEnemyList = gameCombatHandler.miniGameBuilder.GetCharacter(0);
        }
        //如果是敌方AI
        else if (characterMiniGameData.characterType == 0)
        {
            relativeOurList = gameCombatHandler.miniGameBuilder.GetCharacter(0);
            relativeEnemyList = gameCombatHandler.miniGameBuilder.GetCharacter(1);
        }
        float intentRate = Random.Range(0f, 1f);
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
                StartCoroutine(CoroutineForIntentSkill(relativeOurList, relativeEnemyList));
            }
        }
    }

    /// <summary>
    /// 协程意图-攻击
    /// </summary>
    public IEnumerator CoroutineForIntentFight(List<NpcAIMiniGameCombatCpt> relativeOurList, List<NpcAIMiniGameCombatCpt> relativeEnemyList)
    {
        gameCombatHandler.miniGameData.SetRoundActionCommand(MiniGameCombatCommand.Fight);
        gameCombatHandler.miniGameData.SetRoundActionPowerTest(0.8f);
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
        gameCombatHandler.miniGameData.SetRoundTargetCharacter(targetNpc);
        yield return new WaitForSeconds(1);
        gameCombatHandler.RoundForAction();
    }

    /// <summary>
    ///协程意图-技能
    /// </summary>
    public IEnumerator CoroutineForIntentSkill(List<NpcAIMiniGameCombatCpt> relativeOurList, List<NpcAIMiniGameCombatCpt> relativeEnemyList)
    {
        gameCombatHandler.miniGameData.SetRoundActionCommand(MiniGameCombatCommand.Skill);
        //随机选一个
        NpcAIMiniGameCombatCpt targetNpc = RandomUtil.GetRandomDataByList(relativeEnemyList);
        gameCombatHandler.miniGameData.SetRoundTargetCharacter(targetNpc);
        yield return new WaitForSeconds(2);
        gameCombatHandler.RoundForAction();
    }

    /// <summary>
    /// 战斗效果执行
    /// </summary>
    public IEnumerator CombatEffectExecute()
    {
        for (int i = 0; i < characterMiniGameData.listCombatEffect.Count; i++)
        {
            MiniGameCombatEffectBean itemCombatEffect = characterMiniGameData.listCombatEffect[i];
            CombatEffectExecute(itemCombatEffect,out bool isCharacterDead);
            //如果角色已经死了 则不进行一下操作
            if (isCharacterDead)
                break;
            if (itemCombatEffect.durationForRound <= 0)
            {
                //移除图标
                RemoveStatusIconByMarkId(itemCombatEffect.iconMarkId);
                //删除数据
                characterMiniGameData.listCombatEffect.Remove(itemCombatEffect);
                i--;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    /// <summary>
    /// 战斗效果执行
    /// </summary>
    /// <param name="itemCombatEffect"></param>
    public void CombatEffectExecute(MiniGameCombatEffectBean itemCombatEffect,out bool isDead)
    {
        //播放粒子特效
        gameCombatBuilder.CreateCombatEffect(itemCombatEffect.effectPSName,transform.position + new Vector3(0,0.5f));
        //完成数据
        itemCombatEffect.CompleteEffect(characterMiniGameData,this);
        //检测角色血量
        gameCombatHandler.CheckCharacterLife();

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
    public void AddCombatEffect(string effectTypeData, string effectDetailsData)
    {
        List<EffectTypeBean> listTypeData = EffectTypeEnumTools.GetListEffectData(effectTypeData);
        EffectDetailsEnumTools.GetEffectDetailsForCombat(effectDetailsData, out string effectPSName, out int durationForRound);

        foreach (EffectTypeBean itemType in listTypeData)
        {
            EffectTypeEnumTools.GetEffectDetails(iconDataManager ,itemType);
            MiniGameCombatEffectBean gameCombatEffectData = new MiniGameCombatEffectBean();
            gameCombatEffectData.effectTypeData = itemType;
            gameCombatEffectData.durationForRound = durationForRound;
            gameCombatEffectData.effectPSName = effectPSName;
            gameCombatEffectData.iconMarkId = SystemUtil.GetUUID( SystemUtil.UUIDTypeEnum.N);
            //回合数小于0则是立即执行效果
            if (gameCombatEffectData.durationForRound <= 0)
            {
                CombatEffectExecute(gameCombatEffectData,out bool isDead);
                if (isDead)
                    return;
            }
            else
            {
                AddStatusIconForEffect(itemType.spIcon, Color.white, gameCombatEffectData.iconMarkId);
                characterMiniGameData.listCombatEffect.Add(gameCombatEffectData);
            }
        }
    }
}