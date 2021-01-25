using UnityEngine;
using UnityEditor;

public abstract class BaseNormalSceneInit : BaseSceneInit, DialogView.IDialogCallBack
{

    public override void Start()
    {
        base.Start();
        //设置时间
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        TimeBean timeData = gameData.gameTime;
        GameTimeHandler.Instance.SetTime(timeData.hour, timeData.minute);
        GameTimeHandler.Instance.SetTimeStatus(false);
        //增加回调
        GameTimeHandler.Instance.RegisterNotifyForTime(NotifyForTime);

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
        if (GameControlHandler.Instance.GetControl().transform.position.y < -50)
        {
            AudioHandler.Instance.PauseEnvironment();
        }
    }


    /// <summary>
    /// 初始化角色位置
    /// </summary>
    public virtual ControlForMoveCpt InitUserPosition() {
        //开始角色控制
        ControlForMoveCpt moveControl = GameControlHandler.Instance.StartControl<ControlForMoveCpt>(GameControlHandler.ControlEnum.Normal);
        return moveControl;
    }


    /// <summary>
    /// 结束一天
    /// </summary>
    public virtual void EndDay()
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        //停止时间
        GameTimeHandler.Instance.SetTimeStatus(true);
        //停止控制
        GameControlHandler.Instance.EndControl();

        //重置游戏时间
        GameTimeHandler.Instance.SetDayStatus(GameTimeHandler.DayEnum.End);


        DialogBean dialogBean = new DialogBean();
        if (GameTimeHandler.Instance.GetDayStatus() == GameTimeHandler.DayEnum.Work)
        {
            dialogBean.content = GameCommonInfo.GetUITextById(3006);
        }
        else if (GameTimeHandler.Instance.GetDayStatus() == GameTimeHandler.DayEnum.Rest)
        {
            dialogBean.content = GameCommonInfo.GetUITextById(3014);
        }
        DialogHandler.Instance.CreateDialog<DialogView>(DialogEnum.Text, this, dialogBean, 5);
    }
    #region  时间通知回调
    public void NotifyForTime(GameTimeHandler.NotifyTypeEnum notifyType, float timeHour)
    {
        if (notifyType == GameTimeHandler.NotifyTypeEnum.EndDay)
        {
            EndDay();
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