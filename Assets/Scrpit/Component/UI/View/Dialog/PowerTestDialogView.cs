using UnityEngine;
using UnityEditor;

public class PowerTestDialogView : DialogView, StrengthTestView.ICallBack
{
    //力度测试
    public StrengthTestView strengthTest;

    public float resultsForce;

    private ICallBack mCallBack;

    public override void Start()
    {
        base.Start();
        if (strengthTest != null)
            strengthTest.SetCallBack(this);
    }

    /// <summary>
    /// 设置回调
    /// </summary>
    /// <param name="callBack"></param>
    public void SetCallBack(ICallBack callBack)
    {
        this.mCallBack = callBack;
    }

    /// <summary>
    /// 设置武力数值
    /// </summary>
    /// <param name="force"></param>
    public void SetData(float moveSpeed,float timeDelayDelete)
    {
        SetDelayDelete(timeDelayDelete);
        resultsForce = 0;
        if (moveSpeed < 0.1f)
            moveSpeed = 0.1f;
        strengthTest.SetData(GameCommonInfo.GetUITextById(52), moveSpeed);
        StartPowerTest();
    }

    public void StartPowerTest()
    {
        strengthTest.StartTest();
    }

    #region 力度测试回调
    public void StrengthTestEnd(StrengthTestView view, float value)
    {
        if (mCallBack != null)
            mCallBack.PowerTestEnd(value);
        DestroyDialog();
    }
    #endregion

    public interface ICallBack
    {
        void PowerTestEnd(float power);
    }
}