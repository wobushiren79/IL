using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class MiniGameDebateHandler : BaseMiniGameHandler<MiniGameDebateBuilder, MiniGameDebateBean>,
    UIMiniGameDebate.ICallBack
{
    //预备战斗时间
    public float preCombatTime = 0.5f;
    //战斗时间
    public float combatTime = 1f;
    //斗魅的UI
    public UIMiniGameDebate uiMiniGameDebate;
    public override void Awake()
    {
        builderName = "MiniGameDebateBuilder";
        base.Awake();
    }

    public override void InitGame(MiniGameDebateBean miniGameData)
    {
        base.InitGame(miniGameData);
        //创建角色
        miniGameBuilder.CreateAllCharacter(miniGameData.listUserGameData, miniGameData.listEnemyGameData, miniGameData.miniGamePosition);
        //设置摄像机位置
        GameControlHandler.Instance.StartControl<ControlForMiniGameDebateCpt>(GameControlHandler.ControlEnum.MiniGameDebate);
        GameControlHandler.Instance.manager.GetControl().SetFollowPosition(miniGameData.miniGamePosition);

        //打开倒计时UI
        OpenCountDownUI(miniGameData);
    }

    public override void StartGame()
    {
        base.StartGame();
        //打开UI
        uiMiniGameDebate = UIHandler.Instance.OpenUIAndCloseOther<UIMiniGameDebate>();
        ClearAllCard();
        uiMiniGameDebate.SetCallBack(this);
        uiMiniGameDebate.SetData(miniGameData);
        DrawCard();
    }

    public override void EndGame(MiniGameResultEnum gameResult)
    {
        base.EndGame(gameResult);
    }

    /// <summary>
    /// 抽卡
    /// </summary>
    public void DrawCard()
    {
        if (miniGameData.listUserCard.Count >= 5)
            return;
        List<ItemMiniGameDebateCardCpt.DebateCardTypeEnun> listUserDebate = new List<ItemMiniGameDebateCardCpt.DebateCardTypeEnun>();
        List<ItemMiniGameDebateCardCpt.DebateCardTypeEnun> listEnemyDebate = new List<ItemMiniGameDebateCardCpt.DebateCardTypeEnun>();
        for (int i = 0; i < 5 - miniGameData.listUserCard.Count; i++)
        {
            listUserDebate.Add(RandomUtil.GetRandomEnum<ItemMiniGameDebateCardCpt.DebateCardTypeEnun>());
        }
        for (int i = 0; i < 5 - miniGameData.listUserCard.Count; i++)
        {
            listEnemyDebate.Add(RandomUtil.GetRandomEnum<ItemMiniGameDebateCardCpt.DebateCardTypeEnun>());
        }
        uiMiniGameDebate.CreateCardItemList(listUserDebate, listEnemyDebate);
    }

    /// <summary>
    /// 清理所有卡片
    /// </summary>
    public void ClearAllCard()
    {
        miniGameData.listUserCard.Clear();
        miniGameData.listEnemyCard.Clear();
        uiMiniGameDebate.ClearAllCard();
    }

    /// <summary>
    /// 选择了卡片 开始战斗
    /// </summary>
    /// <param name="selectCard"></param>
    public void StartCombat(ItemMiniGameDebateCardCpt selectCard)
    {
        if (miniGameData.GetDebateStatus() != MiniGameDebateBean.DebateStatus.Idle)
            return;
        //设置状态为开始战斗
        miniGameData.SetDebateStatus(MiniGameDebateBean.DebateStatus.Combat);
        //获取敌人的卡片
        ItemMiniGameDebateCardCpt enemyCard = GetEnemyCombatCard();
        //检测谁胜利
        CheckCombatWinner(selectCard, enemyCard, out ItemMiniGameDebateCardCpt winner, out ItemMiniGameDebateCardCpt loser);
        //创建战斗动画
        uiMiniGameDebate.CreateCombatAnim(selectCard, enemyCard, winner, loser, preCombatTime, combatTime);
        //从卡堆中移除选择的卡
        miniGameData.listUserCard.Remove(selectCard);
        miniGameData.listEnemyCard.Remove(enemyCard);
        NpcAIMiniGameDebateCpt winnerNpc = null;
        NpcAIMiniGameDebateCpt loserNpc = null;
        if (winner == null || loser == null)
        {

        }
        else if (selectCard == winner)
        {
            winnerNpc = miniGameBuilder.GetUserCharacter();
            loserNpc = miniGameBuilder.GetEnemyCharacter();
        }
        else
        {
            winnerNpc = miniGameBuilder.GetEnemyCharacter();
            loserNpc = miniGameBuilder.GetUserCharacter();
        }
        //创建攻击粒子
        float allComabtTime = preCombatTime + combatTime;
        if (winnerNpc != null && loserNpc != null)
        {
            GameObject objWinEffect = miniGameBuilder.CreateCombatEffect(winnerNpc.transform.position + new Vector3(0, 0.5f));
            GameObject objLoseEffect = miniGameBuilder.CreateCombatEffect(loserNpc.transform.position + new Vector3(0, 0.5f));

            AnimForCombatEffect(objWinEffect, loserNpc.transform.position + new Vector3(0, 0.5f), allComabtTime);
            AnimForCombatEffect(objLoseEffect, (winnerNpc.transform.position + loserNpc.transform.position) / 2f + new Vector3(0, 0.5f), allComabtTime / 2f);
        }
        //战斗过程
        StartCoroutine(CoroutineForCombat(allComabtTime, winnerNpc, loserNpc));
    }

    /// <summary>
    /// 开始新的回合
    /// </summary>
    public void StartNewRound()
    {
        if (miniGameData.listUserCard.Count <= 2)
        {
            DrawCard();
        }
        miniGameData.SetDebateStatus(MiniGameDebateBean.DebateStatus.Idle);
    }

    /// <summary>
    /// 获取敌人战斗的卡片
    /// </summary>
    /// <returns></returns>
    public ItemMiniGameDebateCardCpt GetEnemyCombatCard()
    {
        return RandomUtil.GetRandomDataByList(miniGameData.listEnemyCard);
    }

    /// <summary>
    /// 检测胜利者
    /// </summary>
    /// <param name="userCard"></param>
    /// <param name="enemyCard"></param>
    /// <returns></returns>
    public void CheckCombatWinner(
        ItemMiniGameDebateCardCpt userCard, ItemMiniGameDebateCardCpt enemyCard,
        out ItemMiniGameDebateCardCpt winnerCard, out ItemMiniGameDebateCardCpt loserCard)
    {
        ItemMiniGameDebateCardCpt.DebateCardTypeEnun userCardType = userCard.debateCardType;
        ItemMiniGameDebateCardCpt.DebateCardTypeEnun enemyCardType = enemyCard.debateCardType;
        int result = (int)userCardType - (int)enemyCardType;
        if (result == -1 || result == 2)
        {
            winnerCard = userCard;
            loserCard = enemyCard;
        }
        else if (result == 0)
        {
            winnerCard = null;
            loserCard = null;
        }
        else
        {
            winnerCard = enemyCard;
            loserCard = userCard;
        }
    }

    /// <summary>
    /// 检测是否游戏结束
    /// </summary>
    /// <param name="isWinGame"></param>
    /// <returns></returns>
    public bool CheckIsGameOver(out MiniGameResultEnum gameResult)
    {
        gameResult = MiniGameResultEnum.Lose;
        NpcAIMiniGameDebateCpt userCharacter = miniGameBuilder.GetUserCharacter();
        NpcAIMiniGameDebateCpt enemyCharacter = miniGameBuilder.GetEnemyCharacter();
        if (userCharacter.characterMiniGameData.characterCurrentLife <= 0 || enemyCharacter.characterMiniGameData.characterCurrentLife <= 0)
        {
            if (userCharacter.characterMiniGameData.characterCurrentLife <= 0)
            {
                gameResult = MiniGameResultEnum.Lose;
            }
            else if (enemyCharacter.characterMiniGameData.characterCurrentLife <= 0)
            {
                gameResult = MiniGameResultEnum.Win;
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// 协程战斗
    /// </summary>
    /// <param name="time"></param>
    /// <param name="winner"></param>
    /// <param name="loser"></param>
    /// <returns></returns>
    public IEnumerator CoroutineForCombat(float time, NpcAIMiniGameDebateCpt winner, NpcAIMiniGameDebateCpt loser)
    {
        yield return new WaitForSeconds(time);
        if (winner != null && loser != null)
        {
            winner.characterData.GetAttributes( out CharacterAttributesBean characterAttributes);
            int damageNumber = characterAttributes.charm * 2;
            loser.UnderAttack(damageNumber);
        }
        uiMiniGameDebate.RefreshUI();
        //检测是否结束游戏
        if (CheckIsGameOver(out MiniGameResultEnum gameResult))
        {
            EndGame(gameResult);
        }
        else
        {
            StartNewRound();
        }
    }

    #region 倒计时UI回调
    public override void GamePreCountDownStart()
    {
        base.GamePreCountDownStart();
    }

    public override void GamePreCountDownEnd()
    {
        base.GamePreCountDownEnd();
        //开始游戏
        StartGame();
    }
    #endregion

    #region UI操作回调
    public void CombatAnimEnd()
    {

    }
    #endregion

    /// <summary>
    /// 战斗粒子动画
    /// </summary>
    /// <param name="objEffect"></param>
    /// <param name="targetPostion"></param>
    /// <param name="time"></param>
    private void AnimForCombatEffect(GameObject objEffect, Vector3 targetPostion, float time)
    {
        objEffect.transform
            .DOMove(targetPostion, time)
            .SetEase(Ease.Linear)
            .OnComplete
                (() =>
                {
                    Destroy(objEffect);
                });
    }
}