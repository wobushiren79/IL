using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIGameFamily : UIBaseOne
{
    public Text ui_TVMarryDate;
    public ProgressView ui_PVBirth;
    public ScrollGridVertical ui_FamilyList;

    public GameObject objItemModel;
    public GameObject objContainer;

    protected List<CharacterForFamilyBean> listFamilyData = new List<CharacterForFamilyBean>();

    public override void Awake()
    {
        base.Awake();
        if(ui_FamilyList)
            ui_FamilyList.AddCellListener(OnCellForFamilyList);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        FamilyDataBean familyData = gameData.GetFamilyData();
        listFamilyData = familyData.GetAllFamilyData();
        ui_FamilyList.SetCellCount(listFamilyData.Count);
        SetMarryDate(familyData.timeForMarry);
        SetBirthPro(familyData.birthPro);
    }

    public void InitData()
    {
        CptUtil.RemoveChildsByActive(objContainer);
    }

    /// <summary>
    /// 设置结婚日期
    /// </summary>
    /// <param name="time"></param>
    public void SetMarryDate(TimeBean time)
    {
        if (ui_TVMarryDate && time != null)
        {
            ui_TVMarryDate.text = string.Format(TextHandler.Instance.manager.GetTextById(78), time.year + "", time.month + "", time.day + "");
        }
    }

    /// <summary>
    /// 设置亲密度
    /// </summary>
    /// <param name="pro"></param>
    public void SetBirthPro(float pro)
    {
        if (ui_PVBirth)
        {
            ui_PVBirth.SetData(pro);
        }
    }

    /// <summary>
    /// 监听
    /// </summary>
    /// <param name="itemCell"></param>
    public void OnCellForFamilyList(ScrollGridCell itemCell)
    {
        ItemGameFamilyCpt itemCpt = itemCell.GetComponent<ItemGameFamilyCpt>();
        itemCpt.SetData(listFamilyData[itemCell.index]);
    }

}