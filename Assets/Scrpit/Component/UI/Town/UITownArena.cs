using UnityEngine;
using UnityEditor;

public class UITownArena : UIBaseOne,IRadioGroupCallBack
{
    public GameObject objArenaContainer;
    public GameObject objArenaModel;

    public RadioGroupView rgType;

    public override void Start()
    {
        base.Start();
        if (rgType != null)
        {
            rgType.SetCallBack(this);
        }
    }

    public override void OpenUI()
    {
        base.OpenUI();
        rgType.SetPosition(0, false);
        InitData(0);
    }
    
    public void InitData(int type)
    {

    }

    #region 等级选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion
}