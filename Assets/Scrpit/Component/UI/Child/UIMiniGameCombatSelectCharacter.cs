using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;

public class UIMiniGameCombatSelectCharacter : BaseUIChildComponent<UIMiniGameCombat>
{
    public GameObject objSelectIconContainer;
    public GameObject objSelectIconModel;

    //选中的NPC
    public Dictionary<NpcAIMiniGameCombatCpt, GameObject> listSelectNpc = new Dictionary<NpcAIMiniGameCombatCpt, GameObject>();

    public NpcAIMiniGameCombatCpt currentSelectNpc;
    public GameObject currentSelectCursor;

    protected MiniGameCombatBuilder miniGameCombatBuilder;
    protected MiniGameCombatHandler miniGameCombatHandler;
    protected UIGameManager uiGameManager;

    protected int selectType;
    protected int selectNumber;
    protected ICallBack callBack;

    public override void Awake()
    {
        base.Awake();
        miniGameCombatBuilder = FindInChildren<MiniGameCombatBuilder>(ImportantTypeEnum.MiniGameBuilder);
        miniGameCombatHandler = Find<MiniGameCombatHandler>(ImportantTypeEnum.MiniGameHandler);
        uiGameManager = Find<UIGameManager>(ImportantTypeEnum.GameUI);
    }

    private void Update()
    {
        //设置选中角色图标
        if (listSelectNpc != null && listSelectNpc.Count != 0)
        {
            foreach (var itemSelect in listSelectNpc)
            {
                NpcAIMiniGameCombatCpt itemNpc = itemSelect.Key;
                GameObject objCursor = itemSelect.Value;
                GameUtil.WorldPointToUILocalPoint((RectTransform)uiGameManager.transform, itemNpc.transform.position + new Vector3(0, 0.5f), (RectTransform)objCursor.transform);
            }
        }
        //设置指定角色图标
        if(currentSelectCursor!=null&& currentSelectNpc != null)
        {
            GameUtil.WorldPointToUILocalPoint((RectTransform)uiGameManager.transform, currentSelectNpc.transform.position + new Vector3(0, 0.5f), (RectTransform)currentSelectCursor.transform);
        }
    }

    public override void Open()
    {
        base.Open();
        uiComponent.isSelecting = true;
    }

    public override void Close()
    {
        base.Close();
        uiComponent.isSelecting = false;
        listSelectNpc.Clear();
        CptUtil.RemoveChildsByActive(objSelectIconContainer);
        currentSelectNpc = null;
    }

    public void SetCallBack(ICallBack callBack)
    {
        this.callBack = callBack;
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
    /// <param name="isAddCurrent">是否增加上一个角色</param>
    public void ChangeCharacter(int next)
    {
        ((UIGameManager)uiComponent.uiManager).audioHandler.PlaySound(AudioSoundEnum.ChangeSelect);
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
        //如果选择的人数大于=一共的对象，那么默认选择全部
        if(selectNumber >= listData.Count)
        {
            selectNumber = 0;
        }
        //如果还没有选择角色 则选择第一个
        if (currentSelectNpc == null)
        {
            currentSelectNpc = listData[0];
        }
        //如果有选择角色则改变为下一个
        else
        {
            //如果不是全选 则先删除上一个
            if (selectNumber != 0)
            {
                if(currentSelectCursor!=null)
                    Destroy(currentSelectCursor);
            }
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
        //镜头对准选中的角色
        miniGameCombatHandler.SetCameraPosition(currentSelectNpc.transform.position);

        //如果是全选
        if (selectNumber == 0)
        {
            //如果还没有选中所有
            if (listSelectNpc == null || listSelectNpc.Count == 0)
                foreach (NpcAIMiniGameCombatCpt itemNpc in listData)
                {
                    GameObject itemCursor = Instantiate(objSelectIconContainer, objSelectIconModel);
                    listSelectNpc.Add(itemNpc, itemCursor);
                }
        }
        else
        {
            if (!listSelectNpc.ContainsKey(currentSelectNpc))
            {
                currentSelectCursor = Instantiate(objSelectIconContainer, objSelectIconModel);
            }
        }
    }

    /// <summary>
    /// 确认选择
    /// </summary>
    public void ConfirmSelect()
    {
        if (callBack == null)
            return;
        ((UIGameManager)uiComponent.uiManager).audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        List<NpcAIMiniGameCombatCpt> listData = new List<NpcAIMiniGameCombatCpt>();
        foreach (var itemData in listSelectNpc)
        {
            listData.Add(itemData.Key);
        }
        //如果是全选则选择成功
        if (selectNumber == 0)
        {
            callBack.SelectComplete(listData);
        }
        else
        {
            if (!listSelectNpc.ContainsKey(currentSelectNpc))
            {
                listData.Add(currentSelectNpc);
                GameObject itemCursor = Instantiate(objSelectIconContainer, objSelectIconModel);
                listSelectNpc.Add(currentSelectNpc, itemCursor);
            }
            //如果选择人数到达则
            if (selectNumber == listData.Count)
            {
                callBack.SelectComplete(listData);
            }
            //没有则继续选择
            else
            {
                ChangeCharacter(1);
            }
        }
    }


    public interface ICallBack
    {
        void SelectComplete(List<NpcAIMiniGameCombatCpt> listData);
    }
}