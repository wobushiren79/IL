using UnityEngine;
using UnityEditor;

public class CombatPowerView : BaseMonoBehaviour, StrengthTestView.ICallBack
{
    //准度测试
    public StrengthTestView strengthTestForAccuracy;
    //力度测试
    public StrengthTestView strengthTestForForce;

    public float resultsAccuracy;
    public float resultsForce;

    private ICallBack mCallBack;

    private void Start()
    {
        if (strengthTestForAccuracy != null)
            strengthTestForAccuracy.SetCallBack(this);
        if (strengthTestForForce != null)
            strengthTestForForce.SetCallBack(this);
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
    public void SetData(int force)
    {
        resultsAccuracy = 0;
        resultsForce = 0;
        float moveSpeed = 6f -(float)force/20f;
        if (moveSpeed < 0.1f)
            moveSpeed = 0.1f;
        strengthTestForAccuracy.SetData( GameCommonInfo.GetUITextById(51),moveSpeed);
        strengthTestForForce.SetData(GameCommonInfo.GetUITextById(52), moveSpeed);
        StartPowerTest();
    }

    public void StartPowerTest()
    {
        strengthTestForAccuracy.StartTest();
    }

    #region 力度测试回调
    public void StrengthTestEnd(StrengthTestView view, float value)
    {
        if (view == strengthTestForAccuracy)
        {
            resultsAccuracy = value;
            strengthTestForForce.StartTest();
        }
        else if (view == strengthTestForForce)
        {
            resultsForce = value;
            if (mCallBack != null)
                mCallBack.PowerTestEnd(resultsAccuracy, resultsForce);
        }
    }
    #endregion

    public interface ICallBack
    {
        void PowerTestEnd(float resultsAccuracy,float resultsForce);
    }
}