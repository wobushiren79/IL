using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

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
        if (uiGameManager.gameDataManager == null)
            return;
        uiGameManager.gameDataManager.SetSimpleGameDataCallBack(this);
        uiGameManager.gameDataManager.GetSimpleGameDataList();
        AnimForInit();
    }

    /// <summary>
    /// 初始化动画
    /// </summary>
    public void AnimForInit()
    {
        transform.DOScaleX(0, 0.5f).From().SetEase(Ease.OutBack);
    }

    /// <summary>
    /// 创建列表数据
    /// </summary>
    /// <param name="listGameData"></param>
    public void CreateListItem(List<GameDataSimpleBean> listGameData)
    {
        CptUtil.RemoveChildsByActive(objGameDataContent.transform);
        if (CheckUtil.ListIsNull(listGameData))
        {
            tvNull.gameObject.SetActive(true);
            return;
        }
        tvNull.gameObject.SetActive(false);
        for (int i = 0; i < listGameData.Count; i++)
        {
            GameObject itemGameObj = Instantiate(objGameDataModel, objGameDataContent.transform);
            itemGameObj.SetActive(true);
            ItemGameDataCpt itemGameDataCpt = itemGameObj.GetComponent<ItemGameDataCpt>();
            itemGameDataCpt.SetData(listGameData[i]);
        }
    }

    /// <summary>
    /// 开启开始菜单
    /// </summary>
    public void OpenStartUI()
    {
        //按键音效
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForBack);

        uiManager.OpenUIAndCloseOtherByName(EnumUtil.GetEnumName(UIEnum.MainStart));
    }

    #region  数据列表回调
    public void GetSimpleGameDataSuccess(List<GameDataSimpleBean> listData)
    {
        CreateListItem(listData);
    }
    #endregion
}