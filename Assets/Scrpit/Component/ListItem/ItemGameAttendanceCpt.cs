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
            SetAttendance(characterData.baseInfo.isAttendance);
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
    public void SetAttendance(bool isAttendance)
    {
        if (isAttendance)
        {
            rbAttendance.ChangeStates(RadioButtonView.RadioButtonStates.Selected);
            rbAttendance.rbText.text = "出勤";
        }
        else
        {
            rbAttendance.ChangeStates(RadioButtonView.RadioButtonStates.Unselected);
            rbAttendance.rbText.text = "休息";
        }
        characterData.baseInfo.isAttendance = isAttendance;
        if (mCallBack != null)
            mCallBack.AttendanceChange(this, isAttendance, characterData);
    }

    #region RB回调
    public override void RadioButtonSelected(RadioButtonView view, RadioButtonView.RadioButtonStates buttonStates)
    {
        GetUIManager<UIGameManager>().audioHandler.PlaySound(SoundEnum.ButtonForNormal);
        if (buttonStates == RadioButtonView.RadioButtonStates.Selected)
        {
            SetAttendance(true);
        }
        else
        {
            SetAttendance(false);
        }
      
    }
    #endregion

    public interface ICallBack
    {
        void AttendanceChange(ItemGameAttendanceCpt itemView, bool isAttendance, CharacterBean characterBean);
    }
}