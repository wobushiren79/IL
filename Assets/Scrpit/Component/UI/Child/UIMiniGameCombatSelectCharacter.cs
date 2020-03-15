using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;

public class UIMiniGameCombatSelectCharacter : BaseUIChildComponent<UIMiniGameCombat>
{
    public GameObject objSelectIconContainer;
    public GameObject objSelectIconModel;

    //选中的NPC
    public List<NpcAIMiniGameCombatCpt> listSelectNpc = new List<NpcAIMiniGameCombatCpt>();

    protected MiniGameCombatBuilder miniGameCombatBuilder;
    protected MiniGameCombatHandler miniGameCombatHandler;

    protected NpcAIMiniGameCombatCpt currentSelectNpc = null;

    protected int selectType;
    protected int selectNumber;

    private void Awake()
    {
        miniGameCombatBuilder = FindInChildren<MiniGameCombatBuilder>(ImportantTypeEnum.MiniGameBuilder);
        miniGameCombatHandler = Find<MiniGameCombatHandler>(ImportantTypeEnum.MiniGameHandler);
    }

    public override void Close()
    {
        base.Close();
        listSelectNpc.Clear();
        currentSelectNpc = null;
    }

    /// <summary>
    /// 选择人物
    /// </summary>
    /// <param name="selectNumber">0 为全选</param>
    /// <param name="selectNumber">1 为友方 2 为敌方</param>
    public void SetData(int selectNumber, int selectType)
    {
        this.selectNumber = selectNumber;
        this.selectType = selectType;
        ChangeCharacter(1);
    }

    /// <summary>
    /// 改变选择的角色
    /// </summary>
    /// <param name="selectType"></param>
    public void ChangeCharacter(int next)
    {
        CptUtil.RemoveChildsByActive(objSelectIconContainer);

        List<NpcAIMiniGameCombatCpt> listData = new List<NpcAIMiniGameCombatCpt>();

        //友方
        if (selectType == 1)
        {
            listData = miniGameCombatBuilder.GetUserCharacter();
        }
        //敌方
        else if (selectType == 2)
        {
            listData = miniGameCombatBuilder.GetEnemyCharacter();
        }
        //通用
        else
        {
            listData = miniGameCombatBuilder.GetAllCharacter();
        }
        //如果还没有选择角色 则选择第一个
        if (currentSelectNpc == null)
        {
            currentSelectNpc = listData[0];
        }
        //如果有选择角色则改变为下一个
        else
        {
            int changeNumber = 0;
            for (int i = 0; i < listData.Count; i++)
            {
                NpcAIMiniGameCombatCpt itemCharacter = listData[i];
                if (itemCharacter == currentSelectNpc)
                {
                    changeNumber = (i + next);
                    if (changeNumber >= listData.Count)
                    {
                        changeNumber = 0;
                    }
                    else if (changeNumber < 0)
                    {
                        changeNumber = listData.Count - 1;
                    }
                    break;
                }
            }
            currentSelectNpc = listData[changeNumber];
        }
        miniGameCombatHandler.SetCameraPosition(currentSelectNpc.transform.position);

        Vector3 ptScreen = Camera.main.WorldToViewportPoint(currentSelectNpc.transform.position);
        Instantiate(objSelectIconContainer, objSelectIconModel, new Vector3(ptScreen.x, ptScreen.y + 60));
    }

    public interface ICallBack
    {
        void SelectComplete(List<NpcAIMiniGameCombatCpt> listData);
    }
}