using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using System;

public partial class UIMainContinue : BaseUIComponent
{
    public override void Awake()
    {
        base.Awake();
        ui_ItemGameData.ShowObj(false);
    }

    public override void OnClickForButton(Button viewButton)
    {
        if (viewButton == ui_BTBack)
        {
            OpenStartUI();
        }
    }

    public override void OpenUI()
    {
        base.OpenUI();
        GameDataHandler.Instance.manager.GetSimpleGameDataList(SetSimpleGameData);
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
        ui_GameDataContent.transform.DestroyAllChild(true);
        if (listGameData.IsNull())
        {
            ui_Null.gameObject.SetActive(true);
            return;
        }
        ui_Null.gameObject.SetActive(false);
        for (int i = 0; i < listGameData.Count; i++)
        {
            GameDataSimpleBean gameDataSimple = listGameData[i];
            if (gameDataSimple == null)
            {
                continue;
            }
            GameObject itemGameObj = Instantiate(ui_GameDataContent.gameObject, ui_ItemGameData.gameObject);
            itemGameObj.SetActive(true);
            ItemGameDataCpt itemGameDataCpt = itemGameObj.GetComponent<ItemGameDataCpt>();
            itemGameDataCpt.SetData(gameDataSimple);
        }
    }

    /// <summary>
    /// 开启开始菜单
    /// </summary>
    public void OpenStartUI()
    {
        //按键音效
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForBack);
        UIHandler.Instance.OpenUIAndCloseOther<UIMainStart>();
    }

    #region  数据列表回调
    public void SetSimpleGameData(List<GameDataSimpleBean> listData)
    {
        CreateListItem(listData);
    }
    #endregion
}