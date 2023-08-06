using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIGameBuildCourtyard : BaseUIComponent
{
    protected List<ItemBean> listSeedData = new List<ItemBean>();

    public override void Awake()
    {
        base.Awake();
        ui_ItemGameBuildCourtyard.ShowObj(false);
        ui_Content.AddCellListener(OnCellForItem);
    }

    public override void OpenUI()
    {
        base.OpenUI();

        //停止时间
        GameTimeHandler.Instance.SetTimeStatus(true);
        GameControlHandler.Instance.StartControl<ControlForBuildCourtyardCpt>(GameControlHandler.ControlEnum.BuildCourtyard);
        InitData();
    }

    public override void CloseUI()
    {
        base.CloseUI();
        //删除当前选中
        GameControlHandler.Instance.manager.GetControl<ControlForBuildCourtyardCpt>(GameControlHandler.ControlEnum.BuildCourtyard).ClearBuildItem();
        //时间添加1小时
        GameTimeHandler.Instance.AddHour(1);
        //添加研究经验
        GameDataHandler.Instance.AddTimeProcess(60);
        //继续时间
        GameTimeHandler.Instance.SetTimeStatus(false);
        //设置角色到门口
        //SceneCourtyardManager sceneCourtyardManager = GameScenesHandler.Instance.manager.GetSceneManager<SceneCourtyardManager>();
        //Vector3 startPosition = sceneCourtyardManager.Instance.GetRandomEntrancePosition();
        //恢复控制器
        var baseControl = GameControlHandler.Instance.StartControl<BaseControl>(GameControlHandler.ControlEnum.Normal);
        //baseControl.SetFollowPosition(startPosition + new Vector3(0, -2, 0));
    }

    public override void RefreshUI(bool isOpenInit = false)
    {
        base.RefreshUI(isOpenInit);
        if (isOpenInit == false)
        {
            ui_Content.RefreshAllCells();
        }
    }

    public void InitData()
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        listSeedData = gameData.GetItemsByType(GeneralEnum.Seed);
        ui_Content.SetCellCount(listSeedData.Count);

        if (listSeedData.Count == 0)
        {
            ui_Null.ShowObj(true);
        }
        else
        {
            ui_Null.ShowObj(false);
        }
    }

    public void OnCellForItem(ScrollGridCell itemCell)
    {
        var itemData = listSeedData[itemCell.index];
        UIItemGameBuildCourtyard itemCpt = itemCell.GetComponent<UIItemGameBuildCourtyard>();
        itemCpt.SetData(itemData);
    }

    public override void OnClickForButton(Button viewButton)
    {
        base.OnClickForButton(viewButton);
        if (viewButton == ui_BTBack)
        {
            OnClickForBack();
        }
        else if (viewButton == ui_BTDismantle)
        {
            OnClickForDismantleMode();
        }
    }
    /// <summary>
    /// 拆除模式
    /// </summary>
    public void OnClickForDismantleMode()
    {
        GameControlHandler.Instance.manager.GetControl<ControlForBuildCourtyardCpt>(GameControlHandler.ControlEnum.BuildCourtyard).SetDismantleMode();
    }

    public void OnClickForBack()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForBack);
        //打开主UI
        UIHandler.Instance.OpenUIAndCloseOther<UIGameMain>();
    }
}