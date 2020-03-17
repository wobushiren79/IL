using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class UIMiniGameCombatCommand :   BaseUIChildComponent<UIMiniGameCombat>,DialogView.IDialogCallBack
{
    public Button btCommandFight;
    public Button btCommandSkill;
    public Button btCommandItems;

    protected ICallBack callBack;

    private void Start()
    {
        if (btCommandFight)
            btCommandFight.onClick.AddListener(CommandFight);
        if (btCommandSkill)
            btCommandSkill.onClick.AddListener(CommandSkill);
        if (btCommandItems)
            btCommandItems.onClick.AddListener(CommandItems);
    }

    public override void Open()
    {
        base.Open();
        uiComponent.miniGameData.SetRoundActionCommand(MiniGameCombatCommand.None);
        transform.localScale = new Vector3(1, 1, 1);
        transform.DOScale(new Vector3(0f, 0f, 0f), 0.5f).From().SetEase(Ease.OutBack).SetDelay(0.2f);
    }

    public void SetCallBack(ICallBack callBack)
    {
        this.callBack = callBack;
    }

    /// <summary>
    /// 指令-战斗
    /// </summary>
    public void CommandFight()
    {
        uiComponent.uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        uiComponent.miniGameData.SetRoundActionCommand(MiniGameCombatCommand.Fight);
        uiComponent.OpenSelectCharacter(1, 2);
    }

    /// <summary>
    /// 指令 技能
    /// </summary>
    public void CommandSkill()
    {
        uiComponent.uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        uiComponent.miniGameData.SetRoundActionCommand(MiniGameCombatCommand.Skill);
        ToastManager toastManager = uiComponent.GetUIManager<UIGameManager>().toastManager;
        toastManager.ToastHint("开发中");
    }

    /// <summary>
    /// 指令物品
    /// </summary>
    public void CommandItems()
    {
        uiComponent.uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        uiComponent.miniGameData.SetRoundActionCommand(MiniGameCombatCommand.Items);

        DialogManager dialogManager= uiComponent.uiGameManager.dialogManager;
        DialogBean dialogData = new DialogBean();
        PickForItemsDialogView pickForItemsDialog=(PickForItemsDialogView) dialogManager.CreateDialog( DialogEnum.PickForItems,this, dialogData);
        pickForItemsDialog.SetData(new List<GeneralEnum>() { GeneralEnum.Medicine }, PopupItemsSelection.SelectionTypeEnum.Use);
    }

    #region 弹窗回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        if(dialogView as PickForItemsDialogView)
        {
            PickForItemsDialogView pickForItemsDialog = (PickForItemsDialogView)dialogView;
            pickForItemsDialog.GetSelectedItems(out ItemsInfoBean  itemsInfo,out ItemBean itemData);
            //设置使用的物品
            uiComponent.miniGameData.SetRoundActionItemsId(itemsInfo.id);

            if (callBack != null)
                callBack.PickItemsComplete(itemsInfo);
        }
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {
        uiComponent.OpenCombatCommand();
    }
    #endregion


    public interface ICallBack
    {
        void PickItemsComplete(ItemsInfoBean itemsInfo);

        void PickSkillComplete(long skillId);
    }
}