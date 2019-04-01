using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class UIMainCreate : BaseUIComponent, IRadioGroupCallBack, ColorView.CallBack, SelectView.CallBack
{
    //返回按钮
    public Button btBack;
    public Text tvBack;
    //开始按钮
    public Button btCreate;
    public Text tvCreate;
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
    //帽子
    public SelectView selectHat;
    //衣服
    public SelectView selectClothes;
    //鞋子
    public SelectView selectShoes;

    //角色身体控制
    public CharacterBodyCpt characterBodyCpt;
    public CharacterBodyManager characterBodyManager;
    //角色着装控制
    public CharacterDressCpt characterDressCpt;
    public CharacterDressManager characterDressManager;

    private void Start()
    {
        if (btBack != null)
            btBack.onClick.AddListener(OpenStartUI);
        if (btCreate != null)
            btCreate.onClick.AddListener(CreateNewGame);
        if (rgSex != null)
            rgSex.SetCallBack(this);
        if (colorSkin != null)
            colorSkin.SetCallBack(this);
        if (colorHair != null)
            colorHair.SetCallBack(this);
        if (selectHair != null)
        {
            selectHair.SetSelectNumber(characterBodyManager.listIconBodyHair.Count);
            selectHair.SetCallBack(this);
        }
        if (colorEye != null)
            colorEye.SetCallBack(this);
        if (selectEye != null)
        {
            selectEye.SetSelectNumber(characterBodyManager.listIconBodyEye.Count);
            selectEye.SetCallBack(this);
        }
        if (colorMouth != null)
            colorMouth.SetCallBack(this);
        if (selectMouth != null)
        {
            selectMouth.SetSelectNumber(characterBodyManager.listIconBodyMouth.Count);
            selectMouth.SetCallBack(this);
        }

        if (selectHat != null)
        {
            selectHat.SetSelectNumber(characterDressManager.GetHatList().Count + 1);
            selectHat.SetCallBack(this);
        }
        if (selectClothes != null)
        {
            selectClothes.SetSelectNumber(characterDressManager.GetClothesList().Count + 1);
            selectClothes.SetCallBack(this);
        }
        if (selectShoes != null)
        {
            selectShoes.SetSelectNumber(characterDressManager.GetShoesList().Count + 1);
            selectShoes.SetCallBack(this);
        }
    }

    /// <summary>
    /// 创建新游戏
    /// </summary>
    public void CreateNewGame()
    {

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
    public void ChangeSelectPosition(SelectView selectView, int position)
    {
        if (selectView == selectHair)
        {
            characterBodyCpt.SetHair(characterBodyManager.GetHairIconBeanByPosition(position).key);
        }
        else if (selectView == selectEye)
        {
            characterBodyCpt.SetEye(characterBodyManager.GetEyeIconBeanByPosition(position).key);
        }
        else if (selectView == selectMouth)
        {
            characterBodyCpt.SetMouth(characterBodyManager.GetMouthIconBeanByPosition(position).key);
        }
        else if (selectView == selectHat)
        {
            if (position == 0)
                characterDressCpt.SetHat(null);
            else
                characterDressCpt.SetHat(characterDressManager.GetHatList()[position - 1]);
        }
        else if (selectView == selectClothes)
        {
            if (position == 0)
                characterDressCpt.SetClothes(null);
            else
                characterDressCpt.SetClothes(characterDressManager.GetClothesList()[position-1]);
        }
        else if (selectView == selectShoes)
        {
            if (position == 0)
                characterDressCpt.SetShoes(null);
            else
                characterDressCpt.SetShoes(characterDressManager.GetShoesList()[position - 1]);
        }
    }
    #endregion

}