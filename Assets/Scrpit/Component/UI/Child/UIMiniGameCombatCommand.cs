using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class UIMiniGameCombatCommand : BaseUIChildComponent<UIMiniGameCombat>, DialogView.IDialogCallBack
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
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        uiComponent.miniGameData.SetRoundActionCommand(MiniGameCombatCommand.Fight);
        uiComponent.OpenSelectCharacter(1, 2);
    }

    /// <summary>
    /// 指令 技能
    /// </summary>
    public void CommandSkill()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        uiComponent.miniGameData.SetRoundActionCommand(MiniGameCombatCommand.Skill);
        NpcAIMiniGameCombatCpt npcCpt = uiComponent.miniGameData.GetRoundActionCharacter();
        SkillInfoHandler skillInfoHandler = ((UIGameManager)uiComponent.uiManager).skillInfoHandler;
        //获取角色拥有的技能
        List<long> listSkill = npcCpt.characterMiniGameData.characterData.attributes.listSkills;
        List<SkillInfoBean> listSkillInfo=  skillInfoHandler.GetSkillByIds(listSkill);

        DialogManager dialogManager = ((UIGameManager)uiComponent.uiManager).dialogManager;
        DialogBean dialogData = new DialogBean();
        PickForSkillDialogView pickForSkillDialog = DialogHandler.Instance.CreateDialog<PickForSkillDialogView>(DialogEnum.PickForSkill, this, dialogData);
        Dictionary<long, int> listUsedSkill = npcCpt.characterMiniGameData.listUsedSkill;
        pickForSkillDialog.SetData(listSkillInfo, listUsedSkill);
    }

    /// <summary>
    /// 指令物品
    /// </summary>
    public void CommandItems()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        uiComponent.miniGameData.SetRoundActionCommand(MiniGameCombatCommand.Items);

        DialogManager dialogManager = ((UIGameManager)uiComponent.uiManager).dialogManager;
        DialogBean dialogData = new DialogBean();
        PickForItemsDialogView pickForItemsDialog = DialogHandler.Instance.CreateDialog<PickForItemsDialogView>(DialogEnum.PickForItems, this, dialogData);
        pickForItemsDialog.SetData(new List<GeneralEnum>() { GeneralEnum.Medicine }, PopupItemsSelection.SelectionTypeEnum.Use);
    }

    /// <summary>
    /// 指令物品
    /// </summary>
    public void CommandPass()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        uiComponent.miniGameData.SetRoundActionCommand(MiniGameCombatCommand.Pass);
        if (callBack != null)
            callBack.PassComplete();
    }

    /// <summary>
    /// 指令逃跑
    /// </summary>
    public void CommandEscape()
    {
        MiniGameCombatHandler miniGameCombatHandler = ((UIGameManager)uiComponent.uiManager).miniGameCombatHandler;
        Close();
        miniGameCombatHandler.EndGame(MiniGameResultEnum.Escape);
    }

    #region 弹窗回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        if (dialogView as PickForItemsDialogView)
        {
            PickForItemsDialogView pickForItemsDialog = dialogView as PickForItemsDialogView;
            pickForItemsDialog.GetSelectedItems(out ItemsInfoBean itemsInfo, out ItemBean itemData);
            //设置使用的物品
            uiComponent.miniGameData.SetRoundActionItemsId(itemsInfo.id);

            if (callBack != null)
                callBack.PickItemsComplete(itemsInfo);
        }
        else if (dialogView as PickForSkillDialogView)
        {
            PickForSkillDialogView pickForSkillDialog = dialogView as PickForSkillDialogView;
            pickForSkillDialog.GetSelectedSkill(out SkillInfoBean skillInfo);
            //设置使用的技能
            uiComponent.miniGameData.SetRoundActionSkill(skillInfo);

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