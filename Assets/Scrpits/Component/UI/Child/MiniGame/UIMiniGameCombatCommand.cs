using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using System;

public class UIMiniGameCombatCommand : BaseUIView, DialogView.IDialogCallBack
{
    public Button btCommandFight;
    public Button btCommandSkill;
    public Button btCommandItems;
    public Button btCommandPass;
    public Button btCommandEscape;
    protected ICallBack callBack;

    private void Start()
    {
        if (btCommandFight)
            btCommandFight.onClick.AddListener(CommandFight);
        if (btCommandSkill)
            btCommandSkill.onClick.AddListener(CommandSkill);
        if (btCommandItems)
            btCommandItems.onClick.AddListener(CommandItems);
        if (btCommandPass)
            btCommandPass.onClick.AddListener(CommandPass);
        if (btCommandEscape)
            btCommandEscape.onClick.AddListener(CommandEscape);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        UIMiniGameCombat uiMiniGameCombat = UIHandler.Instance.GetUI<UIMiniGameCombat>();
        uiMiniGameCombat.miniGameData.SetRoundActionCommand(MiniGameCombatCommand.None);
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
        UIMiniGameCombat uiMiniGameCombat = UIHandler.Instance.GetUI<UIMiniGameCombat>();
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        uiMiniGameCombat.miniGameData.SetRoundActionCommand(MiniGameCombatCommand.Fight);
        uiMiniGameCombat.OpenSelectCharacter(1, 2);
    }

    /// <summary>
    /// 指令 技能
    /// </summary>
    public void CommandSkill()
    {
        UIMiniGameCombat uiMiniGameCombat = UIHandler.Instance.GetUI<UIMiniGameCombat>();
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        uiMiniGameCombat.miniGameData.SetRoundActionCommand(MiniGameCombatCommand.Skill);
        NpcAIMiniGameCombatCpt npcCpt = uiMiniGameCombat.miniGameData.GetRoundActionCharacter();
        //获取角色拥有的技能
        List<long> listSkill = npcCpt.characterMiniGameData.characterData.attributes.listSkills;

        DialogBean dialogData = new DialogBean();
        dialogData.dialogType = DialogEnum.PickForSkill;
        dialogData.callBack = this;
        PickForSkillDialogView pickForSkillDialog = UIHandler.Instance.ShowDialog<PickForSkillDialogView>(dialogData);
        Dictionary<long, int> listUsedSkill = npcCpt.characterMiniGameData.listUsedSkill;
        pickForSkillDialog.SetData(listSkill, listUsedSkill);
    }

    /// <summary>
    /// 指令物品
    /// </summary>
    public void CommandItems()
    {
        UIMiniGameCombat uiMiniGameCombat = UIHandler.Instance.GetUI<UIMiniGameCombat>();
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        uiMiniGameCombat.miniGameData.SetRoundActionCommand(MiniGameCombatCommand.Items);

        DialogBean dialogData = new DialogBean();
        dialogData.dialogType = DialogEnum.PickForItems;
        dialogData.callBack = this;
        PickForItemsDialogView pickForItemsDialog = UIHandler.Instance.ShowDialog<PickForItemsDialogView>(dialogData);
        pickForItemsDialog.SetData(new List<GeneralEnum>() { GeneralEnum.Medicine }, ItemsSelectionDialogView.SelectionTypeEnum.Use);
    }

    /// <summary>
    /// 指令物品
    /// </summary>
    public void CommandPass()
    {
        UIMiniGameCombat uiMiniGameCombat = UIHandler.Instance.GetUI<UIMiniGameCombat>();
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        uiMiniGameCombat.miniGameData.SetRoundActionCommand(MiniGameCombatCommand.Pass);
        if (callBack != null)
            callBack.PassComplete();
    }

    /// <summary>
    /// 指令逃跑
    /// </summary>
    public void CommandEscape()
    {
        CloseUI();
        MiniGameHandler.Instance.handlerForCombat.EndGame(MiniGameResultEnum.Escape);
    }

    #region 弹窗回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        UIMiniGameCombat uiMiniGameCombat = UIHandler.Instance.GetUI<UIMiniGameCombat>();
        if (dialogView as PickForItemsDialogView)
        {
            PickForItemsDialogView pickForItemsDialog = dialogView as PickForItemsDialogView;
            pickForItemsDialog.GetSelectedItems(out ItemsInfoBean itemsInfo, out ItemBean itemData);
            //设置使用的物品
            uiMiniGameCombat.miniGameData.SetRoundActionItemsId(itemsInfo.id);

            if (callBack != null)
                callBack.PickItemsComplete(itemsInfo);
        }
        else if (dialogView as PickForSkillDialogView)
        {
            PickForSkillDialogView pickForSkillDialog = dialogView as PickForSkillDialogView;
            pickForSkillDialog.GetSelectedSkill(out SkillInfoBean skillInfo);
            //设置使用的技能
            uiMiniGameCombat.miniGameData.SetRoundActionSkill(skillInfo);

            if (callBack != null)
                callBack.PickSkillComplete(skillInfo);
        }
       
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {
        //uiComponent.OpenCombatCommand();
    }
    #endregion

    public interface ICallBack
    {
        void PickItemsComplete(ItemsInfoBean itemsInfo);

        void PickSkillComplete(SkillInfoBean skillInfo);

        void PassComplete();
    }
}