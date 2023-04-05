using UnityEngine;
using DG.Tweening;
using UnityEditor;
using System.Collections.Generic;
using System;

public class BaseMiniGameHandler<B, D> : BaseHandler<BaseMiniGameHandler<B, D>,BaseManager>, UIMiniGameCountDown.ICallBack, UIMiniGameEnd.ICallBack
    where D : MiniGameBaseBean
    where B : BaseMiniGameBuilder
{

    Action<MiniGameStatusEnum, object[]> notifyForMiniGameStatus;

    //数据
    //游戏构建器
    public B miniGameBuilder;
    //游戏数据
    public D miniGameData;

    public string builderName = "";

    //迷你游戏状态
    protected MiniGameStatusEnum miniGameStatus = MiniGameStatusEnum.GamePre;

    public override void Awake()
    {
        GameObject objModel = null;
        try
        {
            objModel = LoadAddressablesUtil.LoadAssetSync<GameObject>($"Assets/Prefabs/Builder/{builderName}.prefab");
        }
        catch
        {
            LogUtil.Log($"没有找到{builderName}的迷你游戏构建 可能会引发错误");
        }
        if (objModel == null)
            return;
        GameObject objItem =  Instantiate(gameObject, objModel);
        miniGameBuilder = objItem.GetComponent<B>();
    }

    /// <summary>
    /// 注册时间通知
    /// </summary>
    /// <param name="notifyForTime"></param>
    public void RegisterNotifyForMiniGameStatus(Action<MiniGameStatusEnum, object[]> notifyForMiniGameStatus)
    {
        this.notifyForMiniGameStatus += notifyForMiniGameStatus;
    }
    public void UnRegisterNotifyForMiniGameStatus(Action<MiniGameStatusEnum, object[]> notifyForMiniGameStatus)
    {
        this.notifyForMiniGameStatus -= notifyForMiniGameStatus;
    }

    /// <summary>
    /// 设置游戏状态
    /// </summary>
    /// <param name="status"></param>
    public void SetMiniGameStatus(MiniGameStatusEnum status)
    {
        miniGameStatus = status;
    }

    /// <summary>
    /// 设置摄像头位置
    /// </summary>
    /// <param name="cameraPosition"></param>
    public virtual void SetCameraPosition(Vector3 cameraPosition)
    {
        //设置摄像机位置
        GameControlHandler.Instance.manager.GetControl().SetFollowPosition(cameraPosition);
    }

    /// <summary>
    /// 获取游戏状态
    /// </summary>
    /// <returns></returns>
    public MiniGameStatusEnum GetMiniGameStatus()
    {
        return miniGameStatus;
    }

    /// <summary>
    /// 获取游戏地点
    /// </summary>
    /// <returns></returns>
    public Vector3 GetMiniGamePosition()
    {
        return miniGameData.miniGamePosition;
    }

    /// <summary>
    /// 初始化游戏
    /// </summary>
    public virtual void InitGame(D miniGameData)
    {
        this.miniGameData = miniGameData;
        GameTimeHandler.Instance.SetTimeStop();
        SetMiniGameStatus(MiniGameStatusEnum.GamePre);
        notifyForMiniGameStatus?.Invoke(MiniGameStatusEnum.GamePre, new object[] { miniGameData });
    }

    /// <summary>
    /// 游戏开始
    /// </summary>
    public virtual void StartGame()
    {
        AudioHandler.Instance.PlayMusicForLoop(AudioMusicEnum.Battle);

        SetMiniGameStatus(MiniGameStatusEnum.Gameing);
        //通知 游戏开始
        notifyForMiniGameStatus?.Invoke(MiniGameStatusEnum.Gameing, new object[] { miniGameData });
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    /// <param name="isWinGame"></param>
    /// <param name="isSlow">是否开启慢镜头</param>
    public virtual void EndGame(MiniGameResultEnum gameResulte, bool isSlow)
    {
        if (GetMiniGameStatus() == MiniGameStatusEnum.Gameing)
        {
            SetMiniGameStatus(MiniGameStatusEnum.GameEnd);
            StopAllCoroutines();
            //拉近尽头
            BaseControl baseControl = GameControlHandler.Instance.manager.GetControl();
            if(baseControl)
                baseControl.SetCameraOrthographicSize(6);
            if (isSlow)
            {
                //开启慢镜头
                Time.timeScale = 0.1f;
            }
            transform.DOScale(new Vector3(1, 1, 1), 0.3f).OnComplete(delegate ()
            {
                Time.timeScale = 1f; 
                if (baseControl)
                    baseControl.SetCameraOrthographicSize();
                if(miniGameBuilder)
                    miniGameBuilder.DestroyAll();
                //设置游戏数据
                miniGameData.SetGameResult(gameResulte);
                GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
                //经验加成
                List<MiniGameCharacterBean> listUserData = miniGameData.GetListUserGameData();
                List<CharacterBean> listWorkerData = gameData.GetAllCharacterData();
                foreach (MiniGameCharacterBean itemCharacterData in listUserData)
                {
                    foreach (CharacterBean itemWorkerData in listWorkerData)
                    {
                        if (itemWorkerData.baseInfo.characterId == null || itemCharacterData.characterData.baseInfo.characterId == null)
                        {
                            continue;
                        }
                        if (itemWorkerData.baseInfo.characterId.Equals(itemCharacterData.characterData.baseInfo.characterId))
                        {
                            WorkerEnum workerType = MiniGameEnumTools.GetWorkerTypeByMiniGameType(miniGameData.gameType);
                            CharacterWorkerBaseBean characterWorker = itemWorkerData.baseInfo.GetWorkerInfoByType(workerType);
                            if (miniGameData.GetGameResult() == MiniGameResultEnum.Win)
                            {
                                characterWorker.AddExp(10, out bool isLevelUp);
                            }
                            else
                            {
                                characterWorker.AddExp(5, out bool isLevelUp);
                            }
                        }
                    }

                }
                //打开游戏结束UI
                UIMiniGameEnd uiMiniGameEnd = UIHandler.Instance.OpenUIAndCloseOther<UIMiniGameEnd>();
                uiMiniGameEnd.SetData(miniGameData);
                uiMiniGameEnd.SetCallBack(this);
            });
            //通知 游戏结束
            notifyForMiniGameStatus?.Invoke(MiniGameStatusEnum.GameEnd, new object[] { miniGameData });
            AudioHandler.Instance.StopMusic();
        }
    }
    public virtual void EndGame(MiniGameResultEnum gameResult)
    {
        EndGame(gameResult, true);
    }

    /// <summary>
    /// 打开倒计时UI
    /// </summary>
    /// <param name="miniGameData"></param>
    public void OpenCountDownUI(MiniGameBaseBean miniGameData, bool isCountDown)
    {
        //打开游戏准备倒计时UI
        UIMiniGameCountDown uiCountDown = UIHandler.Instance.OpenUIAndCloseOther<UIMiniGameCountDown>();
        uiCountDown.SetCallBack(this);
        //设置胜利条件
        List<string> listWinConditions = miniGameData.GetListWinConditions();
        string targetTitleStr = miniGameData.GetGameName();
        //设置准备UI的数据
        uiCountDown.SetData(targetTitleStr, listWinConditions, isCountDown);
    }

    public void OpenCountDownUI(MiniGameBaseBean miniGameData)
    {
        OpenCountDownUI(miniGameData, true);
    }

    #region 倒计时UI回调
    public virtual void GamePreCountDownStart()
    {

    }

    public virtual void GamePreCountDownEnd()
    {

    }
    #endregion

    #region 游戏结束按钮回调
    public void OnClickClose()
    {
        if(miniGameBuilder!=null)
            miniGameBuilder.DestroyAll();
        GameTimeHandler.Instance.SetTimeRestore();
        //通知 关闭游戏      
        notifyForMiniGameStatus?.Invoke(MiniGameStatusEnum.GameClose, new object[] { miniGameData });
        notifyForMiniGameStatus = null;
    }
    #endregion
}