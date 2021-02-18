using UnityEngine;
using UnityEditor;

public class ControlForMiniGameCombatCpt : BaseControl
{

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
        UIMiniGameCombat uiMiniGameCombat = UIHandler.Instance.manager.GetUI<UIMiniGameCombat>(UIEnum.MiniGameCombat);
        if (MiniGameHandler.Instance.handlerForCombat.miniGameData.GetCombatStatus() != MiniGameCombatBean.MiniGameCombatStatusEnum.OurRound
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
        if (Input.GetButtonDown(InputInfo.Interactive_E)|| Input.GetButtonDown(InputInfo.Confirm))
        {
            uiMiniGameCombat.uiForSelectCharacter.ConfirmSelect();
        }
        if (Input.GetButtonDown(InputInfo.Cancel))
        {
            uiMiniGameCombat.OpenCombatCommand();
            //开启选中特效
            MiniGameHandler.Instance.handlerForCombat.SelectCharacter(MiniGameHandler.Instance.handlerForCombat.miniGameData.GetRoundActionCharacter());
        }
    }
}