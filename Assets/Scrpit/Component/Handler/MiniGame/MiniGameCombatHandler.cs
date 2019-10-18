using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class MiniGameCombatHandler : BaseMiniGameHandler, UIMiniGameCountDown.ICallBack, UIMiniGameCombat.ICallBack
{
    public enum MiniGameCombatStatusEnum
    {
        Rounding,//回合进行中
        OurRound,//我方回合
        EnemyRound,//敌方回合
    }

    public GameItemsManager gameItemsManager;
    //游戏数据
    public MiniGameCombatBean gameCombatData;
    //生成器
    public MiniGameCombatBuilder gameCombatBuilder;
    //战斗状态
    private MiniGameCombatStatusEnum mMiniGameCombatStatus;
    //回合的行动角色数据
    private NpcAIMiniGameCombatCpt mRoundActionCharacter;
    //回合对象角色数据
    private NpcAIMiniGameCombatCpt mRoundTargetCharacter;
    //回合对象选择序列
    private int mTargetSelectedPosition = 0;

    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="gameCombatData"></param>
    public void InitData(MiniGameCombatBean gameCombatData)
    {
        SetMiniGameStatus(MiniGameStatusEnum.GamePre);
        if (gameCombatData == null)
        {
            LogUtil.Log("战斗游戏数据为NULL，无法初始化战斗游戏");
            return;
        }
        mTargetSelectedPosition = 0;
        this.gameCombatData = gameCombatData;
        //创建NPC
        gameCombatBuilder.CreateAllPlaer(gameCombatData.combatPosition, gameCombatData.listUserGameData, gameCombatData.listEnemyGameData);
        //设置摄像机位置
        controlHandler.StartControl(ControlHandler.ControlEnum.MiniGameCombat);
        controlHandler.GetControl().SetCameraPosition(gameCombatData.combatPosition);
        //打开倒计时UI
        OpenCountDownUI(gameCombatData);
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
    /// 开始游戏
    /// </summary>
    public void StartGame()
    {
        SetMiniGameStatus(MiniGameStatusEnum.Gameing);
    }

    /// <summary>
    /// 获取某一个角色
    /// </summary>
    /// <param name="gameCharacterData"></param>
    public NpcAIMiniGameCombatCpt GetCharacter(MiniGameCharacterBean gameCharacterData)
    {
        List<NpcAIMiniGameCombatCpt> listCharacter = gameCombatBuilder.GetAllCharacter();
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
    public void SelectedChangeCharacter(NpcAIMiniGameCombatCpt beforeNpc, NpcAIMiniGameCombatCpt currentNpc)
    {
        if (currentNpc == null)
            return;
        //设置选中特效
        if (beforeNpc != null)
            beforeNpc.SetSelected(false);
        if (currentNpc != null)
            currentNpc.SetSelected(true);
        //聚焦当前选中角色
        ControlForMiniGameCombatCpt controlForMiniGameCombat = (ControlForMiniGameCombatCpt)controlHandler.GetControl();
        controlForMiniGameCombat.SetCameraPosition(currentNpc.transform.position);
    }
    public void SelectedChangeCharacter(NpcAIMiniGameCombatCpt currentNpc)
    {
        SelectedChangeCharacter(null, currentNpc);
    }

    /// <summary>
    /// 开始下一个回合
    /// </summary>
    public void StartNextRound()
    {
        //UI继续回合计时
        UIMiniGameCombat uiMiniGameCombat = (UIMiniGameCombat)uiGameManager.GetOpenUI();
        //关闭指令UI
        uiMiniGameCombat.CloseCommand();
        //设置当前角色重新开始
        uiMiniGameCombat.StartNextRoundForCharacter(mRoundActionCharacter.characterMiniGameData);
        uiMiniGameCombat.StartRound();
        //关闭选中特效
        mRoundActionCharacter.SetSelected(false);
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
        uiMiniGameCombat.SetData(gameCombatData);
        SetMiniGameCombatStatus(MiniGameCombatStatusEnum.Rounding);
        uiMiniGameCombat.StartRound();
        //开始游戏
        StartGame();
    }
    #endregion

    #region 游戏回调
    public void CharacterRoundCombat(MiniGameCharacterBean gameCharacterData)
    {
        //初始化数据
        gameCharacterData.CombatInit();
        //如果是敌方
        if (gameCharacterData.characterType == 0)
        {
            SetMiniGameCombatStatus(MiniGameCombatStatusEnum.EnemyRound);

        }
        //如果是友方
        else if (gameCharacterData.characterType == 1)
        {
            SetMiniGameCombatStatus(MiniGameCombatStatusEnum.OurRound);
            UIMiniGameCombat uiMiniGameCombat = (UIMiniGameCombat)uiGameManager.GetOpenUI();
            uiMiniGameCombat.OpenCommand();
        }
        //设置角色为选中状态
        NpcAIMiniGameCombatCpt npcCpt = GetCharacter(gameCharacterData);
        npcCpt.SetCombatStatus(0);
        //开启选中特效
        SelectedChangeCharacter(npcCpt);
        mRoundActionCharacter = npcCpt;
    }

    /// <summary>
    /// 指令 战斗
    /// </summary>
    /// <param name="details"></param>
    public void CommandFight(int details)
    {
        if (mRoundActionCharacter == null || mRoundActionCharacter.characterMiniGameData == null || mRoundActionCharacter.characterMiniGameData.characterType != 1)
            return;
        List<NpcAIMiniGameCombatCpt> listEnemy = gameCombatBuilder.GetEnemyCharacter();
        switch (details)
        {
            //选择攻击
            case 0:
                //设置选中特效 默认选中第一个]
                mRoundTargetCharacter = listEnemy[mTargetSelectedPosition];
                SelectedChangeCharacter(null, mRoundTargetCharacter);
                break;
            //交换 
            case 1:
                mTargetSelectedPosition++;
                if (mTargetSelectedPosition >= listEnemy.Count)
                    mTargetSelectedPosition = 0;
                SelectedChangeCharacter(mRoundTargetCharacter, listEnemy[mTargetSelectedPosition]);
                mRoundTargetCharacter = listEnemy[mTargetSelectedPosition];
                break;
            //确认
            case 2:
                //打开力道测试
                UIMiniGameCombat uiMiniGameCombat = (UIMiniGameCombat)uiGameManager.GetOpenUI();
                uiMiniGameCombat.OpenCombatPowerTest(mRoundActionCharacter.characterMiniGameData);
                break;
            //取消
            case 3:
                SelectedChangeCharacter(mRoundTargetCharacter, mRoundActionCharacter);
                mRoundTargetCharacter = null;
                break;
        }

    }

    public void CommandDefend(int details)
    {
        if (mRoundActionCharacter == null || mRoundActionCharacter.characterMiniGameData == null || mRoundActionCharacter.characterMiniGameData.characterType != 1)
            return;
        mRoundActionCharacter.characterMiniGameData.combatIsDefend = true;
        mRoundActionCharacter.SetCombatStatus(1);
        StartNextRound();
    }

    public void PowerTestEnd(float resultsAccuracy, float resultsForce)
    {
        //获取属性
        mRoundActionCharacter.characterMiniGameData.characterData.GetAttributes(gameItemsManager, out CharacterAttributesBean characterAttributes);

        //计算闪避
        float dodgeRate = UnityEngine.Random.Range(0f,1f);
        if(dodgeRate <= resultsAccuracy)
        {
            //如果角色防御了
            if (mRoundTargetCharacter.characterMiniGameData.combatIsDefend)
                resultsAccuracy = resultsAccuracy / 2f;
            //计算伤害
            int damage = (int)((resultsForce + 0.2f) * characterAttributes.force) * 2;
            //角色伤害
            mRoundTargetCharacter.UnderAttack(resultsForce, damage);
        }
        else
        {
            mRoundTargetCharacter.ShowTextInfo(GameCommonInfo.GetUITextById(14001));
        }
        StartNextRound();
    }
    #endregion

}