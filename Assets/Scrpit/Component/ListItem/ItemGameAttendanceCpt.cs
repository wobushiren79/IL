using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ItemGameAttendanceCpt : ItemGameWorkerCpt,IRadioButtonCallBack
{
    public RadioButtonView rbAttendance;
    public Image ivAttendance;

    private ICallBack mCallBack;

    protected WorkerStatusEnum workerStatus = WorkerStatusEnum.Other;
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
        this.workerStatus = workerStatus;
        characterData.baseInfo.SetWorkerStatus(workerStatus);
        characterData.baseInfo.GetWorkerStatus(out string workerStatusStr);
        if (workerStatus == WorkerStatusEnum.Rest || workerStatus == WorkerStatusEnum.Work)
        {
            rbAttendance.SetEnabled(true);
            if (workerStatus == WorkerStatusEnum.Work)
            {
                rbAttendance.ChangeStates(RadioButtonView.RadioButtonStatus.Selected);
            }
            else if (workerStatus == WorkerStatusEnum.Rest)
            {
                rbAttendance.ChangeStates(RadioButtonView.RadioButtonStatus.Unselected);
            }
            ivAttendance.gameObject.SetActive(true);
        }
        else
        {
            rbAttendance.SetEnabled(false);
            rbAttendance.ChangeStates(RadioButtonView.RadioButtonStatus.Unselected);
            ivAttendance.gameObject.SetActive(false);
        }
        rbAttendance.rbText.text = workerStatusStr;
        if (mCallBack != null)
            mCallBack.AttendanceChange(this, workerStatus, characterData);
    }


    #region RB回调
    public void RadioButtonSelected(RadioButtonView view, RadioButtonView.RadioButtonStatus buttonStatus)
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        if (buttonStatus == RadioButtonView.RadioButtonStatus.Selected)
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