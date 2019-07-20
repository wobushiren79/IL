using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using DG.Tweening;

public class UITownGuildAchievement : UIBaseOne, IRadioGroupCallBack,IAchievementInfoView
{
    public GameObject objGroceryContent;
    public GameObject objGroceryModel;

    public RadioGroupView rgGroceryType;

    private AchievementInfoController mAchievementInfoController;
    public List<AchievementInfoBean> listAchData;
    private void Awake()
    {
        mAchievementInfoController = new AchievementInfoController(this,this);
        mAchievementInfoController.GetAllAchievementInfo();
    }

    public new void Start()
    {
        base.Start();
        if (rgGroceryType != null)
            rgGroceryType.SetCallBack(this);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        rgGroceryType.SetPosition(0, false);
        InitDataByType(0);
    }

    public void InitDataByType(int type)
    {
        switch (type)
        {
            case 0:
                break;
            case 1:
                break;
        }
        CreateAchievementData(listAchData);
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
            AchievementInfoBean itemData = listAchData[i];
            GameObject itemObj = Instantiate(objGroceryModel, objGroceryContent.transform);
            itemObj.SetActive(true);
            ItemGameGuildAchievementCpt achCpt = itemObj.GetComponent<ItemGameGuildAchievementCpt>();
            achCpt.SetData(itemData);
            itemObj.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBack).SetDelay(i * 0.05f).From();
        }
    }

    #region 类型选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView view)
    {
        InitDataByType(position);
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView view)
    {

    }
    #endregion

    #region 数据回调
    public void GetAllAchievementInfoSuccess(List<AchievementInfoBean> listData)
    {
        listAchData = listData;
    }

    public void GetAllAchievementInfoFail()
    {

    }
    #endregion 数据回调
}