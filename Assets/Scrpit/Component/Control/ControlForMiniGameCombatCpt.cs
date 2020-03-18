using UnityEngine;
using UnityEditor;

public class ControlForMiniGameCombatCpt : BaseControl
{
    protected UIGameManager uiGameManager;
    protected MiniGameCombatHandler gameCombatHandler;
    protected UIMiniGameCombat uiMiniGameCombat;
    private void Awake()
    {
        gameCombatHandler = Find<MiniGameCombatHandler>(ImportantTypeEnum.MiniGameHandler);
        uiGameManager = Find<UIGameManager>(ImportantTypeEnum.GameUI);
        uiMiniGameCombat = (UIMiniGameCombat)uiGameManager.GetUIByName(EnumUtil.GetEnumName(UIEnum.MiniGameCombat));
    }

    /// <summary>
    /// 选中一个角色
    /// </summary>
    /// <param name="npcAI"></param>
    public void ChangeCharacter(NpcAIMiniGameCombatCpt currentNpc)
    {

    }

    private void Update()
    {
        HandleForSelectCharacter();
    }

    /// <summary>
    /// 角色选择处理
    /// </summary>
    public void HandleForSelectCharacter()
    {
        //如果没有在玩家回合 切没有在选择人物中则不处理
        if (gameCombatHandler.miniGameData.GetCombatStatus() != MiniGameCombatBean.MiniGameCombatStatusEnum.OurRound
            || !uiMiniGameCombat.isSelecting)
        {
            return;
        }
        if (Input.GetButtonDown(InputInfo.Direction_Up)
            || Input.GetButtonDown(InputInfo.Direction_Left)
  )
        {
            uiMiniGameCombat.uiForSelectCharacter.ChangeCharacter(-1);
        }
        if (Input.GetButtonDown(InputInfo.Direction_Right)
            || Input.GetButtonDown(InputInfo.Direction_Down))
        {
            uiMiniGameCombat.uiForSelectCharacter.ChangeCharacter(1);
        }
        if (Input.GetButtonDown(InputInfo.Interactive_E))
        {
            uiMiniGameCombat.uiForSelectCharacter.ConfirmSelect();
        }
        if (Input.GetButtonDown(InputInfo.Cancel))
        {
            uiMiniGameCombat.OpenCombatCommand();
            //开启选中特效
            gameCombatHandler.SelectCharacter(gameCombatHandler.miniGameData.GetRoundActionCharacter());
        }
    }
}