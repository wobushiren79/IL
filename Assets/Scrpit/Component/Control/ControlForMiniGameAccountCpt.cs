using UnityEngine;
using UnityEditor;

public class ControlForMiniGameAccountCpt : BaseControl
{
    private ICallBack mCallBack;

    public void SetCallBack(ICallBack mCallBack)
    {
        this.mCallBack = mCallBack;
    }

    private void Update()
    {
        if (Input.GetButtonDown(InputInfo.Interactive_E)|| Input.GetButtonDown(InputInfo.Interactive_Space))
        {
            if (mCallBack != null)
                mCallBack.OnClickLaunch();
        }
    }

    public interface ICallBack
    {
        /// <summary>
        /// 点击发射
        /// </summary>
        void OnClickLaunch();
    }
}