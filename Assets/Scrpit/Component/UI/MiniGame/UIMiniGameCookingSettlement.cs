using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIMiniGameCookingSettlement : BaseUIComponent
{
    public Button btClose;

    public GameObject objItemSettlementContainer;
    public GameObject objItemSettlementModel;

    private ICallBack mCallBack;

    private void Start()
    {
        if (btClose != null)
            btClose.onClick.AddListener(CloseUI);
    }

    public void SetCallBack(ICallBack callBack)
    {
        mCallBack = callBack;
    }

    public void SetData(List<NpcAIMiniGameCookingCpt> listNpc)
    {
        CreateSettlementData(listNpc);
    }

    public override void CloseUI()
    {
        base.CloseUI();
        if (mCallBack != null)
            mCallBack.UIMiniGameCookingSettlementClose();
    }

    /// <summary>
    /// 创建结算数据
    /// </summary>
    /// <param name="listNpc">选手列表</param>
    public void CreateSettlementData(List<NpcAIMiniGameCookingCpt> listNpc)
    {
        CptUtil.RemoveChildsByActive(objItemSettlementContainer);
        if (listNpc == null)
            return;
        for (int i = 0; i < listNpc.Count; i++)
        {
            NpcAIMiniGameCookingCpt itemNpc = listNpc[i];
            GameObject objItem=  Instantiate(objItemSettlementContainer, objItemSettlementModel);
            ItemMiniGameCookingSettlementCpt itemCpt= objItem.GetComponent<ItemMiniGameCookingSettlementCpt>();
            itemCpt.SetData(itemNpc,i+1);
        }
    }

    public interface ICallBack
    {
        void UIMiniGameCookingSettlementClose();
    }

}