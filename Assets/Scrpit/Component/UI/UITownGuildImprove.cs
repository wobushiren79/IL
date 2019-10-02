using UnityEngine;
using UnityEditor;

public class UITownGuildImprove : UIBaseOne,IRadioGroupCallBack
{
    public RadioGroupView rgType;
    public GameObject objTypeInn;
    public GameObject objTypeCharacter;

    private new void Start()
    {
        base.Start();
        ChangeUIType(0);
    }

    /// <summary>
    /// 改变UI类型
    /// </summary>
    /// <param name="type"></param>
    public void ChangeUIType(int type)
    {
        objTypeInn.SetActive(false);
        objTypeCharacter.SetActive(false);
        switch (type)
        {
            case 0:
                objTypeInn.SetActive(true);
                break;
            case 1:
                objTypeCharacter.SetActive(true);
                break;
        }
    }

    #region 类型选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        ChangeUIType(position);
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion
}