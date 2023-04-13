using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using System;

public partial class UIMiniGameCombat : UIBaseMiniGame<MiniGameCombatBean>,
    UIMiniGameCombatSelectCharacter.ICallBack,
    UIMiniGameCombatCommand.ICallBack,
    DialogView.IDialogCallBack,
    PowerTestDialogView.ICallBack,
    IRadioGroupCallBack,
    IRadioButtonCallBack
{
    //角色选择UI
    public UIMiniGameCombatSelectCharacter uiForSelectCharacter;
    //战斗指令
    public UIMiniGameCombatCommand uiForCombatCommand;

    //进度条
    public RectTransform rtfRoundContainer;
    //友方角色信息容器
    public GameObject objOurCharacterContainer;
    //敌方角色信息容器
    public GameObject objEnemyCharacterContainer;
    //角色回合信息容器
    public GameObject objRoundCharacterContainer;

    public GameObject objOurCharacterModel;
    public GameObject objEnemyCharacterModel;
    public GameObject objRoundCharacterModel;
    //会和条结束位置
    public GameObject objRoundEnd;

    public List<ItemMiniGameCombatCharacterInfoCpt> listCharacterInfo = new List<ItemMiniGameCombatCharacterInfoCpt>();
    public List<ItemMiniGameCombatCharacterRoundCpt> listCharacterRound = new List<ItemMiniGameCombatCharacterRoundCpt>();

    public bool isRounding = false;//是否回合进行中
    public bool isSelecting = false;//是否正在选择中

    private void Start()
    {
        if (uiForSelectCharacter != null)
            uiForSelectCharacter.SetCallBack(this);
        if (uiForCombatCommand != null)
            uiForCombatCommand.SetCallBack(this);
        ui_SpeedControl.SetCallBack(this);
        ui_Auto.SetCallBack(this);
    }

    private void Update() 
    {
        //回合条移动处理
        HandleForRoundMove();
    }

    /// <summary>
    /// 刷新UI
    /// </summary>
    public override void RefreshUI(bool isOpenInit = false)
    {
        base.RefreshUI(isOpenInit);
        if (isOpenInit)
            return;
        for (int i = 0; i < listCharacterInfo.Count; i++)
        {
            ItemMiniGameCombatCharacterInfoCpt itemInfo = listCharacterInfo[i];
            itemInfo.RefreshUI();
        }
        for (int i = 0; i < listCharacterRound.Count; i++)
        {
            ItemMiniGameCombatCharacterRoundCpt itemInfo = listCharacterRound[i];
            itemInfo.RefreshUI();
        }

        //设置自动战斗的状态
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        ui_Auto.SetStates(gameData.isAutoForCombat);
        //设置速度
        if (gameData.speedForCombat == 1)
        {
            ui_SpeedControl.SetPosition(0, false);
        }
        else if (gameData.speedForCombat == 2)
        {
            ui_SpeedControl.SetPosition(1, false);
        }
        else if (gameData.speedForCombat == 5)
        {
            ui_SpeedControl.SetPosition(2, false);
        }
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="miniGameCombatData"></param>
    public override void SetData(MiniGameCombatBean gameCombatData)
    {
        base.SetData(gameCombatData);
        ClearAllData();

        //创建Item
        CreateOurCharacterList(gameCombatData.listUserGameData);
        CreateEnemyCharacterList(gameCombatData.listEnemyGameData);
    }

    /// <summary>
    /// 清空所有数据
    /// </summary>
    public void ClearAllData()
    {
        //清空数据
        listCharacterInfo.Clear();
        listCharacterRound.Clear();
        CptUtil.RemoveChildsByActive(objRoundCharacterContainer.transform);
        CptUtil.RemoveChildsByActive(objOurCharacterContainer.transform);
        CptUtil.RemoveChildsByActive(objEnemyCharacterContainer.transform);
    }

    /// <summary>
    /// 创建友方信息列表
    /// </summary>
    /// <param name="listCharacterData"></param>
    public void CreateOurCharacterList(List<MiniGameCharacterBean> listCharacterData)
    {
        if (listCharacterData == null)
            return;
        for (int i = 0; i < listCharacterData.Count; i++)
        {
            MiniGameCharacterBean itemData = listCharacterData[i];
            CreateCharacterItem(i, itemData, objOurCharacterContainer, objOurCharacterModel);
        }
    }

    /// <summary>
    /// 创建敌方信息列表
    /// </summary>
    /// <param name="listCharacterData"></param>
    public void CreateEnemyCharacterList(List<MiniGameCharacterBean> listCharacterData)
    {
        if (listCharacterData == null)
            return;
        for (int i = 0; i < listCharacterData.Count; i++)
        {
            MiniGameCharacterBean itemData = listCharacterData[i];
            CreateCharacterItem(i, itemData, objEnemyCharacterContainer, objEnemyCharacterModel);
        }
    }

    /// <summary>
    /// 创建角色信息
    /// </summary>
    /// <param name="position"></param>
    /// <param name="gameCharacterData"></param>
    /// <param name="objContainer"></param>
    /// <param name="objModel"></param>
    public void CreateCharacterItem(int position, MiniGameCharacterBean gameCharacterData, GameObject objContainer, GameObject objModel)
    {
        //创建角色信息
        GameObject objItem = Instantiate(objContainer, objModel);
        ItemMiniGameCombatCharacterInfoCpt characterInfoCpt = objItem.GetComponent<ItemMiniGameCombatCharacterInfoCpt>();
        characterInfoCpt.SetData(gameCharacterData);
        listCharacterInfo.Add(characterInfoCpt);
        //创建回合条信息
        GameObject objRoundItem = Instantiate(objRoundCharacterContainer, objRoundCharacterModel);
        RectTransform rtfItemRound = objRoundItem.GetComponent<RectTransform>();
        rtfItemRound.anchoredPosition = new Vector2(0, rtfItemRound.anchoredPosition.y);
        ItemMiniGameCombatCharacterRoundCpt characterRoundCpt = objRoundItem.GetComponent<ItemMiniGameCombatCharacterRoundCpt>();
        characterRoundCpt.SetData((MiniGameCharacterForCombatBean)gameCharacterData);
        listCharacterRound.Add(characterRoundCpt);
        //添加动画
        objItem.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f).From().SetDelay(0.1f * position).SetEase(Ease.OutBack);
        objRoundItem.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f).From().SetDelay(0.1f * position).SetEase(Ease.OutBack);
    }

    /// <summary>
    /// 战斗速度改变
    /// </summary>
    /// <param name="timeSpeed"></param>
    public void CombatSpeedChange(float timeSpeed)
    {
        Time.timeScale = timeSpeed;
        GameDataHandler.Instance.manager.gameData.speedForCombat = timeSpeed;
    }

    /// <summary>
    /// 开始回合计时
    /// </summary>
    public void StartRound()
    {
        isRounding = true;
    }

    /// <summary>
    /// 回合条移动处理
    /// </summary>
    public void HandleForRoundMove()
    {
        if (!isRounding)
            return;
        float withRound = rtfRoundContainer.rect.width;
        for (int i = 0; i < listCharacterRound.Count; i++)
        {
            ItemMiniGameCombatCharacterRoundCpt itemCpt = listCharacterRound[i];
            float roundSpeed = itemCpt.speedForMove * Time.deltaTime * 5f;
            //回合条向右移动
            RectTransform itemRtf = (RectTransform)itemCpt.transform;
            itemRtf.anchoredPosition = new Vector2(itemRtf.anchoredPosition.x + roundSpeed, itemRtf.anchoredPosition.y);
            //检测是否到达目标点
            if (itemRtf.anchoredPosition.x >= withRound)
            {
                itemRtf.anchoredPosition = new Vector2(withRound, itemRtf.anchoredPosition.y);
                //设置为选中状态
                itemCpt.SetStatus(true);
                //通知轮到角色回合
                EventHandler.Instance.TriggerEvent(EventsInfo.MiniGameCombat_EventForCharacterRound, itemCpt.gameCharacterData);
                isRounding = false;
                break;
            }
        }

        //排序
        if (listCharacterRound != null)
        {
            listCharacterRound = listCharacterRound.OrderByDescending(i => ((RectTransform)i.transform).anchoredPosition.x).ToList();
            for (int i = 0; i < listCharacterRound.Count; i++)
            {
                RectTransform itemRTF = (RectTransform)listCharacterRound[i].transform;
                itemRTF.SetAsFirstSibling();
            }
        }
    }

    /// <summary>
    /// 移除角色回合行动
    /// </summary>
    public void RemoveCharacterRound(MiniGameCharacterBean miniGameCharacter)
    {
        for (int i = 0; i < listCharacterRound.Count; i++)
        {
            ItemMiniGameCombatCharacterRoundCpt itemRound = listCharacterRound[i];
            if (itemRound.gameCharacterData == miniGameCharacter)
            {
                listCharacterRound.Remove(itemRound);
                Destroy(itemRound.gameObject);
                return;
            }
        }
    }

    /// <summary>
    /// 开启指令UI
    /// </summary>
    public void OpenCombatCommand()
    {
        uiForSelectCharacter.CloseUI();
        uiForCombatCommand.OpenUI();
    }

    /// <summary>
    /// 打开任务选择
    /// </summary>
    /// <param name="selectNumber"></param>
    /// <param name="selectType"></param>
    public void OpenSelectCharacter(int selectNumber, int selectType)
    {
        //如果选择是选择自己
        if (selectType == 0)
        {
            SelectComplete(new List<NpcAIMiniGameCombatCpt>() { miniGameData.GetRoundActionCharacter() });
        }
        else
        {
            uiForCombatCommand.CloseUI();
            uiForSelectCharacter.OpenUI();
            uiForSelectCharacter.SetData(selectNumber, selectType);
        }
    }

    /// <summary>
    /// 打开力度测试
    /// </summary>
    public void OpenPowerTest(MiniGameCharacterBean gameCharacterData)
    {
        uiForCombatCommand.CloseUI();
        uiForSelectCharacter.CloseUI();
        GameConfigBean gameConfig = GameDataHandler.Instance.manager.GetGameConfig();
        if (gameConfig.statusForCombatForPowerTest == 1)
        {
            //获取属性
            gameCharacterData.characterData.GetAttributes(out CharacterAttributesBean characterAttributes);

            DialogBean dialogData = new DialogBean();
            dialogData.dialogType = DialogEnum.PowerTest;
            dialogData.callBack = this;
            PowerTestDialogView powerTestDialog = UIHandler.Instance.ShowDialog<PowerTestDialogView>(dialogData);
            powerTestDialog.SetCallBack(this);
            powerTestDialog.SetData(1.5f, 1);
        }
        else
        {
            PowerTestEnd(0.8f);
        }
    }

    /// <summary>
    /// 关闭所有功能界面
    /// </summary>
    public void CloseAll()
    {
        uiForCombatCommand.CloseUI();
        uiForSelectCharacter.CloseUI();
    }

    /// <summary>
    /// 初始化角色回合
    /// </summary>
    /// <param name="gameCharacterData"></param>
    public void InitCharacterRound(MiniGameCharacterBean gameCharacterData)
    {
        for (int i = 0; i < listCharacterRound.Count; i++)
        {
            ItemMiniGameCombatCharacterRoundCpt itemCpt = listCharacterRound[i];
            if (gameCharacterData == itemCpt.gameCharacterData)
            {
                //恢复状态
                itemCpt.SetStatus(false);
                //重新设置位置
                RectTransform itemRTF = (RectTransform)itemCpt.transform;
                itemRTF.anchoredPosition = Vector2.zero;
                return;
            }
        }
    }

    #region 力度测试回调
    public void PowerTestEnd(float resultsPower)
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        miniGameData.SetRoundActionPowerTest(resultsPower);
        EventHandler.Instance.TriggerEvent(EventsInfo.MiniGameCombat_EventForCommandEnd);
    }
    #endregion


    #region 角色选择回调
    public void SelectComplete(List<NpcAIMiniGameCombatCpt> listData)
    {
        miniGameData.SetRoundTargetCharacter(listData);
        MiniGameCombatCommand miniGameCombatCommand = miniGameData.GetRoundActionCommand();
        if (miniGameCombatCommand == MiniGameCombatCommand.Fight)
        {
            //如果是攻击和技能 则开启力度测试
            OpenPowerTest(miniGameData.GetRoundActionCharacter().characterMiniGameData);
        }
        else
        {
            CloseAll();
            EventHandler.Instance.TriggerEvent(EventsInfo.MiniGameCombat_EventForCommandEnd);
        }
    }
    #endregion


    #region 物品和技能选择回调
    public void PickItemsComplete(ItemsInfoBean itemsInfo)
    {
        EffectDetailsEnumTools.GetEffectRange(itemsInfo.effect_details, out int impactNumber, out int impactType);
        OpenSelectCharacter(impactNumber, impactType);
    }

    public void PickSkillComplete(SkillInfoBean skillInfo)
    {
        EffectDetailsEnumTools.GetEffectRange(skillInfo.effect_details, out int impactNumber, out int impactType);
        OpenSelectCharacter(impactNumber, impactType);
    }

    public void PassComplete()
    {
        CloseAll();
        EventHandler.Instance.TriggerEvent(EventsInfo.MiniGameCombat_EventForCommandEnd);
    }
    #endregion

    #region 弹出框回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {

    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {

    }
    #endregion

    public void RadioButtonSelected(RadioButtonView view, bool isSelect)
    {
        //自动战斗
        if (view == ui_Auto)
        {
            GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
            if(gameData.isAutoForCombat != isSelect)
            {
                gameData.isAutoForCombat = isSelect;
                if (isSelect)
                {
                    uiForCombatCommand.CloseUI();
                    MiniGameCharacterForCombatBean roundGameCharacterData = MiniGameHandler.Instance.handlerForCombat.roundGameCharacterData;
                    //通知轮到角色回合
                    EventHandler.Instance.TriggerEvent(EventsInfo.MiniGameCombat_EventForCharacterRound, roundGameCharacterData);
                }
            }
        }
    }

    public void RadioButtonSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {
        if (rbview == ui_Speed1)
        {
            CombatSpeedChange(1);
        }
        else if (rbview == ui_Speed2)
        {
            CombatSpeedChange(2);
        }
        else if (rbview == ui_Speed5)
        {
            CombatSpeedChange(5);
        }
    }

    public void RadioButtonUnSelected(RadioGroupView rgView, int position, RadioButtonView rbview)
    {

    }
}