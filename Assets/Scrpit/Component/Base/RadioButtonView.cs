using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class RadioButtonView : BaseMonoBehaviour
{
    //选中时
    public Sprite spSelected;
    public Color colorIVSelected;
    public Color colorTVSelected;

    //未选中时
    public Sprite spUnselected;
    public Color colorIVUnselected;
    public Color colorTVUnselected;

    public Button rbButton;
    public Image rbImage;
    public Text rbText;

    private IRadioButtonCallBack mRBCallBack;

    public enum RadioButtonStatus
    {
        Selected,//选中状态
        Unselected,//未选中状态
    }

    public RadioButtonStatus status = RadioButtonStatus.Selected;

    private void Start()
    {
        ChangeStates(status);
        rbButton.onClick.AddListener(RadioButtonSelected);
    }

    /// <summary>
    /// 按钮选择触发
    /// </summary>
    public void RadioButtonSelected()
    {
        ChangeStates();
        if (mRBCallBack != null)
        {
            mRBCallBack.RadioButtonSelected(this, status);
        }
    }

    /// <summary>
    /// 设置回调
    /// </summary>
    /// <param name="callback"></param>
    public void SetCallBack(IRadioButtonCallBack callback)
    {
        this.mRBCallBack = callback;
    }


    /// <summary>
    /// 改变状态
    /// </summary>
    /// <param name="status"></param>
    public void ChangeStates(RadioButtonStatus status)
    {
        if (rbButton.enabled == false)
            return;
        this.status = status;
        switch (status)
        {
            case RadioButtonStatus.Selected:
                if (rbImage) {
                    rbImage.sprite = spSelected;
                    rbImage.color = colorIVSelected;
                }
                if (rbText)
                    rbText.color = colorTVSelected;
                break;
            case RadioButtonStatus.Unselected:
                if (rbImage) {
                    rbImage.sprite = spUnselected;
                    rbImage.color = colorIVUnselected;
                }
                if (rbText)
                    rbText.color = colorTVUnselected;
                break;
        }
    }


    public void ChangeStates()
    {
        if (rbButton.enabled == false)
            return;
        if (status == RadioButtonStatus.Selected)
        {
            status = RadioButtonStatus.Unselected;
        }
        else
        {
            status = RadioButtonStatus.Selected;
        }
        ChangeStates(status);
    }

    /// <summary>
    /// 设置是否开启点击
    /// </summary>
    /// <param name="enabled"></param>
    public void SetEnabled(bool enabled)
    {
        rbButton.enabled = enabled;
    }

    public bool GetEnabled()
    {
        return rbButton.enabled;
    }
}