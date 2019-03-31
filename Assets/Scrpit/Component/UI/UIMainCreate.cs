using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class UIMainCreate : BaseUIComponent, IRadioGroupCallBack, ColorView.CallBack, SelectView.CallBack
{
    //返回按钮
    public Button btBack;
    public Text tvBack;
    //性别选择
    public RadioGroupView rgSex;
    //皮肤颜色
    public ColorView colorSkin;
    //发型
    public ColorView colorHair;
    public SelectView selectHair;
    //眼睛
    public ColorView colorEye;
    public SelectView selectEye;
    //嘴
    public ColorView colorMouth;
    public SelectView selectMouth;

    //角色身体控制
    public CharacterBodyCpt characterBodyCpt;
    public CharacterBodyManager characterBodyManager;
    private void Start()
    {
        if (btBack != null)
            btBack.onClick.AddListener(OpenStartUI);
        if (rgSex != null)
            rgSex.SetCallBack(this);
        if (colorSkin != null)
            colorSkin.SetCallBack(this);
        if (colorHair != null)
            colorHair.SetCallBack(this);
        if (selectHair != null)
        {
            selectHair.SetSelectData(characterBodyManager.listIconBodyHair);
            selectHair.SetCallBack(this);
        }
        if (colorEye != null)
            colorEye.SetCallBack(this);
        if (selectEye != null)
        {
            selectEye.SetSelectData(characterBodyManager.listIconBodyEye);
            selectEye.SetCallBack(this);
        }
        if (colorMouth != null)
            colorMouth.SetCallBack(this);
        if (selectMouth != null)
        {
            selectMouth.SetSelectData(characterBodyManager.listIconBodyMouth);
            selectMouth.SetCallBack(this);
        }
    }

    /// <summary>
    /// 返回开始菜单
    /// </summary>
    public void OpenStartUI()
    {
        uiManager.OpenUIAndCloseOtherByName("Start");
    }

    #region 性别回调
    public void RadioButtonSelected(int position, RadioButtonView view)
    {
        if (position == 0)
        {
            characterBodyCpt.SetSex(1);
        }
        else
        {
            characterBodyCpt.SetSex(2);
        }
    }

    public void RadioButtonUnSelected(int position, RadioButtonView view)
    {

    }
    #endregion

    #region 颜色回调
    public void ColorChange(ColorView colorView, float r, float g, float b)
    {
        if (colorView == colorSkin)
        {
            characterBodyCpt.SetSkin(colorSkin.GetColor());
        }
        else if (colorView == colorHair)
        {
            characterBodyCpt.SetHair(colorHair.GetColor());
        }
        else if (colorView == colorEye)
        {
            characterBodyCpt.SetEye(colorEye.GetColor());
        }
        else if (colorView == colorMouth)
        {
            characterBodyCpt.SetMouth(colorMouth.GetColor());
        }

    }
    #endregion

    #region 选择回调
    public void ChangeSelectPosition(SelectView selectView, int position, IconBean iconBean)
    {
        if (selectView == selectHair)
        {
            characterBodyCpt.SetHair(iconBean.key);
        }
        else if (selectView == selectEye)
        {
            characterBodyCpt.SetEye(iconBean.key);
        }
        else if (selectView == selectMouth)
        {
            characterBodyCpt.SetMouth(iconBean.key);
        }
    }
    #endregion

}