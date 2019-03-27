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

    public enum RadioButtonStates
    {
        Selected,//选中状态
        Unselected,//未选中状态
    }

    public RadioButtonStates states = RadioButtonStates.Selected;

    private void Start()
    {
        rbButton.onClick.AddListener(RadioButtonSelected);
    }

    /// <summary>
    /// 按钮选择触发
    /// </summary>
    public void RadioButtonSelected()
    {
        if (mRBCallBack != null)
            mRBCallBack.RadioButtonSelected(this);
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
    /// <param name="states"></param>
    public void ChangeStates(RadioButtonStates states)
    {
        switch (states)
        {
            case RadioButtonStates.Selected:
                if (rbImage) {
                    rbImage.sprite = spSelected;
                    rbImage.color = colorIVSelected;
                }
                if (rbText)
                    rbText.color = colorTVSelected;
                break;
            case RadioButtonStates.Unselected:
                if (rbImage) {
                    rbImage.sprite = spUnselected;
                    rbImage.color = colorIVUnselected;
                }
                if (rbText)
                    rbText.color = colorTVUnselected;
                break;
        }
    }


}