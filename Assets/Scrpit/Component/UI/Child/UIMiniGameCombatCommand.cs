using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;

public class UIMiniGameCombatCommand :   BaseUIChildComponent<UIMiniGameCombat>
{
    public Button btCommandFight;
    public Button btCommandSkill;
    public Button btCommandItems;

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

    /// <summary>
    /// 指令-战斗
    /// </summary>
    public void CommandFight()
    {
        uiComponent.miniGameData.SetRoundActionCommand(MiniGameCombatCommand.Fight);
        uiComponent.OpenSelectCharacter(1, 2);
    }

    /// <summary>
    /// 指令 技能
    /// </summary>
    public void CommandSkill()
    {
        uiComponent.miniGameData.SetRoundActionCommand(MiniGameCombatCommand.Skill);
        ToastManager toastManager = uiComponent.GetUIManager<UIGameManager>().toastManager;
        toastManager.ToastHint("开发中");
    }

    /// <summary>
    /// 指令物品
    /// </summary>
    public void CommandItems()
    {
        uiComponent.miniGameData.SetRoundActionCommand(MiniGameCombatCommand.Items);
        ToastManager toastManager = uiComponent.GetUIManager<UIGameManager>().toastManager;
        toastManager.ToastHint("开发中");
    }
}