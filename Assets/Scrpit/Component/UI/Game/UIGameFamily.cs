using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIGameFamily : UIBaseOne
{
    public Text ui_TVMarryDate;
    public ProgressView ui_PVBirth;

    public GameObject objItemModel;
    public GameObject objContainer;

    public override void OpenUI()
    {
        base.OpenUI();
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        FamilyDataBean familyData = gameData.GetFamilyData();
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
            ui_TVMarryDate.text = string.Format(GameCommonInfo.GetUITextById(78), time.year + "", time.month + "", time.day + "");
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

    public void CreateFamilyItem(CharacterBean characterData)
    {
        GameObject objItem = Instantiate(objContainer, objItemModel);
        ItemGameFamilyCpt itemCpt = objItem.GetComponent<ItemGameFamilyCpt>();
        itemCpt.SetData(characterData);
    }

}