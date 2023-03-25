using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;

public class NpcAIRascalCpt : BaseNpcAI
{
    public enum RascalIntentEnum
    {
        Idle = 0,
        GoToInn = 1,//前往客栈
        WaitingForReply = 2,//等待回复
        MakeTrouble = 3,//闹事
        Fighting = 4,//打架中
        ContinueMakeTrouble = 5,//继续闹事
        Leave = 10,//离开
    }

    public RascalIntentEnum rascalIntent = RascalIntentEnum.Idle;

    //检测范围展示
    public GameObject objRascalSpaceShow;
    //打架特效
    public GameObject objFightShow;
    //生命条
    public CharacterLifeCpt characterLifeCpt;

    //离开点
    public Vector3 leavePosition;
    //下一个移动点
    public Vector3 movePosition;
    //战斗对象
    public BaseNpcAI npcFight;

    //想要说的对话
    public List<TextInfoBean> listShoutTextInfo = new List<TextInfoBean>();

    //角色生命值
    public int characterMaxLife = 50;
    public int characterLife = 50;

    //制造麻烦的时间
    public float timeMakeTrouble = 60;
    //增加的好感
    public int addFavorability = 0;
    //小队唯一代码
    public string teamCode;
    //小队数据
    public NpcTeamBean teamData;
    //小队中的位置
    public int teamRank;

    //是否移动  用于追击判定
    protected bool isMove = false;


    private void Update()
    {
        switch (rascalIntent)
        {
            case RascalIntentEnum.GoToInn:
                HandleGoToInn();
                break;
            case RascalIntentEnum.Leave:
                HandlerLeave();
                break;
            case RascalIntentEnum.Fighting:
                HandlerFighting();
                break;
        }

    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        GameEventHandler.Instance.UnRegisterNotifyForEvent(NotifyForEvent);
    }


    /// <summary>
    /// 设置移动状态
    /// </summary>
    /// <param name="isMove"></param>
    public void SetMoveStatus(bool isMove)
    {
        this.isMove = isMove;
        if (!isMove)
        {
            characterMoveCpt.StopAutoMove();
        }
    }

    /// <summary>
    /// 设置意图
    /// </summary>
    /// <param name="intentEnum"></param>
    public void SetIntent(RascalIntentEnum intentEnum)
    {
        if (this)
            StopAllCoroutines();
        this.rascalIntent = intentEnum;
        SetMoveStatus(true);
        switch (intentEnum)
        {
            case RascalIntentEnum.GoToInn:
                SetIntentForGoToInn();
                break;
            case RascalIntentEnum.WaitingForReply:
                SetIntentForWaitingForReply();
                break;
            case RascalIntentEnum.MakeTrouble:
                SetIntentForMakeTrouble();
                break;
            case RascalIntentEnum.Fighting:
                SetIntentForFighting();
                break;
            case RascalIntentEnum.ContinueMakeTrouble:
                SetIntentForContinueMakeTrouble();
                break;
            case RascalIntentEnum.Leave:
                SetIntentForLeave();
                break;
        }
    }

    /// <summary>
    /// 处理-前往客栈
    /// </summary>
    protected void HandleGoToInn()
    {
        //是否到达目的地
        if (CheckCharacterIsArrive())
        {
            //判断是否关门
            if (InnHandler.Instance.GetInnStatus() == InnHandler.InnStatusEnum.Close)
            {
                SetIntent(RascalIntentEnum.Leave);
            }
            else
            {
                SetIntent(RascalIntentEnum.WaitingForReply);
            }
        }
    }

    /// <summary>
    /// 处理-离开客栈
    /// </summary>
    protected void HandlerLeave()
    {
        //到底目的地删除对象
        if (characterMoveCpt.IsAutoMoveStop())
            Destroy(gameObject);
    }

    protected float audioTime = 0;
    protected void HandlerFighting()
    {
        audioTime -= Time.deltaTime;
        if (audioTime <= 0)
        {
            AudioHandler.Instance.PlaySound(AudioSoundEnum.Fight);
            audioTime = 1;
        }
    }

    /// <summary>
    /// 意图-前往客栈
    /// </summary>
    protected void SetIntentForGoToInn()
    {
        //移动到门口附近
        movePosition = InnHandler.Instance.GetRandomEntrancePosition();
        if (movePosition == null)
            movePosition = Vector3.zero;
        //前往门
        characterMoveCpt.SetDestination(movePosition);
    }

    /// <summary>
    /// 意图-等待回复
    /// </summary>
    protected void SetIntentForWaitingForReply()
    {
        if (teamRank == 0)
        {
            long talk_ids = RandomUtil.GetRandomDataByArray(teamData.GetTalkIds());
            if (talk_ids == 0)
            {
                SetTeamIntent(RascalIntentEnum.Leave);
                return;
            }
            bool isTrigger = GameEventHandler.Instance.EventTriggerForTalkByRascal(this, talk_ids);
            if (isTrigger)
            {
                GameEventHandler.Instance.RegisterNotifyForEvent(NotifyForEvent);

            }
            else
            {
                //如果没有触发事件 则全体离开
                SetTeamIntent(RascalIntentEnum.Leave);
            }
        }
    }

    /// <summary>
    /// 意图-制造麻烦
    /// </summary>
    protected void SetIntentForMakeTrouble()
    {
        long[] shoutIds = teamData.GetShoutIds();
        TextInfoHandler.Instance.manager.GetTextForTalkByMarkId(shoutIds[0], SetTextInfoData);
        //展示生命条
        characterLifeCpt.gameObject.SetActive(true);
        characterLifeCpt.gameObject.transform.localScale = new Vector3(1, 1, 1);
        characterLifeCpt.gameObject.transform.DOScale(new Vector3(0.2f, 0.2f), 0.5f).From().SetEase(Ease.OutBack);
        //闹事人员添加
        InnHandler.Instance.rascalrQueue.Add(this);

        StartCoroutine(CoroutineForStartMakeTrouble());
        //延迟显示范围
        OpenMakeTroubleRange();
    }

    /// <summary>
    /// 意图-打架
    /// </summary>
    protected void SetIntentForFighting()
    {
        StopAllCoroutines();
        characterMoveCpt.StopAutoMove();
        objFightShow.SetActive(true);
    }

    /// <summary>
    /// 意图-继续闹事
    /// </summary>
    protected void SetIntentForContinueMakeTrouble()
    {
        npcFight = null;
        objFightShow.SetActive(false);
        StartCoroutine(CoroutineForStartMakeTrouble());
    }

    /// <summary>
    /// 意图-离开
    /// </summary>
    protected void SetIntentForLeave()
    {
        InnHandler.Instance.rascalrQueue.Remove(this);
        npcFight = null;
        characterLifeCpt.gameObject.SetActive(false);
        objFightShow.SetActive(false);
        objRascalSpaceShow.SetActive(false);
        //随机获取一个退出点 如果已经有团队的点则不需要获取
        if (leavePosition == Vector3.zero)
        {
            SceneInnManager sceneInnManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneInnManager>();
            leavePosition = sceneInnManager.GetRandomSceneExportPosition();
        }
        movePosition = leavePosition + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
        characterMoveCpt.SetDestination(movePosition);
    }

    /// <summary>
    /// 设置小队数据
    /// </summary>
    /// <param name="teamData"></param>
    /// <param name="teamRank">该人物在小队中的位置</param>
    public void SetTeamData(string teamCode, NpcTeamBean teamData, int teamRank)
    {
        this.teamCode = teamCode;
        this.teamData = teamData;
        this.teamRank = teamRank;
    }

    /// <summary>
    /// 修改生命值
    /// </summary>
    /// <param name="life"></param>
    public int AddLife(int life)
    {
        characterLife += life;
        if (characterLife <= 0)
        {
            characterLife = 0;
            SetIntent(RascalIntentEnum.Leave);

            long[] shoutIds = teamData.GetShoutIds();
            TextInfoHandler.Instance.manager.GetTextForTalkByMarkId(shoutIds[1], SetTextInfoData);

            //随机获取一句喊话
            if (!listShoutTextInfo.IsNull())
            {
                TextInfoBean textInfo = RandomUtil.GetRandomDataByList(listShoutTextInfo);
                characterShoutCpt.Shout(textInfo.content);
            }
            //快速离开
            characterMoveCpt.SetMoveSpeed(5);
        }
        else if (characterLife > characterMaxLife)
        {
            characterLife = characterMaxLife;
        }
        characterLifeCpt.SetData(characterLife, characterMaxLife);
        return characterLife;
    }

    /// <summary>
    /// 检测
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (objRascalSpaceShow.activeSelf)
        {
            NpcAICustomerCpt customerCpt = collision.GetComponent<NpcAICustomerCpt>();
            if (customerCpt)
            {
                if (customerCpt.customerIntent != NpcAICustomerCpt.CustomerIntentEnum.Leave
                    && customerCpt.customerIntent != NpcAICustomerCpt.CustomerIntentEnum.Walk
                    && customerCpt.customerIntent != NpcAICustomerCpt.CustomerIntentEnum.Want
                    && customerCpt.customerIntent != NpcAICustomerCpt.CustomerIntentEnum.WaitAccost
                    && customerCpt.customerIntent != NpcAICustomerCpt.CustomerIntentEnum.TalkWithAccost)
                    AudioHandler.Instance.PlaySound(AudioSoundEnum.Passive);
                customerCpt.ChangeMood(-99999);
            }
        }
    }

    /// <summary>
    /// 开始制造麻烦
    /// </summary>
    /// <returns></returns>
    public IEnumerator CoroutineForStartMakeTrouble()
    {
        while (rascalIntent == RascalIntentEnum.MakeTrouble || rascalIntent == RascalIntentEnum.ContinueMakeTrouble)
        {
            if (isMove)
            {
                movePosition = InnHandler.Instance.GetRandomInnPositon();
                bool canGo = CheckUtil.CheckPath(transform.position, movePosition);
                if (canGo)
                    characterMoveCpt.SetDestination(movePosition);
            }
            //随机获取一句喊话
            //int shoutId = Random.Range(13101, 13106);
            if (!listShoutTextInfo.IsNull())
            {
                TextInfoBean textInfo = RandomUtil.GetRandomDataByList(listShoutTextInfo);
                characterShoutCpt.Shout(textInfo.content);
            }
            yield return new WaitForSeconds(5);
            //时间到了就离开
            timeMakeTrouble -= 5;
            if (timeMakeTrouble <= 0)
            {
                SetIntent(RascalIntentEnum.Leave);
            }
        }
    }

    /// <summary>
    /// 协程-延迟生成判定
    /// </summary>
    /// <returns></returns>
    public void OpenMakeTroubleRange()
    {
        //展示范围
        objRascalSpaceShow.SetActive(true);
        objRascalSpaceShow.transform.localScale = new Vector3(2, 2, 2);
        objRascalSpaceShow.transform.DOScale(new Vector3(0.2f, 0.2f), 0.5f).From().SetEase(Ease.OutBack);
    }

    /// <summary>
    /// 设置全体意图
    /// </summary>
    public void SetTeamIntent(RascalIntentEnum rascalIntent)
    {
        List<NpcAIRascalCpt> listNpc = NpcHandler.Instance.builderForEvent.GetRascalTeamByTeamCode(teamCode);
        if (rascalIntent == RascalIntentEnum.Leave)
        {
            SceneInnManager sceneInnManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneInnManager>();
            leavePosition = sceneInnManager.GetRandomSceneExportPosition();
        }
        foreach (NpcAIRascalCpt itemNpc in listNpc)
        {
            itemNpc.leavePosition = leavePosition;
            itemNpc.SetIntent(rascalIntent);
        }
    }

    /// <summary>
    /// 设置文本数据
    /// </summary>
    /// <param name="listData"></param>
    public void SetTextInfoData(List<TextInfoBean> listData)
    {
        listShoutTextInfo = listData;
    }

    #region 事件通知

    public void NotifyForEvent(GameEventHandler.NotifyEventTypeEnum notifyEventType,params object[] obj)
    {
        if (notifyEventType == GameEventHandler.NotifyEventTypeEnum.TalkForAddFavorability)
        {
            addFavorability += (int)obj[1];
        }
        else if (notifyEventType == GameEventHandler.NotifyEventTypeEnum.EventEnd)
        {
            //根据对话的好感加成 不同的反应
            if (addFavorability > 0)
            {
                SetTeamIntent(RascalIntentEnum.Leave);
            }
            else if (addFavorability < 0)
            {
                SetTeamIntent(RascalIntentEnum.MakeTrouble);
            }
        }
    }
    #endregion



}