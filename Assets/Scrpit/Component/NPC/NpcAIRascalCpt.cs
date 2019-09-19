using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NpcAIRascalCpt : BaseNpcAI,ITextInfoView
{
    public enum RascalIntentEnum
    {
         Idle = 0,
         GoToInn = 1,//前往客栈
         WaitingForReply=2,//等待回复
         Leave=10,//离开
    }

    public RascalIntentEnum rascalIntent= RascalIntentEnum.Idle;
    //下一个移动点
    public Vector3 movePosition;
    //客栈处理
    public InnHandler innHandler;
    //客栈区域数据管理
    public SceneInnManager sceneInnManager;
    private TextInfoController mTextInfoController;

    //想要说的对话
    public List<TextInfoBean> listTextInfoBean;

    private void Start()
    {
        mTextInfoController = new TextInfoController(this,this);
    }

    /// <summary>
    /// 开始作恶
    /// </summary>
    public void StartEvil()
    {
        SetIntentForGoToInn();
    }

    private void Update()
    {
        switch (rascalIntent)
        {
            case RascalIntentEnum.GoToInn:
                //是否到达目的地
                if (characterMoveCpt.IsAutoMoveStop())
                {
                    //判断是否关门
                    if (innHandler.GetInnStatus() == InnHandler.InnStatusEnum.Close)
                    {
                        SetIntent(RascalIntentEnum.Leave);
                    }
                    else
                    {
                        SetIntent(RascalIntentEnum.WaitingForReply);
                    }
                }
                break;
            case RascalIntentEnum.Leave:
                //到底目的地删除对象
                if (characterMoveCpt.IsAutoMoveStop())
                    Destroy(gameObject);
                break;
        }
    }

    /// <summary>
    /// 设置意图
    /// </summary>
    /// <param name="intentEnum"></param>
    public void SetIntent(RascalIntentEnum intentEnum)
    {
        this.rascalIntent = intentEnum;
        switch (intentEnum)
        {
            case RascalIntentEnum.GoToInn:
                SetIntentForGoToInn();
                break;
            case RascalIntentEnum.WaitingForReply:
                SetIntentForWaitingForReply();
                break;
            case RascalIntentEnum.Leave:
                SetIntentForLeave();
                break;
        }
    }
    
    /// <summary>
    /// 意图-等待恢复
    /// </summary>
    public void SetIntentForWaitingForReply()
    {
        //获取文本信息
        mTextInfoController.GetTextForTalk(characterFavorabilityData.characterId, characterFavorabilityData.favorability);
    }

    /// <summary>
    /// 意图-前往客栈
    /// </summary>
    public void SetIntentForGoToInn()
    {
        //移动到门口附近
        movePosition = innHandler.GetRandomEntrancePosition();
        if (movePosition == null)
            movePosition = Vector3.zero;
        //前往门
        characterMoveCpt.SetDestination(movePosition);
    }

    /// <summary>
    /// 意图-离开
    /// </summary>
    public void SetIntentForLeave()
    {
        //随机获取一个退出点
        movePosition = sceneInnManager.GetRandomSceneExportPosition();
        characterMoveCpt.SetDestination(movePosition);
    }

    #region 对话信息回调
    public void GetTextInfoForLookSuccess(List<TextInfoBean> listData)
    {
        this.listTextInfoBean = listData;
        if (CheckUtil.ListIsNull(listTextInfoBean))
        {
            SetIntent(RascalIntentEnum.Leave);
            return;
        }
        TextInfoBean textInfo = RandomUtil.GetRandomDataByList(listTextInfoBean);
        EventHandler.Instance.EventTriggerForTalk(textInfo.mark_id);
    }

    public void GetTextInfoForTalkSuccess(List<TextInfoBean> listData)
    {

    }

    public void GetTextInfoForStorySuccess(List<TextInfoBean> listData)
    {

    }

    public void GetTextInfoFail()
    {

    }
    #endregion
}