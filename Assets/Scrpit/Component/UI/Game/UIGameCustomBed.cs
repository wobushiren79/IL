using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class UIGameCustomBed : UIBaseOne, StoreInfoManager.ICallBack, IRadioGroupCallBack
{
    public BuildBedBean customeBedData;

    public RadioGroupView rgBedType;

    public List<StoreInfoBean> listBedData;

    public GameObject objItemContainer;
    public GameObject objItemModel;

    public override void Awake()
    {
        base.Awake();
        if (rgBedType != null)
            rgBedType.SetCallBack(this);
    }

    public override void OpenUI()
    {
        base.OpenUI();

        CptUtil.RemoveChildsByActive(objItemContainer);

        uiGameManager.storeInfoManager.SetCallBack(this);
        uiGameManager.storeInfoManager.GetStoreInfoForCarpenterBed();

        if (customeBedData == null)
            customeBedData = new BuildBedBean();
    }

    /// <summary>
    /// 创建列表数据
    /// </summary>
    /// <param name="listData"></param>
    public void CreateBedDataList(List<StoreInfoBean> listData)
    {
        if (listData == null)
        {
            return;
        }
        for (int i = 0; i < listData.Count; i++)
        {
            StoreInfoBean storeInfo = listData[i];
            GameObject objItem = Instantiate(objItemContainer, objItemModel);
        }
    }

    /// <summary>
    /// 根据类型获取数据
    /// </summary>
    /// <param name="buildItemType"></param>
    /// <returns></returns>
    public List<StoreInfoBean> GetListBedDataByType(BuildItemTypeEnum buildItemType)
    {
        List<StoreInfoBean> listData = new List<StoreInfoBean>();
        if (listBedData == null)
            return listData;
        InnBuildManager innBuildManager = uiGameManager.innBuildManager;
        for (int i = 0; i < listBedData.Count; i++)
        {
            StoreInfoBean storeInfo = listBedData[i];
            BuildItemBean buildItem = innBuildManager.GetBuildDataById(storeInfo.mark_id);
            if (buildItem.GetBuildType() == buildItemType)
            {
                listData.Add(storeInfo);
            }

        }
        return listData;
    }

    public override void OnClickForBack()
    {
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.TownCarpenter));
    }

    #region 获取数据回调
    public void GetStoreInfoSuccess(StoreTypeEnum type, List<StoreInfoBean> listData)
    {
        listBedData = listData;
        rgBedType.SetPosition(0, true);
    }
    #endregion

    #region 类型选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        BuildItemTypeEnum buildItemType = EnumUtil.GetEnum<BuildItemTypeEnum>(rbview.name);
        List<StoreInfoBean> listData = GetListBedDataByType(buildItemType);
        CreateBedDataList(listData);
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion
}