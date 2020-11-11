using UnityEngine;
using UnityEditor;
using DG.Tweening;
using System.Collections.Generic;
using System;
using System.Collections;
using static MiniGameCombatBean;

public class MiniGameCombatHandler : BaseMiniGameHandler<MiniGameCombatBuilder, MiniGameCombatBean>, UIMiniGameCountDown.ICallBack, UIMiniGameCombat.ICallBack
{
    protected SkillInfoHandler skillInfoHandler;
    //游戏UI
    protected UIMiniGameCombat uiMiniGameCombat;
    protected CharacterDressManager characterDressManager;
    protected override void Awake()
    {
        base.Awake();
        skillInfoHandler = Find<SkillInfoHandler>(ImportantTypeEnum.SkillHandler);
        characterDressManager = Find<CharacterDressManager>(ImportantTypeEnum.CharacterManager);
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
        //初始化技能
        skillInfoHandler.InitData();
        //创建NPC
        miniGameBuilder.CreateAllCharacter(gameCombatData.miniGamePosition, gameCombatData.listUserGameData, gameCombatData.listEnemyGameData);
        //设置摄像机位置
        InitCameraPosition();
        //打开倒计时UI
        OpenCountDownUI(gameCombatData,false);
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public override void StartGame()
    {
        base.StartGame();
        //打开游戏UI
        uiMiniGameCombat = (UIMiniGameCombat)uiGameManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.MiniGameCombat));
        uiMiniGameCombat.SetCallBack(this);
        uiMiniGameCombat.SetData(miniGameData);
        miniGameData.SetCombatStatus(MiniGameCombatStatusEnum.Rounding);
        uiMiniGameCombat.StartRound();
    }

    /// <summary>
    /// 结束战斗
    /// </summary>
    /// <param name="isWinGame"></param>
    public override void EndGame(MiniGameResultEnum gameResulte)
    {
        base.EndGame(gameResulte);
    }

    /// <summary>
    /// 设置摄像机位置
    /// </summary>
    public void InitCameraPosition()
    {
        controlHandler.StartControl(ControlHandler.ControlEnum.MiniGameCombat);
        SetCameraPosition(miniGameData.miniGamePosition);
    }

    /// <summary>
    /// 选择角色
    /// </summary>
    /// <param name="character"></param>
    public void SelectCharacter(NpcAIMiniGameCombatCpt character)
    {
        miniGameBuilder.DeleteSelectEffect();
        miniGameBuilder.CreateSelectEffect(character.transform.position);
        SetCameraPosition(character.transform.position);
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
    /// 开始下一个回合
    /// </summary>
    public void StartNextRound()
    {
        //关闭指令UI
        uiMiniGameCombat.CloseAll();
        //设置当前角色重新开始
        uiMiniGameCombat.InitCharacterRound(miniGameData.GetRoundActionCharacter().characterMiniGameData);
        //结束当前回合
        miniGameData.EndRound();
        //开始新回合
        uiMiniGameCombat.StartRound();
    }

    /// <summary>
    /// 检测是否游戏结束
    /// </summary>
    /// <returns></returns>
    public bool CheckIsGameOver(out MiniGameResultEnum gameResult)
    {
        gameResult =  MiniGameResultEnum.Lose;
        List<NpcAIMiniGameCombatCpt> listUserNpc = miniGameBuilder.GetUserCharacter();
        List<NpcAIMiniGameCombatCpt> listEnemyNpc = miniGameBuilder.GetEnemyCharacter();
        bool isOurNpcAllDead = true;
        bool isEnemyNpcAllDead = true;
        foreach (NpcAIMiniGameCombatCpt itemNpc in listUserNpc)
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
            gameResult = MiniGameResultEnum.Lose;
        }
        if (isEnemyNpcAllDead)
        {
            gameResult = MiniGameResultEnum.Win;
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
    /// 回合行动
    /// </summary>
    /// <param name="resultsAccuracy"></param>
    /// <param name="resultsForce"></param>
    public void RoundForAction()
    {
        //先调整镜头
        InitCameraPosition();
        StartCoroutine(CoroutineForAction());
    }

    /// <summary>
    /// 回合准备
    /// </summary>
    public IEnumerator RoundForPre(MiniGameCharacterForCombatBean gameCharacterData)
    {
        //获取角色
        NpcAIMiniGameCombatCpt npcCpt = GetCharacter(gameCharacterData);
        //设置当前回合行动的角色
        miniGameData.SetRoundActionCharacter(npcCpt);
        //执行效果
        yield return StartCoroutine(npcCpt.CombatEffectExecute());
        //如果角色死亡
        if (gameCharacterData.characterCurrentLife <= 0)
        {
            StartNextRound();
        }
        else
        {
            //如果是敌方
            if (gameCharacterData.characterType == 0)
            {
                //电脑开始行动
                miniGameData.SetCombatStatus(MiniGameCombatStatusEnum.EnemyRound);
                npcCpt.OpenAI();
            }
            //如果是友方
            else if (gameCharacterData.characterType == 1)
            {
                //友方行动
                miniGameData.SetCombatStatus(MiniGameCombatStatusEnum.OurRound);
                uiMiniGameCombat.OpenCombatCommand();
            }
            //开启选中特效
            SelectCharacter(npcCpt);
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

    #region 游戏UI回调
    /// <summary>
    /// 角色的回合
    /// </summary>
    /// <param name="gameCharacterData"></param>
    public void CharacterRound(MiniGameCharacterForCombatBean gameCharacterData)
    {
        StartCoroutine(RoundForPre(gameCharacterData));
        uiMiniGameCombat.RefreshUI();
    }

    /// <summary>
    /// 指令 结束
    /// </summary>
    /// <param name="details"></param>
    public void CommandEnd()
    {
        RoundForAction();
        uiMiniGameCombat.RefreshUI();
    }
    #endregion

    /// <summary>
    /// 协程 行动
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoroutineForAction()
    {
        //先取消选中框
        miniGameBuilder.DeleteSelectEffect();

        switch (miniGameData.GetRoundActionCommand())
        {
            case MiniGameCombatCommand.Fight:
                yield return StartCoroutine(CoroutineForCommandFight());
                break;
            case MiniGameCombatCommand.Skill:
                yield return StartCoroutine(CoroutineForCommandSkill());
                break;
            case MiniGameCombatCommand.Items:
                yield return StartCoroutine(CoroutineForCommandItems());
                break;
            case MiniGameCombatCommand.Pass:
                break;
        }
        //刷新UI（用于数值变化）
        uiMiniGameCombat.RefreshUI();
        CheckCharacterLife();
        yield return new WaitForSeconds(0.5f);
        StartNextRound();
    }

    /// <summary>
    /// 协程 战斗
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoroutineForCommandFight()
    {
        NpcAIMiniGameCombatCpt actionNpc = miniGameData.GetRoundActionCharacter();
        NpcAIMiniGameCombatCpt targetNpc = miniGameData.GetRoundTargetCharacter();
        //获取属性
        actionNpc.characterData.GetAttributes(gameItemsManager, actionNpc.characterMiniGameData, out CharacterAttributesBean actionCharacterAttributes);

        //让行动角色移动到被攻击对象面前
        Vector3 offsetPosition;
        //根据角色朝向决定位置
        if (targetNpc.GetCharacterFace() == 1)
            offsetPosition = new Vector3(-1, 0);
        else
            offsetPosition = new Vector3(1, 0);
        //记录之前的位置
        Vector3 oldPosition = actionNpc.transform.position;
        //行动角色移动到目标角色面前
        actionNpc.transform.DOMove(targetNpc.transform.position + offsetPosition, 0.5f);
        yield return new WaitForSeconds(0.5f);
        //TODO 计算闪避(待定)
        float dodgeRate = UnityEngine.Random.Range(0f, 1f);
        if (dodgeRate <= 1)
        {
            //力量测试加成
            float damagePowerRate = (miniGameData.GetRoundActionPowerTest() + 0.2f);
            //计算伤害
            int damage = (int)(damagePowerRate  * actionCharacterAttributes.force);
            //伤害减免
            damage = targetNpc.characterMiniGameData.GetTotalDef(gameItemsManager, damage);
            //角色伤害
            targetNpc.UnderAttack(damagePowerRate, damage);
            audioHandler.PlaySound(AudioSoundEnum.Fight);
        }
        else
        {
            //角色闪避了
            targetNpc.ShowTextInfo(GameCommonInfo.GetUITextById(14001),Color.blue);
        }
        //行动角色回到自己的位置
        yield return new WaitForSeconds(0.5f);
        actionNpc.transform.DOMove(oldPosition, 0.5f);
    }

    /// <summary>
    /// 协程 技能使用
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoroutineForCommandSkill()
    {
        NpcAIMiniGameCombatCpt actionNpc = miniGameData.GetRoundActionCharacter();
        List<NpcAIMiniGameCombatCpt> listTargetNpc = miniGameData.GetRoundTargetListCharacter();
        SkillInfoBean skillData = miniGameData.GetRoundActionSkill();
        Sprite spSkill= iconDataManager.GetIconSpriteByName(skillData.icon_key);
        //喊出技能名字
        actionNpc.SetShout(skillData.name);
        yield return new WaitForSeconds(1f);
        actionNpc.characterMiniGameData.AddUsedSkill(skillData.id, 1);
        //增加技能效果
        foreach (NpcAIMiniGameCombatCpt itemNpc in listTargetNpc)
        {
            itemNpc.AddCombatEffect(actionNpc, skillData.effect, skillData.effect_details, spSkill);
        }
        yield return new WaitForSeconds(1f);
    }

    /// <summary>
    /// 协程 物品使用
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoroutineForCommandItems()
    {
        NpcAIMiniGameCombatCpt actionNpc = miniGameData.GetRoundActionCharacter();
        List<NpcAIMiniGameCombatCpt> listTargetNpc = miniGameData.GetRoundTargetListCharacter();

        long itemsId = miniGameData.GetRoundActionItemsId();
        ItemsInfoBean itemsInfo = gameItemsManager.GetItemsById(itemsId);
        //从物品栏移除物品
        gameDataManager.gameData.AddItemsNumber(itemsInfo.id, -1);
        Sprite spItems = GeneralEnumTools.GetGeneralSprite(itemsInfo,iconDataManager,gameItemsManager,characterDressManager);
        //增加物品效果
        foreach (NpcAIMiniGameCombatCpt itemNpc in listTargetNpc)
        {
            itemNpc.AddCombatEffect(actionNpc,itemsInfo.effect, itemsInfo.effect_details, spItems);
        }
        yield return new WaitForSeconds(1f);
    }


    /// <summary>
    /// 检测所有角色生命值
    /// </summary>
    public void CheckCharacterLife()
    {
        List<NpcAIMiniGameCombatCpt> listNpc = miniGameBuilder.GetAllCharacter();
        foreach (NpcAIMiniGameCombatCpt itemNpc in listNpc)
        {
            //如果角色阵亡
            if (itemNpc.characterMiniGameData.characterCurrentLife <= 0)
            {
                //设置角色死亡
                itemNpc.SetCharacterDead();
                //移除这个角色
                if (miniGameBuilder.GetUserCharacter().Contains(itemNpc))
                    miniGameBuilder.GetUserCharacter().Remove(itemNpc);
                if (miniGameBuilder.GetEnemyCharacter().Contains(itemNpc))
                    miniGameBuilder.GetEnemyCharacter().Remove(itemNpc);
                //ui回合移除该角色
                uiMiniGameCombat.RemoveCharacterRound(itemNpc.characterMiniGameData);
     
            }
        }
        //检测是否游戏结束
        bool isGameOver = CheckIsGameOver(out MiniGameResultEnum gameResult);
        if (isGameOver)
            //结束游戏
            EndGame(gameResult);
    }

}