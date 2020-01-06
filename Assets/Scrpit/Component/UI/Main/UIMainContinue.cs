using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIMainContinue : BaseUIComponent, GameDataManager.IGameDataSimpleCallBack
{
    //返回按钮
    public Button btBack;
    public Text tvBack;
    public Text tvNull;

    public GameObject objGameDataContent;//列表
    public GameObject objGameDataModel;//模型

    private void Start()
    {
        if (btBack != null)
            btBack.onClick.AddListener(OpenStartUI);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        GameDataManager gameDataManager = GetUIMananger<UIGameManager>().gameDataManager;
        if (gameDataManager == null)
            return;
        gameDataManager.SetSimpleGameDataCallBack(this);
        gameDataManager.GetSimpleGameDataList();
    }

    /// <summary>
    /// 创建列表数据
    /// </summary>
    /// <param name="listGameData"></param>
    public void CreateListItem(List<GameDataSimpleBean> listGameData)
    {
        CptUtil.RemoveChildsByActive(objGameDataContent.transform);
        if ( CheckUtil.ListIsNull(listGameData))
        {
            tvNull.gameObject.SetActive(true);
            return;
        }
        tvNull.gameObject.SetActive(false);
        for(int i = 0; i < listGameData.Count; i++)
        {
            GameObject itemGameObj= Instantiate(objGameDataModel, objGameDataContent.transform);
            itemGameObj.SetActive(true);
            ItemGameDataCpt itemGameDataCpt= itemGameObj.GetComponent<ItemGameDataCpt>();
            itemGameDataCpt.SetData(listGameData[i]);
        }
    }

    /// <summary>
    /// 开启开始菜单
    /// </summary>
    public void OpenStartUI()
    {
        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.MainStart));
    }

    #region  数据列表回调
    public void GetSimpleGameDataSuccess(List<GameDataSimpleBean> listData)
    {
        CreateListItem(listData);
    }
    #endregion
}