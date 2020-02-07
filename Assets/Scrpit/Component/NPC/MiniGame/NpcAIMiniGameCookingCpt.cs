using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class NpcAIMiniGameCookingCpt : BaseNpcAI
{
    public enum MiniGameCookingIntentEnum
    {
        Idle,
        GoToAuditTable,
        GoToStove,
        AutoCooking,
        CookingPre,
        CookingMaking,
        CookingEnd,
        GoToAudit,//前往评审
        EatFood,
        AuditFood,//评审食物
    }

    public MiniGameCookingIntentEnum miniGameCookingIntent = MiniGameCookingIntentEnum.Idle;

    public enum MiniGameCookingNpcTypeEnum
    {
        Player,//参与者
        Auditer,//评审员
        Compere//主持
    }

    private MiniGameCookingNpcTypeEnum mNpcType;
    public SpriteRenderer srScore;
    public TextMesh tvScore;

    //开始的位置
    public Vector3 startPosition;

    //游戏处理
    protected MiniGameCookingHandler miniGameCookingHandler;
    //该NPC的数据
    public MiniGameCharacterForCookingBean characterMiniGameData;
    //该NPC的评审桌
    public MiniGameCookingAuditTableCpt auditTableCpt;
    //该NPC的灶台
    public MiniGameCookingStoveCpt stoveCpt;
    //拥有的食物 评审 和 参与者
    public FoodForCoverCpt foodForCover;
    //拿食物位置
    public GameObject objFoodPosition;
    //被评审的对象
    public NpcAIMiniGameCookingCpt auditTargetNpc;

    public override void Awake()
    {
        base.Awake();
        miniGameCookingHandler = Find<MiniGameCookingHandler>(ImportantTypeEnum.MiniGameHandler);
    }

    private void Update()
    {
        switch (miniGameCookingIntent)
        {
            case MiniGameCookingIntentEnum.GoToStove:
                if (characterMiniGameData != null && characterMoveCpt.IsAutoMoveStop())
                {
                    if (characterMiniGameData.characterType == 1)
                    {
                        //如果是玩家到达灶台 则开始选择制作的食物
                        SetIntent(MiniGameCookingIntentEnum.Idle);
                        miniGameCookingHandler.StartSelectMenu();
                    }
                    else
                    {
                        SetIntent(MiniGameCookingIntentEnum.AutoCooking);
                    }
                    //打开灶台
                    if (stoveCpt != null)
                        stoveCpt.OpenStove();
                }
                break;
            case MiniGameCookingIntentEnum.GoToAudit:
                if (characterMiniGameData.characterType == 1 && characterMoveCpt.IsAutoMoveStop())
                {
                    SetIntent(MiniGameCookingIntentEnum.Idle);
                    miniGameCookingHandler.StartStoryForGameAudit();
                }
                break;
        }
    }

    public void SetNpcType(MiniGameCookingNpcTypeEnum npcType)
    {
        mNpcType = npcType;
    }

    public MiniGameCookingNpcTypeEnum GetNpcType()
    {
        return mNpcType;
    }

    /// <summary>
    /// 设置NPC数据
    /// </summary>
    /// <param name="characterMiniGameData"></param>
    public void SetData(MiniGameCharacterForCookingBean characterMiniGameData)
    {
        this.characterMiniGameData = characterMiniGameData;
        SetCharacterData(characterMiniGameData.characterData);
    }

    /// <summary>
    /// 设置评审桌子
    /// </summary>
    /// <param name="auditTableCpt"></param>
    public void SetAuditTable(MiniGameCookingAuditTableCpt auditTableCpt)
    {
        this.auditTableCpt = auditTableCpt;
    }

    /// <summary>
    /// 设置料理灶台
    /// </summary>
    /// <param name="stoveCpt"></param>
    public void SetStove(MiniGameCookingStoveCpt stoveCpt)
    {
        this.stoveCpt = stoveCpt;
        this.stoveCpt.SetMenuInfo(characterMiniGameData.cookingMenuInfo);
    }

    public void OpenAI()
    {
        characterMoveCpt.navMeshAgent.enabled = true;
    }

    /// <summary>
    /// 设置意图
    /// </summary>
    /// <param name="intent"></param>
    /// <param name="auditType"> 评审类型 1题2色3香4味</param>
    public void SetIntent(MiniGameCookingIntentEnum intent, int auditType)
    {
        this.miniGameCookingIntent = intent;
        switch (miniGameCookingIntent)
        {
            case MiniGameCookingIntentEnum.Idle:
                break;
            case MiniGameCookingIntentEnum.GoToAuditTable:
                SetIntentForGoToAuditTable();
                break;
            case MiniGameCookingIntentEnum.GoToStove:
                SetIntentForGoToStove();
                break;
            case MiniGameCookingIntentEnum.AutoCooking:
                SetIntentForAutoCooking();
                break;
            case MiniGameCookingIntentEnum.CookingPre:
                SetIntentForCookingPre();
                break;
            case MiniGameCookingIntentEnum.CookingMaking:
                SetIntentForCookingMaking();
                break;
            case MiniGameCookingIntentEnum.CookingEnd:
                SetIntentForCookingEnd();
                break;
            case MiniGameCookingIntentEnum.GoToAudit:
                SetIntentForGoToAudit();
                break;
            case MiniGameCookingIntentEnum.EatFood:
                SetIntentForEatFood();
                break;
            case MiniGameCookingIntentEnum.AuditFood:
                SetIntentForAuditFood(auditType);
                break;
        }
    }
    public void SetIntent(MiniGameCookingIntentEnum intent)
    {
        SetIntent(intent, 0);
    }

    /// <summary>
    /// 意图-前往评审桌子
    /// </summary>
    public void SetIntentForGoToAuditTable()
    {
        if (auditTableCpt != null)
        {
            Vector3 seatPosition = auditTableCpt.GetSeatPosition();
            characterMoveCpt.SetDestination(seatPosition);
        }
    }

    /// <summary>
    /// 意图-前往灶台
    /// </summary>
    public void SetIntentForGoToStove()
    {
        if (stoveCpt != null)
        {
            Vector3 makingPosition = stoveCpt.GetCookingMakingPosition();
            characterMoveCpt.SetDestination(makingPosition);
        }
    }

    /// <summary>
    /// 意图-做菜
    /// </summary>
    public void SetIntentForAutoCooking()
    {
        StartCoroutine(CoroutineForAutoCooking());
    }

    /// <summary>
    /// 意图-做菜 备料
    /// </summary>
    public void SetIntentForCookingPre()
    {
        stoveCpt.SetMenuInfo(characterMiniGameData.cookingMenuInfo);
        characterMoveCpt.SetDestination(stoveCpt.GetCookingPrePosition());
        StartCoroutine(CoroutineForCookingPre());
    }

    /// <summary>
    /// 意图-做菜 烹饪
    /// </summary>
    public void SetIntentForCookingMaking()
    {
        characterMoveCpt.SetDestination(stoveCpt.GetCookingMakingPosition());
    }

    /// <summary>
    /// 意图-做菜 摆盘
    /// </summary>
    public void SetIntentForCookingEnd()
    {
        characterMoveCpt.SetDestination(stoveCpt.GetCookingEndPosition());
        foodForCover = stoveCpt.CreateFood();
    }

    /// <summary>
    /// 意图-前往评审
    /// </summary>
    public void SetIntentForGoToAudit()
    {
        //如果是对手。先创建一个食物
        if (characterMiniGameData.characterType == 0)
        {
            foodForCover = stoveCpt.CreateFood();
        }
        //将食物拿在手上
        foodForCover.transform.SetParent(objFoodPosition.transform);
        foodForCover.transform.position = objFoodPosition.transform.position;
        characterMoveCpt.SetDestination(startPosition);
    }

    /// <summary>
    /// 意图-吃食物
    /// </summary>
    public void SetIntentForEatFood()
    {
        StartCoroutine(CoroutineForEatFood());
    }

    /// <summary>
    /// 意图-评审食物
    /// </summary>
    public void SetIntentForAuditFood(int type)
    {
        int score = 0;
        switch (type)
        {
            case 1:
                score = AuditFoodForTheme();
                auditTargetNpc.characterMiniGameData.listScoreForColor.Add(score);
                break;
            case 2:
                score = AuditFoodForColor();
                auditTargetNpc.characterMiniGameData.listScoreForColor.Add(score);
                break;
            case 3:
                score = AuditFoodForSweet();
                auditTargetNpc.characterMiniGameData.listScoreForSweet.Add(score);
                break;
            case 4:
                score = AuditFoodForTaste();
                auditTargetNpc.characterMiniGameData.listScoreForTaste.Add(score);
                break;
        }
        ShowScore(score);
    }

    /// <summary>
    /// 评审主题
    /// </summary>
    /// <returns></returns>
    private int AuditFoodForTheme()
    {
        CookingThemeBean cookingTheme = miniGameCookingHandler.miniGameData.cookingTheme;
        MenuInfoBean menuInfo = auditTargetNpc.characterMiniGameData.cookingMenuInfo;
        float similarity = cookingTheme.GetSimilarity(menuInfo);
        return ScoreDeal((int)(similarity * 100));
    }

    /// <summary>
    /// 评审 色
    /// </summary>
    /// <returns></returns>
    private int AuditFoodForColor()
    {
        int score = auditTargetNpc.characterMiniGameData.settleDataForPre.GetScore();
        return ScoreDeal(score);
    }

    /// <summary>
    /// 评审 香
    /// </summary>
    /// <returns></returns>
    private int AuditFoodForSweet()
    {
        int score = auditTargetNpc.characterMiniGameData.settleDataForPre.GetScore();
        return ScoreDeal(score);
    }

    /// <summary>
    /// 评审 味
    /// </summary>
    /// <returns></returns>
    private int AuditFoodForTaste()
    {
        int score = auditTargetNpc.characterMiniGameData.settleDataForMaking.GetScore();
        return ScoreDeal(score);
    }

    /// <summary>
    /// 分数处理
    /// </summary>
    /// <param name="score"></param>
    /// <returns></returns>
    private int ScoreDeal(int score)
    {
        score += Random.Range(-10, 11);
        if (score < 0)
            score = 0;
        if (score > 100)
            score = 100;
        return score;
    }

    /// <summary>
    /// 展示分数
    /// </summary>
    /// <param name="score"></param>
    public void ShowScore(int score)
    {
        srScore.gameObject.SetActive(true);
        tvScore.text = score + "";
        srScore.transform.localScale = new Vector3(1,1,1);
        srScore.transform.DOScale(Vector3.zero, 0.5f).From().SetEase(Ease.OutBack);
    }

    /// <summary>
    /// 关闭分数
    /// </summary>
    public void CloseScore()
    {
        srScore.transform.DOScale(Vector3.zero, 0.5f).OnComplete(delegate() {
            srScore.gameObject.SetActive(false);
        });
    }

    /// <summary>
    /// 协成-开始做菜
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoroutineForAutoCooking()
    {
        while (miniGameCookingIntent == MiniGameCookingIntentEnum.AutoCooking)
        {
            int randomDo = Random.Range(0, 3);
            float randomDoTime = Random.Range(3f, 7f);
            switch (randomDo)
            {
                case 0:
                    characterMoveCpt.SetDestination(stoveCpt.GetCookingPrePosition());
                    stoveCpt.ChangeIngredientPre();
                    break;
                case 1:
                    characterMoveCpt.SetDestination(stoveCpt.GetCookingMakingPosition());
                    break;
                case 2:
                    characterMoveCpt.SetDestination(stoveCpt.GetCookingEndPosition());
                    break;
            }
            yield return new WaitForSeconds(randomDoTime);
        }
    }

    /// <summary>
    /// 协程 料理准备
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoroutineForCookingPre()
    {
        while (miniGameCookingIntent == MiniGameCookingIntentEnum.CookingPre)
        {
            stoveCpt.ChangeIngredientPre();
            yield return new WaitForSeconds(3);
        }
        stoveCpt.ClearStoveIngredient();
    }

    /// <summary>
    /// 协程 吃食物
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoroutineForEatFood()
    {
        yield return new WaitForSeconds(1);
        if (foodForCover != null)
            foodForCover.FinshFood();
    }
}