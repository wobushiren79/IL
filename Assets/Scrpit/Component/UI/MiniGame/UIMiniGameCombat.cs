using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using System;

public class UIMiniGameCombat : UIBaseMiniGame<MiniGameCombatBean>, CombatPowerView.ICallBack
{
    //指令UI
    public GameObject objCommand;
    public Button btCommandFight;
    public Button btCommandSkill;
    public Button btCommandItem;

    //角色选择UI
    public UIMiniGameCombatSelectCharacter uiForSelectCharacter;

    //攻击力道 
    public CombatPowerView viewCombatPower;
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

    public List<ItemMiniGameCombatCharacterInfoCpt> mListCharacterInfo = new List<ItemMiniGameCombatCharacterInfoCpt>();
    public List<ItemMiniGameCombatCharacterRoundCpt> listCharacterRound = new List<ItemMiniGameCombatCharacterRoundCpt>();

    private ICallBack mCallBack;


    public bool isRounding = false;//是否回合进行中
    public bool isSelecting = false;//是否正在选择中

    private void Start()
    {
        if (btCommandFight)
            btCommandFight.onClick.AddListener(CommandFight);
        if (btCommandSkill)
            btCommandSkill.onClick.AddListener(CommandSkill);
        if (btCommandItem)
            btCommandItem.onClick.AddListener(CommandItem);

        if (viewCombatPower != null)
            viewCombatPower.SetCallBack(this);
    }

    private void Update()
    {
        //回合条移动处理
        HandleForRoundMove();
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
    /// 刷新UI
    /// </summary>
    public override void RefreshUI()
    {
        base.RefreshUI();
    }

    /// <summary>
    /// 设置回调
    /// </summary>
    /// <param name="callBack"></param>
    public void SetCallBack(ICallBack callBack)
    {
        this.mCallBack = callBack;
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
        mListCharacterInfo.Clear();
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
        mListCharacterInfo.Add(characterInfoCpt);
        //创建回合条信息
        GameObject objRoundItem = Instantiate(objRoundCharacterContainer, objRoundCharacterModel);
        RectTransform rtfItemRound = objRoundItem.GetComponent<RectTransform>();
        ItemMiniGameCombatCharacterRoundCpt characterRoundCpt = objRoundItem.GetComponent<ItemMiniGameCombatCharacterRoundCpt>();
        characterRoundCpt.SetData((MiniGameCharacterForCombatBean)gameCharacterData);
        listCharacterRound.Add(characterRoundCpt);
        //添加动画
        objItem.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f).From().SetDelay(0.1f * position).SetEase(Ease.OutBack);
        objRoundItem.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f).From().SetDelay(0.1f * position).SetEase(Ease.OutBack);
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
        for (int i = 0; i < listCharacterRound.Count; i++)
        {
            ItemMiniGameCombatCharacterRoundCpt itemCpt = listCharacterRound[i];
            //获取角色属性
            itemCpt.gameCharacterData.characterData.GetAttributes(uiGameManager.gameItemsManager, out CharacterAttributesBean characterAttributes);
            float roundSpeed = (0.5f + characterAttributes.speed/10f);
            //回合条向右移动
            itemCpt.transform.Translate(new Vector3(1, 0) * Time.deltaTime * roundSpeed);
            //检测是否到达目标点
            if (itemCpt.transform.position.x >= objRoundEnd.transform.position.x)
            {
                itemCpt.transform.position = objRoundEnd.transform.position;
                //设置为选中状态
                itemCpt.SetStatus(true);
                //通知轮到角色回合
                if (mCallBack != null)
                    mCallBack.CharacterRound(itemCpt.gameCharacterData);
                isRounding = false;
                break;
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
    public void OpenCommand()
    {
        objCommand.SetActive(true);
        objCommand.transform.localScale = new Vector3(1, 1, 1);
        objCommand.transform.DOScale(new Vector3(0f, 0f, 0f), 0.5f).From().SetEase(Ease.OutBack).SetDelay(0.2f);
    }

    /// <summary>
    /// 关闭指令
    /// </summary>
    public void CloseCommand()
    {
        objCommand.SetActive(false);
    }

    /// <summary>
    /// 指令-战斗
    /// </summary>
    public void CommandFight()
    {
        isSelecting = true;
        uiForSelectCharacter.Open();
        uiForSelectCharacter.SetData(1,2);

        if (mCallBack != null)
            mCallBack.CommandFight(0);
        objCommand.SetActive(false);
    }

    /// <summary>
    /// 指令 技能
    /// </summary>
    public void CommandSkill()
    {

    }

    /// <summary>
    /// 指令物品
    /// </summary>
    public void CommandItem()
    {
        ToastManager toastManager = GetUIManager<UIGameManager>().toastManager;
        toastManager.ToastHint("开发中");
    }

    /// <summary>
    /// 开启力道测试
    /// </summary>
    /// <param name="gameCharacterData"></param>
    public void OpenCombatPowerTest(MiniGameCharacterBean gameCharacterData)
    {
        //弹出进度条
        viewCombatPower.gameObject.SetActive(true);
        //获取属性
        GameItemsManager gameItemsManager = GetUIManager<UIGameManager>().gameItemsManager;
        gameCharacterData.characterData.GetAttributes(gameItemsManager, out CharacterAttributesBean characterAttributes);
        //设置数据
        viewCombatPower.SetData(characterAttributes.force);
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
    public void PowerTestEnd(float resultsAccuracy, float resultsForce)
    {
        viewCombatPower.gameObject.SetActive(false);
        if (mCallBack != null)
            mCallBack.PowerTestEnd(resultsAccuracy, resultsForce);
    }
    #endregion

    public interface ICallBack
    {
        /// <summary>
        /// 轮到一个角色的回合
        /// </summary>
        /// <param name="gameCharacterData"></param>
        void CharacterRound(MiniGameCharacterForCombatBean gameCharacterData);

        /// <summary>
        /// 指令 战斗
        /// </summary>
        void CommandFight(int detials);

        /// <summary>
        /// 指令 技能
        /// </summary>
        void CommandSkill();

        /// <summary>
        /// 指令 技能
        /// </summary>
        void CommandItems();

        /// <summary>
        /// 力度测试
        /// </summary>
        void PowerTestEnd(float resultsAccuracy, float resultsForce);
    }

}