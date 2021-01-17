using UnityEngine;
using UnityEditor;

public abstract class BaseNormalSceneInit : BaseSceneInit,IBaseObserver, DialogView.IDialogCallBack
{

    public override void Start()
    {
        base.Start();
        //设置时间
        if (gameTimeHandler != null && gameDataManager != null)
        {
            TimeBean timeData = gameDataManager.gameData.gameTime;
            gameTimeHandler.SetTime(timeData.hour, timeData.minute);
            gameTimeHandler.SetTimeStatus(false);
            //增加回调
            gameTimeHandler.AddObserver(this);
        }
        //获取团队NPC信息
        if (npcTeamManager != null)
        {
            npcTeamManager.npcTeamController.GetNpcTeamByType(NpcTeamTypeEnum.Customer);
        }

        //设置角色位置
        InitUserPosition();
        //设置天气
        InitWeather();
    }

    /// <summary>
    /// 初始化天气
    /// </summary>
    public virtual void InitWeather()
    {
        //设置天气
        if (weatherHandler != null)
        {
            weatherHandler.SetWeahter(GameCommonInfo.CurrentDayData.weatherToday);
        }
        //如果是在室内
        if (controlHandler.GetControl().transform.position.y < -50)
        {
            audioHandler.PauseEnvironment();
        }
    }


    /// <summary>
    /// 初始化角色位置
    /// </summary>
    public virtual ControlForMoveCpt InitUserPosition() {
        //开始角色控制
        ControlForMoveCpt moveControl = (ControlForMoveCpt)controlHandler.StartControl(ControlHandler.ControlEnum.Normal);
        return moveControl;
    }


    /// <summary>
    /// 结束一天
    /// </summary>
    public virtual void EndDay()
    {
        //停止时间
        if (gameTimeHandler != null)
            gameTimeHandler.SetTimeStatus(true);
        //停止控制
        if (controlHandler != null)
            controlHandler.EndControl();

        //重置游戏时间
        if (gameTimeHandler != null)
            gameTimeHandler.SetDayStatus(GameTimeHandler.DayEnum.End);

        if (dialogManager != null)
        {
            DialogBean dialogBean = new DialogBean();
            if (gameTimeHandler.GetDayStatus()== GameTimeHandler.DayEnum.Work)
            {
                dialogBean.content = GameCommonInfo.GetUITextById(3006);
            }
            else if (gameTimeHandler.GetDayStatus() == GameTimeHandler.DayEnum.Rest)
            {
                dialogBean.content = GameCommonInfo.GetUITextById(3014);
            }
            DialogHandler.Instance.CreateDialog<DialogView>(DialogEnum.Text, this, dialogBean, 5);
        }
        else
        {
            SceneUtil.SceneChange(ScenesEnum.GameInnScene);
        }
    }
    #region  时间通知回调
    public void ObserbableUpdate<T>(T observable, int type, params System.Object[] obj) where T : UnityEngine.Object
    {
        if (observable == gameTimeHandler)
        {
            if (type == (int)GameTimeHandler.NotifyTypeEnum.EndDay)
            {
                EndDay();
            }
        }
    }
    #endregion

    #region  弹窗通知回调
    public void Submit(DialogView dialogView, DialogBean dialogData)
    {
        SceneUtil.SceneChange(ScenesEnum.GameInnScene);
    }

    public void Cancel(DialogView dialogView, DialogBean dialogData)
    {
        SceneUtil.SceneChange(ScenesEnum.GameInnScene);
    }
    #endregion

}