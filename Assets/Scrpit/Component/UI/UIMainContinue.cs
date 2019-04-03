using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIMainContinue : BaseUIComponent
{
    //返回按钮
    public Button btBack;
    public Text tvBack;
    public Text tvNull;

    public MainScenesManager scenesManager;

    public GameObject listGameDataObj;//列表
    public GameObject itemGameDataObj;//模型

    private void Start()
    {
        if (btBack != null)
            btBack.onClick.AddListener(OpenStartUI);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        if (scenesManager == null)
            return;
        List<GameDataSimpleBean> listGameData =scenesManager.listGameDataSimple;
    }

    /// <summary>
    /// 创建列表数据
    /// </summary>
    /// <param name="listGameData"></param>
    public void CreateListItem(List<GameDataSimpleBean> listGameData)
    {
        CptUtil.RemoveChildsByActive(listGameDataObj.transform);
        if ( CheckUtil.ListIsNull(listGameData))
        {
            tvNull.gameObject.SetActive(true);
            return;
        }
        tvNull.gameObject.SetActive(false);
        for(int i = 0; i < listGameData.Count; i++)
        {
            GameObject itemGameObj= Instantiate(itemGameDataObj, itemGameDataObj.transform);
            itemGameObj.SetActive(true);
            itemGameObj.transform.SetParent(listGameDataObj.transform);
            ItemGameDataCpt itemGameDataCpt= itemGameObj.GetComponent<ItemGameDataCpt>();
            itemGameDataCpt.SetData(listGameData[i]);
        }
    }

    /// <summary>
    /// 开启开始菜单
    /// </summary>
    public void OpenStartUI()
    {
        uiManager.OpenUIAndCloseOtherByName("Start");
    }
}