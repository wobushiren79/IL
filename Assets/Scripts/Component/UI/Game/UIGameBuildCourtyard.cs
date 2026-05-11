using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public partial class UIGameBuildCourtyard : BaseUIComponent,IRadioButtonCallBack
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
        InitData();
    }

    public void InitData()
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        InnCourtyardBean innCourtyardData = gameData.GetInnCourtyardData();
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

        //自动续种
        if (ui_ItemEventAutoSeedCheckBox_1 != null)
        {
            ui_ItemEventAutoSeedCheckBox_1.SetCallBack(this);


            if (innCourtyardData.isAutoSeed)
            {
                ui_ItemEventAutoSeedCheckBox_1.ChangeStates(true);
            }
            else
            {
                ui_ItemEventAutoSeedCheckBox_1.ChangeStates(false);
            }
        }
        //初始化选人
        if (innCourtyardData.managerId.IsNull())
        {
            ui_CharacterUI.ShowObj(false);
            ui_CharacterName.ShowObj(false);
        }
        else
        {
            ui_CharacterUI.ShowObj(true);
            ui_CharacterName.ShowObj(true);
            var characterData = gameData.GetCharacterDataById(innCourtyardData.managerId);
            ui_CharacterUI.SetCharacterData(characterData.body, characterData.equips);

            ui_CharacterName.text = characterData.baseInfo.name;
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
        else if (viewButton == ui_ManagerChangeBtn)
        {
            OnClickForChangeMananger();
        }
    }

    /// <summary>
    /// 拆除模式
    /// </summary>
    public void OnClickForDismantleMode()
    {
        GameControlHandler.Instance.manager.GetControl<ControlForBuildCourtyardCpt>(GameControlHandler.ControlEnum.BuildCourtyard).SetDismantleMode();
    }

    /// <summary>
    /// 点击更换管理员
    /// </summary>
    public void OnClickForChangeMananger()
    {
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        InnCourtyardBean innCourtyardData = gameData.GetInnCourtyardData();
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        DialogBean dialogData = new DialogBean();
        dialogData.dialogPosition = 0;
        dialogData.dialogType = DialogEnum.PickForCharacter;
        dialogData.actionSubmit = (view,data)=> 
        {
            //还原原来的
            if (!innCourtyardData.managerId.IsNull())
            {
                var oldManager = gameData.GetCharacterDataById(innCourtyardData.managerId);
                oldManager.baseInfo.SetWorkerStatus(WorkerStatusEnum.Rest);
            }

            PickForCharacterDialogView pickForCharacterDialogView = view as PickForCharacterDialogView;
            List<CharacterBean> listMembers = pickForCharacterDialogView.GetPickCharacter();
            if (listMembers.IsNull())
            {
                innCourtyardData.managerId = null;
            }
            else 
            {
                //派遣
                foreach (CharacterBean itemCharacter in listMembers)
                {
                    innCourtyardData.managerId = itemCharacter.baseInfo.characterId;
                    itemCharacter.baseInfo.SetWorkerStatus(WorkerStatusEnum.CourtyardManager);
                }
            }
            RefreshUI();
        };
        PickForCharacterDialogView pickForCharacterDialog = UIHandler.Instance.ShowDialog<PickForCharacterDialogView>(dialogData);
        //排除不能参加的人
        List<string> listExpel = new List<string>();
        List<CharacterBean> listCharacter = gameData.GetAllCharacterData();
        for (int i = 0; i < listCharacter.Count; i++)
        {
            CharacterBean itemCharacter = listCharacter[i];
            if (itemCharacter == gameData.userCharacter
              || (itemCharacter.baseInfo.GetWorkerStatus() != WorkerStatusEnum.Rest && itemCharacter.baseInfo.GetWorkerStatus() != WorkerStatusEnum.Work && itemCharacter.baseInfo.GetWorkerStatus() != WorkerStatusEnum.CourtyardManager))
            {
                listExpel.Add(itemCharacter.baseInfo.characterId);
            }
        }

        pickForCharacterDialog.SetExpelCharacter(listExpel);
        pickForCharacterDialog.SetPickCharacterMax(1);
        pickForCharacterDialog.SetIsNullSelect(true);
    }

    public void OnClickForBack()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForBack);
        //打开主UI
        UIHandler.Instance.OpenUIAndCloseOther<UIGameMain>();
    }

    #region 回调
    public void RadioButtonSelected(RadioButtonView view, bool isSelect)
    {
        if (view == ui_ItemEventAutoSeedCheckBox_1)
        {
            GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
            InnCourtyardBean innCourtyardData = gameData.GetInnCourtyardData();
            innCourtyardData.isAutoSeed = isSelect;
        }
    }
    #endregion
}