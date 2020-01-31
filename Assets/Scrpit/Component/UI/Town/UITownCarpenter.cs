using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using DG.Tweening;

public class UITownCarpenter : UIBaseOne, IRadioGroupCallBack, StoreInfoManager.ICallBack
{
    public RadioGroupView rgCerpenterType;

    public GameObject objCarpenterContent;
    public GameObject objCarpenterModel;

    private List<StoreInfoBean> mCarpenterListData;

    public new void Start()
    {
        base.Start();
        if (rgCerpenterType != null)
            rgCerpenterType.SetCallBack(this);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        rgCerpenterType.SetPosition(0, false);
        
        uiGameManager.storeInfoManager.SetCallBack(this);
        uiGameManager.storeInfoManager.GetStoreInfoForCarpenter();
    }

    public void InitDataByType(int type)
    {
        List<StoreInfoBean> listData = GetCerpenterListDataByMark(type);
        CreateCarpenterData(listData, type);
    }

    /// <summary>
    ///  根据备注类型获取数据
    /// </summary>
    /// <param name="mark"></param>
    /// <returns></returns>
    public List<StoreInfoBean> GetCerpenterListDataByMark(int type)
    {
        List<StoreInfoBean> listData = new List<StoreInfoBean>();
        if (mCarpenterListData == null)
            return listData;
        for (int i = 0; i < mCarpenterListData.Count; i++)
        {
            StoreInfoBean itemData = mCarpenterListData[i];
            if (itemData.mark_type.Equals(type))
            {
                listData.Add(itemData);
            }
        }
        return listData;
    }

    /// <summary>
    /// 创建商品列表
    /// </summary>
    /// <param name="listData"></param>
    public void CreateCarpenterData(List<StoreInfoBean> listData, int type)
    {
        CptUtil.RemoveChildsByActive(objCarpenterContent.transform);
        if (listData == null || objCarpenterContent == null)
            return;
        for (int i = 0; i < listData.Count; i++)
        {
            StoreInfoBean itemData = listData[i];
            if (itemData.mark_type==0)
            {
                if (uiGameManager.gameDataManager.gameData.GetInnBuildData().buildLevel+1!= int.Parse(itemData.mark))
                {
                    continue;
                }
            }
            GameObject itemObj = Instantiate(objCarpenterContent, objCarpenterModel);
            ItemTownCerpenterCpt cerpenterCpt = itemObj.GetComponent<ItemTownCerpenterCpt>();
            cerpenterCpt.SetData(itemData);
            itemObj.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBack).SetDelay(i * 0.05f).From();
        }
    }

    #region 获取商店物品回调
    public void GetStoreInfoSuccess(StoreTypeEnum type, List<StoreInfoBean> listData)
    {
        mCarpenterListData = listData;
        InitDataByType(0);
    }
    #endregion

    #region 类型选择回调
    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        InitDataByType(position);
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
    #endregion
}