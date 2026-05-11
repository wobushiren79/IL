using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using DG.Tweening;
using System;

public class UITownGuildAchievement : UIBaseOne, IRadioGroupCallBack
{
    public GameObject objGroceryContent;
    public GameObject objGroceryModel;

    public RadioGroupView rgGroceryType;

    public List<AchievementInfoBean> listAchData;

    public new void Start()
    {
        base.Start();
        if (rgGroceryType != null)
            rgGroceryType.SetCallBack(this);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        Action<List<AchievementInfoBean>> callBack = SetAchievementInfoData;
        AchievementInfoHandler.Instance.manager.GetAllAchievement(callBack);
        rgGroceryType.SetPosition(0, false);
    }

    public void InitDataByType(int type)
    {
        List<AchievementInfoBean> listTypeData = new List<AchievementInfoBean>();
        foreach (AchievementInfoBean itemData in listAchData)
        {
            if (type == itemData.type)
            {
                listTypeData.Add(itemData);
            }
        };
        CreateAchievementData(listTypeData);
    }

    /// <summary>
    /// 创建成就列表
    /// </summary>
    /// <param name="listData"></param>
    public void CreateAchievementData(List<AchievementInfoBean> listData)
    {
        CptUtil.RemoveChildsByActive(objGroceryContent.transform);
        if (listData == null || objGroceryContent == null || objGroceryModel == null)
            return;
        for (int i = 0; i < listData.Count; i++)
        {
            AchievementInfoBean itemData = listData[i];
            GameObject itemObj = Instantiate(objGroceryContent, objGroceryModel);
            ItemTownGuildAchievementCpt achCpt = itemObj.GetComponent<ItemTownGuildAchievementCpt>();
            achCpt.SetData(itemData);
            //itemObj.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBack).SetDelay(i * 0.02f).From();
        }
    }

    #region 类型选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView view)
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        InitDataByType(position);
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView view)
    {

    }
    #endregion


    /// <summary>
    /// 设置成就数据
    /// </summary>
    /// <param name="listData"></param>
    public void SetAchievementInfoData(List<AchievementInfoBean> listData)
    {
        listAchData = listData;
        InitDataByType(0);
    }
}