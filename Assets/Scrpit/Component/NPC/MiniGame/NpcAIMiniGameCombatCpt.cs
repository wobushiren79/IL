using UnityEngine;
using UnityEditor;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
public class NpcAIMiniGameCombatCpt : BaseNpcAI
{
    //战斗游戏处理
    public MiniGameCombatHandler gameCombatHandler;
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
    }

    public override void SetCharacterDead()
    {
        base.SetCharacterDead();
        SetExpression(CharacterExpressionCpt.CharacterExpressionEnum.Dead, -1); ;
        //关闭血量展示
        characterLifeCpt.gameObject.SetActive(false);
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
        if (powerLevel > 1f)
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
        gameCombatHandler.miniGameData.SetPowerTest(1);
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
        gameCombatHandler.miniGameData.SetPowerTest(1);
        //随机选一个
        NpcAIMiniGameCombatCpt targetNpc = RandomUtil.GetRandomDataByList(relativeEnemyList);
        gameCombatHandler.miniGameData.SetRoundTargetCharacter(targetNpc);
        yield return new WaitForSeconds(2);
        gameCombatHandler.RoundForAction();
    }
}