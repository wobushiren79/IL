using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ItemGameAttendanceCpt : BaseMonoBehaviour,IRadioButtonCallBack
{
    public Text tvName;
    public InfoPromptPopupButton pbName;

    public Text tvPrice;
    public InfoPromptPopupButton pbPrice;

    public Text tvLoyal;
    public InfoPromptPopupButton pbLoyal;

    public Text tvSpeed;
    public InfoPromptPopupButton pbSpeed;
    public Text tvAccount;
    public InfoPromptPopupButton pbAccount;
    public Text tvCharm;
    public InfoPromptPopupButton pbCharm;
    public Text tvCook;
    public InfoPromptPopupButton pbCook;
    public Text tvForce;
    public InfoPromptPopupButton pbForce;
    public Text tvLucky;
    public InfoPromptPopupButton pbLucky;

    public RadioButtonView rbAttendance;

    public CharacterUICpt characterUICpt;
    public CharacterBean characterData;

    private CallBack mCallBack;



    public void SetData(CharacterBean data)
    {
        if (data == null)
            return;
        characterData = data;
        if (characterData.baseInfo != null)
        {
            CharacterBaseBean characterBase = characterData.baseInfo;
            SetName(characterBase.name);
            SetPrice(characterBase.priceS, characterBase.priceM, characterBase.priceL);
            rbAttendance.SetCallBack(this);
            SetAttendance(characterBase.isAttendance);
        }
        if (characterData.attributes != null)
        {
            CharacterAttributesBean characterAttributes = characterData.attributes;
            SetLoyal(characterAttributes.loyal);
        }
        if (data.body != null && data.equips != null)
            characterUICpt.SetCharacterData(data.body, data.equips);
    }

    public void SetCallBack(CallBack callBack)
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

    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        if (tvName == null)
            return;
        tvName.text = name;
    }

    /// <summary>
    /// 设置工资
    /// </summary>
    /// <param name="priceS"></param>
    /// <param name="priceM"></param>
    /// <param name="priceL"></param>
    public void SetPrice(long priceS, long priceM, long priceL)
    {
        if (tvPrice == null)
            return;
        tvPrice.text = priceS + " / 天";
    }

    /// <summary>
    /// 设置忠诚度
    /// </summary>
    /// <param name="loyal"></param>
    public void SetLoyal(float loyal)
    {
        if (tvLoyal == null)
            return;
        tvLoyal.text = loyal + "";
    }

    #region RB回调
    public void RadioButtonSelected(RadioButtonView view, RadioButtonView.RadioButtonStates buttonStates)
    {
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

    public interface CallBack
    {
        void AttendanceChange(ItemGameAttendanceCpt itemView, bool isAttendance, CharacterBean characterBean);
    }
}