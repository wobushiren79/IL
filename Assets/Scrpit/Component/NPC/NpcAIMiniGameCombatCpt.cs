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
    //选中特效
    public ParticleSystem psSelected;
    //战斗状态
    public SpriteRenderer srCombatStatus;
    //伤害数字
    public GameObject objDamageModel;

    //防御图标
    public Sprite spStatusDefend;

    public override void SetCharacterDead()
    {
        base.SetCharacterDead();
        SetExpression(CharacterExpressionCpt.CharacterExpressionEnum.Dead, -1);
        //清空状态
        SetCombatStatus(0);
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
    /// 设置选中
    /// </summary>
    /// <param name="isSelected"></param>
    public void SetSelected(bool isSelected)
    {
        if (isSelected)
        {
            psSelected.gameObject.SetActive(true);
            psSelected.Play();
        }
        else
            psSelected.gameObject.SetActive(false);
    }

    /// <summary>
    /// 设置战斗状态
    /// </summary>
    /// <param name="status">0,没有  1防御</param>
    public void SetCombatStatus(int status)
    {
        srCombatStatus.color = new Color(1, 1, 1, 1);
        switch (status)
        {
            case 0:
                srCombatStatus.color = new Color(1, 1, 1, 0);
                break;
            case 1:
                srCombatStatus.sprite = spStatusDefend;
                srCombatStatus.transform.localScale = new Vector3(1, 1, 1);
                srCombatStatus.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f).From().SetEase(Ease.OutBack);
                break;
            case 2:
                break;
        }

    }

    /// <summary>
    /// 受到攻击
    /// </summary>
    /// <param name="powerLevel">伤害等级</param>
    /// <param name="damage"></param>
    public void UnderAttack(float powerLevel, int damage)
    {
        characterMiniGameData.AddLife(-damage);

        TextMesh tvItem = ShowTextInfo("-" + damage);
        if (powerLevel > 0.8f)
        {
            tvItem.characterSize = 0.15f;
            tvItem.color = Color.red;
        }
        else
        {
            tvItem.characterSize = 0.1f;
            tvItem.color = Color.white;
        }
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
    public TextMesh ShowTextInfo(string text)
    {
        GameObject objItemDamage = Instantiate(gameObject, objDamageModel);
        TextMesh tvItem = objItemDamage.GetComponentInChildren<TextMesh>();
        tvItem.text = text + "";
        //数字特效
        objItemDamage.transform.DOScale(new Vector3(0, 0, 0), 1f).From().SetEase(Ease.OutElastic);
        objItemDamage.transform.DOLocalMoveY(1.5f, 2.5f).OnComplete(delegate ()
        {
            Destroy(objItemDamage);
        });
        DOTween.To(() => 1f, alpha => tvItem.color = new Color(tvItem.color.r, tvItem.color.g, tvItem.color.b, alpha), 0f, 1).SetDelay(4);
        return tvItem;
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
        //如果友方人数大于两人，自己的血量是友方最低 并且低于0.3辣么就防御 
        if (relativeOurList.Count >= 2)
        {
            bool isMinimumLife = true;
            float lifeRate = ((float)characterMiniGameData.characterCurrentLife / (float)characterMiniGameData.characterMaxLife);
            for (int i = 0; i < relativeOurList.Count; i++)
            {
                NpcAIMiniGameCombatCpt itemNpc = relativeOurList[i];
                if (itemNpc != this)
                {
                    if (characterMiniGameData.characterCurrentLife >= itemNpc.characterMiniGameData.characterCurrentLife)
                    {
                        isMinimumLife = false;
                    }
                }
            }
            //判断是否是最低血量 并且低于0.3
            if (isMinimumLife && lifeRate < 0.3f)
            {
                StartCoroutine(IntentForDefend());
                return;
            }
        }
        //如果不防御就攻击
        StartCoroutine(IntentForFight(relativeEnemyList));
    }

    /// <summary>
    /// 意图-攻击
    /// </summary>
    public IEnumerator IntentForFight(List<NpcAIMiniGameCombatCpt> relativeEnemyList)
    {
        //逻辑首先选择不防御的
        if (!CheckUtil.ListIsNull(relativeEnemyList))
        {
            List<NpcAIMiniGameCombatCpt> noDefendCharacter = new List<NpcAIMiniGameCombatCpt>();

            for (int i = 0; i < relativeEnemyList.Count; i++)
            {
                NpcAIMiniGameCombatCpt itemNPC = relativeEnemyList[i];
                if (!itemNPC.characterMiniGameData.combatIsDefend)
                {
                    noDefendCharacter.Add(itemNPC);
                }
            }
            //如果没有不防御的 则选择所有防御的人
            if (CheckUtil.ListIsNull(noDefendCharacter))
            {
                noDefendCharacter = relativeEnemyList;
            }
            //其次选择血最少的
            NpcAIMiniGameCombatCpt targetNpc = null;
            for (int i = 0; i < noDefendCharacter.Count; i++)
            {
                NpcAIMiniGameCombatCpt itemNPC = noDefendCharacter[i];
                if (targetNpc == null)
                {
                    targetNpc = itemNPC;
                }
                else
                {
                    //选择血少的
                    if (targetNpc.characterMiniGameData.characterCurrentLife > itemNPC.characterMiniGameData.characterCurrentLife)
                    {
                        targetNpc = itemNPC;
                    }
                    else if (targetNpc.characterMiniGameData.characterCurrentLife == itemNPC.characterMiniGameData.characterCurrentLife)
                    {
                        if (Random.Range(0, 2) == 1)
                        {
                            targetNpc = itemNPC;
                        }
                    }
                }
            }
            yield return new WaitForSeconds(1);
            gameCombatHandler.SetRoundTargetCharacter(targetNpc);
            gameCombatHandler.SelectedCharacter(targetNpc);
            gameCombatHandler.StartFight(1, 1);
        }
    }

    /// <summary>
    /// 意图-防御
    /// </summary>
    public IEnumerator IntentForDefend()
    {
        yield return new WaitForSeconds(2);
        gameCombatHandler.CommandDefend(0);
    }
    /// <summary>
    /// 意图-攻击
    /// </summary>
    public void IntentForItem()
    {

    }
}