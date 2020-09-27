using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class UITownBeautySalon : UIBaseOne,IRadioGroupCallBack
{
    public RadioGroupView rgType;

    public ColorView cvHair;
    public ColorView cvMouth;
    public ColorView cvEye;
    public ColorView cvSkin;

    public Button btSubmit;
    public Button btSelectCharacter;
    public Button btClear;

    public ScrollGridVertical gridVertical;

    public string selectHair;
    public string selectMouth;
    public string selectEye;
    public string selectSkin;

    public CharacterBean characterData;

    public override void Awake()
    {
        base.Awake();
        if (btSubmit != null)
            btSubmit.onClick.AddListener(OnClickForSubmit);
        if (btSelectCharacter != null)
            btSelectCharacter.onClick.AddListener(OnClickForSelectCharacter);
        if (btClear != null)
            btClear.onClick.AddListener(OnClickForClear);
        if (rgType != null)
            rgType.SetCallBack(this);
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        SetColorData();
    }

    /// <summary>
    /// 设置颜色数据
    /// </summary>
    public void SetColorData()
    {
        if (CheckUtil.StringIsNull(selectHair))
        {
            cvHair.gameObject.SetActive(false);
        }
        else
        {
            cvHair.gameObject.SetActive(true);
        }

        if (CheckUtil.StringIsNull(selectMouth))
        {
            cvMouth.gameObject.SetActive(false);
        }
        else
        {
            cvMouth.gameObject.SetActive(true);
        }

        if (CheckUtil.StringIsNull(selectEye))
        {
            cvEye.gameObject.SetActive(false);
        }
        else
        {
            cvEye.gameObject.SetActive(true);
        }

        if (CheckUtil.StringIsNull(selectSkin))
        {
            cvSkin.gameObject.SetActive(false);
        }
        else
        {
            cvSkin.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 清理数据
    /// </summary>
    public void ClearData()
    {
        selectHair = "";
        selectMouth = "";
        selectEye = "";
        selectSkin = "";
        RefreshUI();
    }

    public void OnClickForSelectCharacter()
    {

    }

    public void OnClickForClear()
    {
        ClearData();
    }

    public void OnClickForSubmit()
    {

    }


    #region 类型选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        switch (rbview.name)
        {
            case "Hair":
                break;
            case "Eye":
                break;
            case "Mouth":
                break;
            case "Skin":
                break;
        }
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion
}