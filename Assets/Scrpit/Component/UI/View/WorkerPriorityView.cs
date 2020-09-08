using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class WorkerPriorityView : BaseMonoBehaviour,IRadioButtonCallBack
{
    public RadioButtonView rbWork;
    public InputField etPriority;

    protected ICallBack callBack;

    private void Awake()
    {
        if(rbWork)
            rbWork.SetCallBack(this);
        if (etPriority)
            etPriority.onEndEdit.AddListener(OnEndEditForChangePriority);
    }

    public void SetCallBack(ICallBack callBack)
    {
        this.callBack = callBack;
    }

    public void SetData(bool isWork, int priority)
    {
        SetWork(isWork);
        SetPriority(priority);
    }

    public void SetWork(bool isWork)
    {
        if (rbWork != null)
        {
            if (isWork)
                rbWork.ChangeStates(RadioButtonView.RadioButtonStatus.Selected);
            else
                rbWork.ChangeStates(RadioButtonView.RadioButtonStatus.Unselected);
        }
    }

    public void SetPriority(int priority)
    {
        if (etPriority!=null)
        {
            etPriority.text = "" + priority;
        }
    }

    /// <summary>
    /// 监听 优先级修改
    /// </summary>
    /// <param name="value"></param>
    public void OnEndEditForChangePriority(string value)
    {

        if (int.TryParse(value,out int result))
        {
            if (callBack != null)
                callBack.ChangePriority(this, result);
        }
        else
        {
            etPriority.text = "0";
        }
       
    }


    public interface ICallBack
    {
        /// <summary>
        /// 改变优先级
        /// </summary>
        /// <param name="view"></param>
        /// <param name="priority"></param>
         void ChangePriority(WorkerPriorityView view, int priority);

        /// <summary>
        /// 改变状态
        /// </summary>
        /// <param name="view"></param>
        /// <param name="isWork"></param>
         void ChangeStatus(WorkerPriorityView view, bool isWork);
    }

    #region 职责回调
    public void RadioButtonSelected(RadioButtonView view, RadioButtonView.RadioButtonStatus buttonStates)
    {
        bool isWork = false;
        if (buttonStates == RadioButtonView.RadioButtonStatus.Selected)
        {
            isWork = true;
        }
        else
        {
            isWork = false;
        }
        if (callBack!=null)
        {
            callBack.ChangeStatus(this,isWork);
        }
    }
    #endregion
}