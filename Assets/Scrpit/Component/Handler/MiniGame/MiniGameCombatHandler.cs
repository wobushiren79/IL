using UnityEngine;
using UnityEditor;
using DG.Tweening;
using System.Collections.Generic;
using System;
using System.Collections;

public class MiniGameCombatHandler : BaseMiniGameHandler<MiniGameCombatBuilder, MiniGameCombatBean>, UIMiniGameCountDown.ICallBack, UIMiniGameCombat.ICallBack
{
    public enum MiniGameCombatStatusEnum
    {
        Rounding,//回合进行中
        OurRound,//我方回合
        EnemyRound,//敌方回合
    }
    protected GameItemsManager gameItemsManager;
    //战斗状态
    private MiniGameCombatStatusEnum mMiniGameCombatStatus;
    //回合的行动角色数据
    private NpcAIMiniGameCombatCpt mRoundActionCharacter;
    //回合对象角色数据
    private NpcAIMiniGameCombatCpt mRoundTargetCharacter;
    //回合对象选择序列
    private int mTargetSelectedPosition = 0;

    protected override void Awake()
    {
        base.Awake();
        gameItemsManager = Find<GameItemsManager>(ImportantTypeEnum.GameItemsManager);
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="gameCombatData"></param>
    public override void InitGame(MiniGameCombatBean gameCombatData)
    {
        base.InitGame(gameCombatData);
        if (gameCombatData == null)
        {
            LogUtil.Log("战斗游戏数据为NULL，无法初始化战斗游戏");
            return;
        }
        mTargetSelectedPosition = 0;
        //创建NPC
        miniGameBuilder.CreateAllCharacter(gameCombatData.combatPosition, gameCombatData.listUserGameData, gameCombatData.listEnemyGameData);
        //设置摄像机位置
        InitCameraPosition();
        //打开倒计时UI
        OpenCountDownUI(gameCombatData);
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public override void StartGame()
    {
        base.StartGame();
    }

    /// <summary>
    /// 结束战斗
    /// </summary>
    /// <param name="isWinGame"></param>
    public override void EndGame(bool isWinGame)
    {
        base.EndGame(isWinGame);
    }

    /// <summary>
    /// 设置摄像机位置
    /// </summary>
    public void InitCameraPosition()
    {
        controlHandler.StartControl(ControlHandler.ControlEnum.MiniGameCombat);
        SetCameraPosition(miniGameData.combatPosition);
    }

    /// <summary>
    /// 设置行动目标
    /// </summary>
    /// <param name="actionNpc"></param>
    public void SetRoundActionCharacter(NpcAIMiniGameCombatCpt actionNpc)
    {
        this.mRoundActionCharacter = actionNpc;
    }

    /// <summary>
    /// 设置目标NPC
    /// </summary>
    /// <param name="targetNpc"></param>
    public void SetRoundTargetCharacter(NpcAIMiniGameCombatCpt targetNpc)
    {
        this.mRoundTargetCharacter = targetNpc;
    }

    /// <summary>
    /// 设置回合状态
    /// </summary>
    /// <returns></returns>
    public void SetMiniGameCombatStatus(MiniGameCombatStatusEnum miniGameCombatStatus)
    {
        mMiniGameCombatStatus = miniGameCombatStatus;
    }

    /// <summary>
    /// 获取回合状态
    /// </summary>
    /// <returns></returns>
    public MiniGameCombatStatusEnum GetMiniGameCombatStatus()
    {
        return mMiniGameCombatStatus;
    }

    /// <summary>
    /// 获取战斗地点
    /// </summary>
    /// <returns></returns>
    public Vector3 GetMiniGameCombatPosition()
    {
        return miniGameData.combatPosition;
    }

    /// <summary>
    /// 获取某一个角色
    /// </summary>
    /// <param name="gameCharacterData"></param>
    public NpcAIMiniGameCombatCpt GetCharacter(MiniGameCharacterBean gameCharacterData)
    {
        List<NpcAIMiniGameCombatCpt> listCharacter = miniGameBuilder.GetAllCharacter();
        for (int i = 0; i < listCharacter.Count; i++)
        {
            NpcAIMiniGameCombatCpt itemNpc = listCharacter[i];
            if (itemNpc.characterMiniGameData == gameCharacterData)
            {
                return itemNpc;
            }
        }
        return null;
    }

    /// <summary>
    /// 选中一个角色
    /// </summary>
    /// <param name="npcAI"></param>
    public void SelectedCharacter(NpcAIMiniGameCombatCpt npcCpt)
    {
        //设置选中特效
        if (npcCpt != null)
        {
            npcCpt.SetSelected(true);
            //聚焦当前选中角色
            ControlForMiniGameCombatCpt controlForMiniGameCombat = (ControlForMiniGameCombatCpt)controlHandler.GetControl();
            controlForMiniGameCombat.SetCameraPosition(npcCpt.transform.position);
        }
    }

    /// <summary>
    /// 取消选中
    /// </summary>
    public void UnSelectedCharacter(NpcAIMiniGameCombatCpt npcCpt)
    {
        if (npcCpt != null)
            npcCpt.SetSelected(false);
    }

    /// <summary>
    /// 开始下一个回合
    /// </summary>
    public void StartNextRound()
    {
        //取消选中状态
        UnSelectedCharacter(mRoundActionCharacter);
        UnSelectedCharacter(mRoundTargetCharacter);

        //UI继续回合计时
        UIMiniGameCombat uiMiniGameCombat = (UIMiniGameCombat)uiGameManager.GetOpenUI();
        //关闭指令UI
        uiMiniGameCombat.CloseCommand();
        //设置当前角色重新开始
        uiMiniGameCombat.InitCharacterRound(mRoundActionCharacter.characterMiniGameData);
        uiMiniGameCombat.StartRound();

        mRoundActionCharacter = null;
        mRoundTargetCharacter = null;
    }

    /// <summary>
    /// 检测是否游戏结束
    /// </summary>
    /// <returns></returns>
    public bool CheckIsGameOver(out bool isWinGame)
    {
        isWinGame = false;
        List<NpcAIMiniGameCombatCpt> listOurNpc = miniGameBuilder.GetOurCharacter();
        List<NpcAIMiniGameCombatCpt> listEnemyNpc = miniGameBuilder.GetEnemyCharacter();
        bool isOurNpcAllDead = true;
        bool isEnemyNpcAllDead = true;
        foreach (NpcAIMiniGameCombatCpt itemNpc in listOurNpc)
        {
            if (itemNpc.characterMiniGameData.characterCurrentLife > 0)
            {
                isOurNpcAllDead = false;
            }
        }
        foreach (NpcAIMiniGameCombatCpt itemNpc in listEnemyNpc)
        {
            if (itemNpc.characterMiniGameData.characterCurrentLife > 0)
            {
                isEnemyNpcAllDead = false;
            }
        }
        if (isOurNpcAllDead)
        {
            isWinGame = false;
        }
        if (isEnemyNpcAllDead)
        {
            isWinGame = true;
        }

        //如果双方都没有全部死亡则游戏没有结束
        if (!isOurNpcAllDead && !isEnemyNpcAllDead)
        {
            return false ;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// 开始战斗
    /// </summary>
    /// <param name="resultsAccuracy"></param>
    /// <param name="resultsForce"></param>
    public void StartFight(float resultsAccuracy, float resultsForce)
    {
        StartCoroutine(CoroutineForFight(resultsAccuracy, resultsForce));
    }

    #region 倒计时UI回调
    public override void GamePreCountDownStart()
    {
        base.GamePreCountDownStart();
    }

    public override void GamePreCountDownEnd()
    {
        base.GamePreCountDownEnd();
        //打开游戏UI
        UIMiniGameCombat uiMiniGameCombat = (UIMiniGameCombat)uiGameManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.MiniGameCombat));
        uiMiniGameCombat.SetCallBack(this);
        uiMiniGameCombat.SetData(miniGameData);
        SetMiniGameCombatStatus(MiniGameCombatStatusEnum.Rounding);
        uiMiniGameCombat.StartRound();
        //开始游戏
        StartGame();
    }
    #endregion

    #region 游戏回调
    /// <summary>
    /// 角色的回合
    /// </summary>
    /// <param name="gameCharacterData"></param>
    public void CharacterRoundCombat(MiniGameCharacterForCombatBean gameCharacterData)
    {
        //初始化数据
        gameCharacterData.CombatInit();
        //获取角色
        NpcAIMiniGameCombatCpt npcCpt = GetCharacter(gameCharacterData);
        SetRoundActionCharacter(npcCpt);
        //取消之前的状态
        npcCpt.SetCombatStatus(0);
        //如果是敌方
        if (gameCharacterData.characterType == 0)
        {
            SetMiniGameCombatStatus(MiniGameCombatStatusEnum.EnemyRound);
            npcCpt.OpenAI();
        }
        //如果是友方
        else if (gameCharacterData.characterType == 1)
        {
            SetMiniGameCombatStatus(MiniGameCombatStatusEnum.OurRound);
            UIMiniGameCombat uiMiniGameCombat = (UIMiniGameCombat)uiGameManager.GetOpenUI();
            uiMiniGameCombat.OpenCommand();
        }
        //开启选中特效
        SelectedCharacter(npcCpt);
    }

    /// <summary>
    /// 指令 战斗
    /// </summary>
    /// <param name="details"></param>
    public void CommandFight(int details)
    {
        if (mRoundActionCharacter == null || mRoundActionCharacter.characterMiniGameData == null || mRoundActionCharacter.characterMiniGameData.characterType != 1)
            return;
        List<NpcAIMiniGameCombatCpt> listEnemy = miniGameBuilder.GetEnemyCharacter();
        switch (details)
        {
            //选择攻击
            case 0:
                //设置选中特效 默认选中第一个]
                SetRoundTargetCharacter(listEnemy[mTargetSelectedPosition]);
                SelectedCharacter(mRoundTargetCharacter);
                break;
            //交换 
            case 1:
                mTargetSelectedPosition++;
                if (mTargetSelectedPosition >= listEnemy.Count)
                    mTargetSelectedPosition = 0;
                //取消选中
                UnSelectedCharacter(mRoundTargetCharacter);
                //设置新目标
                SetRoundTargetCharacter(listEnemy[mTargetSelectedPosition]);
                SelectedCharacter(mRoundTargetCharacter);
                break;
            //确认
            case 2:
                //打开力道测试
                UIMiniGameCombat uiMiniGameCombat = (UIMiniGameCombat)uiGameManager.GetOpenUI();
                uiMiniGameCombat.OpenCombatPowerTest(mRoundActionCharacter.characterMiniGameData);
                break;
            //取消
            case 3:
                UnSelectedCharacter(mRoundTargetCharacter);
                SelectedCharacter(mRoundActionCharacter);
                SetRoundTargetCharacter(null);
                break;
        }

    }

    /// <summary>
    /// 指令防御
    /// </summary>
    /// <param name="details"></param>
    public void CommandDefend(int details)
    {
        if (mRoundActionCharacter == null || mRoundActionCharacter.characterMiniGameData == null)
            return;
        mRoundActionCharacter.characterMiniGameData.combatIsDefend = true;
        mRoundActionCharacter.SetCombatStatus(1);
        StartNextRound();
    }

    /// <summary>
    /// 力道测试结束
    /// </summary>
    /// <param name="resultsAccuracy"></param>
    /// <param name="resultsForce"></param>
    public void PowerTestEnd(float resultsAccuracy, float resultsForce)
    {
        StartFight(resultsAccuracy, resultsForce);
    }
    #endregion

    public IEnumerator CoroutineForFight(float resultsAccuracy, float resultsForce)
    {
        //获取属性
        mRoundActionCharacter.characterMiniGameData.characterData.GetAttributes(gameItemsManager, out CharacterAttributesBean characterAttributes);
        //让行动角色移动到被攻击对象面前
        Vector3 offsetPosition;
        //根据角色朝向决定位置
        if (mRoundTargetCharacter.GetCharacterFace() == 1)
            offsetPosition = new Vector3(-1, 0);
        else
            offsetPosition = new Vector3(1, 0);
        //记录之前的位置
        Vector3 oldPosition = mRoundActionCharacter.transform.position;
        //行动角色移动到目标角色面前
        mRoundActionCharacter.transform.DOMove(mRoundTargetCharacter.transform.position + offsetPosition, 0.5f);
        yield return new WaitForSeconds(0.5f);
        //计算闪避
        float dodgeRate = UnityEngine.Random.Range(0f, 1f);
        if (dodgeRate <= resultsAccuracy)
        {
            //如果角色防御了
            if (mRoundTargetCharacter.characterMiniGameData.combatIsDefend)
                resultsForce = resultsForce / 2f;
            //计算伤害
            int damage = (int)((resultsForce + 0.2f) * characterAttributes.force) * 2;
            //角色伤害
            mRoundTargetCharacter.UnderAttack(resultsForce, damage);
            //如果角色阵亡
            if (mRoundTargetCharacter.characterMiniGameData.characterCurrentLife <= 0)
            {
                //设置角色死亡
                mRoundTargetCharacter.SetCharacterDead();
                //移除这个角色
                if (miniGameBuilder.GetOurCharacter().Contains(mRoundTargetCharacter))
                    miniGameBuilder.GetOurCharacter().Remove(mRoundTargetCharacter);
                if (miniGameBuilder.GetEnemyCharacter().Contains(mRoundTargetCharacter))
                    miniGameBuilder.GetEnemyCharacter().Remove(mRoundTargetCharacter);
                //ui回合移除该角色
                UIMiniGameCombat uiMiniGameCombat = (UIMiniGameCombat)uiGameManager.GetOpenUI();
                uiMiniGameCombat.RemoveCharacterRound(mRoundTargetCharacter.characterMiniGameData);
                //检测是否游戏结束
                bool isGameOver = CheckIsGameOver(out bool isWinGame);
                if (isGameOver)
                    //结束游戏
                    EndGame(isWinGame);
            }
        }
        else
        {
            //角色闪避了
            mRoundTargetCharacter.ShowTextInfo(GameCommonInfo.GetUITextById(14001));
        }
        //行动角色回到自己的位置
        yield return new WaitForSeconds(0.5f);
        mRoundActionCharacter.transform.DOMove(oldPosition, 0.5f);
        yield return new WaitForSeconds(0.5f);
        StartNextRound();
    }
}