using UnityEngine;
using DG.Tweening;
using UnityEditor;
using System.Collections.Generic;

public class BaseMiniGameHandler<B, D> : BaseHandler, UIMiniGameCountDown.ICallBack, UIMiniGameEnd.ICallBack
    where D : MiniGameBaseBean
    where B : BaseMiniGameBuilder
{

    //UI管理
    protected UIGameManager uiGameManager;
    protected GameTimeHandler gameTimeHandler;
    //数据
    protected GameDataManager gameDataManager;

    protected IconDataManager iconDataManager;
    //游戏构建器
    public B miniGameBuilder;
    //游戏数据
    public D miniGameData;

    //迷你游戏状态
    private MiniGameStatusEnum mMiniGameStatus = MiniGameStatusEnum.GamePre;

    protected virtual void Awake()
    {
        gameDataManager = Find<GameDataManager>(ImportantTypeEnum.GameDataManager);
        uiGameManager = Find<UIGameManager>(ImportantTypeEnum.GameUI);
        gameTimeHandler = Find<GameTimeHandler>(ImportantTypeEnum.TimeHandler);
        miniGameBuilder = FindInChildren<B>(ImportantTypeEnum.MiniGameBuilder);
        iconDataManager = Find<IconDataManager>(ImportantTypeEnum.UIManager);
    }

    /// <summary>
    /// 设置游戏状态
    /// </summary>
    /// <param name="status"></param>
    public void SetMiniGameStatus(MiniGameStatusEnum status)
    {
        mMiniGameStatus = status;
    }

    /// <summary>
    /// 设置摄像头位置
    /// </summary>
    /// <param name="cameraPosition"></param>
    public virtual void SetCameraPosition(Vector3 cameraPosition)
    {
        //设置摄像机位置
        GameControlHandler.Instance.GetControl().SetFollowPosition(cameraPosition);
    }

    /// <summary>
    /// 获取游戏状态
    /// </summary>
    /// <returns></returns>
    public MiniGameStatusEnum GetMiniGameStatus()
    {
        return mMiniGameStatus;
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
        if (gameTimeHandler != null)
            GameTimeHandler.Instance.SetTimeStop();
        SetMiniGameStatus(MiniGameStatusEnum.GamePre);
    }

    /// <summary>
    /// 游戏开始
    /// </summary>
    public virtual void StartGame()
    {
        AudioHandler.Instance.PlayMusicForLoop(AudioMusicEnum.Battle);

        SetMiniGameStatus(MiniGameStatusEnum.Gameing);
        //通知 游戏开始
        NotifyAllObserver((int)MiniGameStatusEnum.Gameing, miniGameData);
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
            BaseControl baseControl = GameControlHandler.Instance.GetControl();
            baseControl.SetCameraOrthographicSize(6);
            if (isSlow)
            {
                //开启慢镜头
                Time.timeScale = 0.1f;
            }
            transform.DOScale(new Vector3(1, 1, 1), 0.3f).OnComplete(delegate ()
            {
                Time.timeScale = 1f;
                baseControl.SetCameraOrthographicSize();

                miniGameBuilder.DestroyAll();
                //设置游戏数据
                miniGameData.SetGameResult(gameResulte);
                //经验加成
                List<MiniGameCharacterBean> listUserData = miniGameData.GetListUserGameData();
                List<CharacterBean> listWorkerData = gameDataManager.gameData.GetAllCharacterData();
                foreach (MiniGameCharacterBean itemCharacterData in listUserData)
                {
                    foreach (CharacterBean itemWorkerData in listWorkerData)
                    {
                        if (itemWorkerData.baseInfo.characterId==null|| itemCharacterData.characterData.baseInfo.characterId==null)
                        {
                            continue;
                        }
                        if (itemWorkerData.baseInfo.characterId.Equals(itemCharacterData.characterData.baseInfo.characterId))
                        {
                            WorkerEnum workerType = MiniGameEnumTools.GetWorkerTypeByMiniGameType(miniGameData.gameType);
                            CharacterWorkerBaseBean characterWorker = itemWorkerData.baseInfo.GetWorkerInfoByType(workerType);
                            if (miniGameData.GetGameResult()== MiniGameResultEnum.Win)
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
                UIMiniGameEnd uiMiniGameEnd = UIHandler.Instance.manager.OpenUIAndCloseOther<UIMiniGameEnd>(UIEnum.MiniGameEnd);
                uiMiniGameEnd.SetData(miniGameData);
                uiMiniGameEnd.SetCallBack(this);
            });
            //通知 游戏结束
            NotifyAllObserver((int)MiniGameStatusEnum.GameEnd, miniGameData);
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
        UIMiniGameCountDown uiCountDown = UIHandler.Instance.manager.OpenUIAndCloseOther<UIMiniGameCountDown>(UIEnum.MiniGameCountDown);
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
        if (gameTimeHandler != null)
            GameTimeHandler.Instance.SetTimeRestore();
        //通知 关闭游戏
        NotifyAllObserver((int)MiniGameStatusEnum.GameClose, miniGameData);
    }
    #endregion
}