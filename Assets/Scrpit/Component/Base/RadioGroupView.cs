using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class RadioGroupView : BaseMonoBehaviour, IRadioButtonCallBack
{
    //是否能取消选择想
    public bool isCancelSelect = false;
    //按钮列表
    public List<RadioButtonView> listButton;
    private IRadioGroupCallBack mRGCallBack;


    private void Start()
    {
        if (CheckUtil.ListIsNull(listButton))
        {
            return;
        }
        foreach (RadioButtonView itemRB in listButton)
        {
            itemRB.SetCallBack(this);
        }
    }

    public void SetPosition(int position, bool isCallBack)
    {
        if (listButton == null)
            return;
        if (position > listButton.Count)
            return;
        for (int i = 0; i < listButton.Count; i++)
        {
            RadioButtonView itemRB = listButton[i];
            if (i == position)
            {
                itemRB.ChangeStates(RadioButtonView.RadioButtonStatus.Selected);
                if (isCallBack)
                {
                    if (mRGCallBack != null)
                        mRGCallBack.RadioButtonSelected(this,i, itemRB);
                }
            }
            else
            {
                itemRB.ChangeStates(RadioButtonView.RadioButtonStatus.Unselected);
            }
        }
    }

    /// <summary>
    /// 自动找到rb
    /// </summary>
    public void AutoFindRadioButton()
    {
        if (listButton == null)
            listButton = new List<RadioButtonView>();
        listButton.Clear();
        RadioButtonView[] rbList = GetComponentsInChildren<RadioButtonView>();
        if (rbList != null)
            listButton = TypeConversionUtil.ArrayToList(rbList);
        if (listButton != null)
            foreach (RadioButtonView itemRB in listButton)
            {
                itemRB.SetCallBack(this);
            }
    }

    /// <summary>
    /// 设置回调
    /// </summary>
    /// <param name="callback"></param>
    public void SetCallBack(IRadioGroupCallBack callback)
    {
        this.mRGCallBack = callback;
    }

    public void RadioButtonSelected(RadioButtonView view, RadioButtonView.RadioButtonStatus buttonStates)
    {
        if (CheckUtil.ListIsNull(listButton))
        {
            return;
        }
        for (int i = 0; i < listButton.Count; i++)
        {
            RadioButtonView itemRB = listButton[i];
            if (itemRB.Equals(view))
            {
                if (!isCancelSelect)
                {
                    itemRB.ChangeStates(RadioButtonView.RadioButtonStatus.Selected);
                }
                if (mRGCallBack != null)
                    mRGCallBack.RadioButtonSelected(this,i, itemRB);
            }
            else
            {
                itemRB.ChangeStates(RadioButtonView.RadioButtonStatus.Unselected);
                if (mRGCallBack != null)
                    mRGCallBack.RadioButtonUnSelected(this,i, itemRB);
            }
        }
    }
}