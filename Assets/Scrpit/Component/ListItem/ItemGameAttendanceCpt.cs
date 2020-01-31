using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ItemGameAttendanceCpt : ItemGameWorkerCpt,IRadioButtonCallBack
{
    public RadioButtonView rbAttendance;

    private ICallBack mCallBack;


    public override void SetData(CharacterBean data)
    {
        base.SetData(data);
        if (data == null)
            return;
        characterData = data;
        if (characterData.baseInfo != null)
        {
            rbAttendance.SetCallBack(this);
            SetAttendance(characterData.baseInfo.GetWorkerStatus());
        }
    }

    public void SetCallBack(ICallBack callBack)
    {
        this.mCallBack = callBack;
    }

    /// <summary>
    /// 设置出勤
    /// </summary>
    /// <param name="isAttendance"></param>
    public void SetAttendance(WorkerStatusEnum workerStatus)
    {
        characterData.baseInfo.SetWorkerStatus(workerStatus);
        characterData.baseInfo.GetWorkerStatus(out string workerStatusStr);
        if (workerStatus == WorkerStatusEnum.Work)
        {
            rbAttendance.ChangeStates(RadioButtonView.RadioButtonStates.Selected);
        }
        else if (workerStatus == WorkerStatusEnum.Rest)
        {
            rbAttendance.ChangeStates(RadioButtonView.RadioButtonStates.Unselected);
        }
        else
        {
            rbAttendance.ChangeStates(RadioButtonView.RadioButtonStates.Unselected);
            rbAttendance.SetEnabled(false);
        }
        rbAttendance.rbText.text = workerStatusStr;
        if (mCallBack != null)
            mCallBack.AttendanceChange(this, workerStatus, characterData);
    }

    #region RB回调
    public override void RadioButtonSelected(RadioButtonView view, RadioButtonView.RadioButtonStates buttonStates)
    {
        GetUIManager<UIGameManager>().audioHandler.PlaySound(SoundEnum.ButtonForNormal);
        if (buttonStates == RadioButtonView.RadioButtonStates.Selected)
        {
            SetAttendance(WorkerStatusEnum.Work);
        }
        else
        {
            SetAttendance(WorkerStatusEnum.Rest);
        }
      
    }
    #endregion

    public interface ICallBack
    {
        void AttendanceChange(ItemGameAttendanceCpt itemView, WorkerStatusEnum workerStatus, CharacterBean characterBean);
    }
}