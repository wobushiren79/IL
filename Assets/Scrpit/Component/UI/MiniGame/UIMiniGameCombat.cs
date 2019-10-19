using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using System;

public class UIMiniGameCombat : UIBaseMiniGame, CombatPowerView.ICallBack
{
    //指令UI
    public GameObject objCommand;
    public Button btCommandFight;
    public Button btCommandDefend;
    public Button btCommandItem;

    //攻击指令相关
    public GameObject objCommandFight;
    public Button btCommandFightForChange;
    public Button btCommandFightForConfirm;
    public Button btCommandFightForCancel;

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

    public MiniGameCombatBean gameCombatData;

    public List<ItemMiniGameCombatCharacterInfoCpt> mListCharacterInfo = new List<ItemMiniGameCombatCharacterInfoCpt>();
    public List<ItemMiniGameCombatCharacterRoundCpt> mListCharacterRound = new List<ItemMiniGameCombatCharacterRoundCpt>();

    private ICallBack mCallBack;

    private float mRoundContainerW = 0;

    private void Start()
    {
        if (btCommandFight)
            btCommandFight.onClick.AddListener(CommandFight);
        if (btCommandDefend)
            btCommandDefend.onClick.AddListener(CommandDefend);
        if (btCommandItem)
            btCommandItem.onClick.AddListener(CommandItem);

        if (btCommandFightForChange)
            btCommandFightForChange.onClick.AddListener(CommandFightForChange);
        if (btCommandFightForConfirm)
            btCommandFightForConfirm.onClick.AddListener(CommandFightForConfirm);
        if (btCommandFightForCancel)
            btCommandFightForCancel.onClick.AddListener(CommandFightForCancel);

        if (viewCombatPower != null)
            viewCombatPower.SetCallBack(this);
    }

    private void Awake()
    {
        if (rtfRoundContainer != null)
            mRoundContainerW = rtfRoundContainer.rect.width;
    }

    private void Update()
    {
        //排序
        if (mListCharacterRound != null)
        {
            mListCharacterRound = mListCharacterRound.OrderByDescending(i => ((RectTransform)i.transform).anchoredPosition.x).ToList();
            for (int i = 0; i < mListCharacterRound.Count; i++)
            {
                RectTransform itemRTF = (RectTransform)mListCharacterRound[i].transform;
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
    public void SetData(MiniGameCombatBean gameCombatData)
    {
        if (gameCombatData == null)
            return;
        this.gameCombatData = gameCombatData;
        //清空数据
        mListCharacterInfo.Clear();
        mListCharacterRound.Clear();
        CptUtil.RemoveChildsByActive(objRoundCharacterContainer.transform);
        CptUtil.RemoveChildsByActive(objOurCharacterContainer.transform);
        CptUtil.RemoveChildsByActive(objEnemyCharacterContainer.transform);

        //创建Item
        CreateOurCharacterList(gameCombatData.listUserGameData);
        CreateEnemyCharacterList(gameCombatData.listEnemyGameData);
    }

    /// <summary>
    /// 移除角色回合行动
    /// </summary>
    public void RemoveCharacterRound(MiniGameCharacterBean miniGameCharacter)
    {
        for (int i = 0;i<mListCharacterRound.Count;i++ )
        {
            ItemMiniGameCombatCharacterRoundCpt itemRound = mListCharacterRound[i];
            if (itemRound.gameCharacterData == miniGameCharacter)
            {
                mListCharacterRound.Remove(itemRound);
                Destroy(itemRound.gameObject);
                return;
            }
        }
    }

    /// <summary>
    /// 开启命令UI
    /// </summary>
    public void OpenCommand()
    {
        objCommand.SetActive(true);
        objCommand.transform.localScale = new Vector3(1, 1, 1);
        objCommand.transform.DOScale(new Vector3(0f, 0f, 0f), 0.5f).From().SetEase(Ease.OutBack).SetDelay(0.2f);
    }
    public void OpenCommandFight()
    {
        objCommandFight.SetActive(true);
        objCommandFight.transform.localScale = new Vector3(1, 1, 1);
        objCommandFight.transform.DOScale(new Vector3(0f, 0f, 0f), 0.5f).From().SetEase(Ease.OutBack).SetDelay(0.2f);
    }

    public void CloseCommand()
    {
        objCommand.SetActive(false);
    }

    /// <summary>
    /// 指令-战斗
    /// </summary>
    public void CommandFight()
    {
        if (mCallBack != null)
            mCallBack.CommandFight(0);
        objCommand.SetActive(false);
        OpenCommandFight();
    }
    private void CommandFightForChange()
    {
        if (mCallBack != null)
            mCallBack.CommandFight(1);
    }
    private void CommandFightForConfirm()
    {
        if (mCallBack != null)
            mCallBack.CommandFight(2);
        objCommandFight.SetActive(false);
    }
    private void CommandFightForCancel()
    {
        if (mCallBack != null)
            mCallBack.CommandFight(3);
        objCommandFight.SetActive(false);
        OpenCommand();
    }

    /// <summary>
    /// 指定防守
    /// </summary>
    public void CommandDefend()
    {
        if (mCallBack != null)
            mCallBack.CommandDefend(0);
    }

    /// <summary>
    /// 指令物品
    /// </summary>
    public void CommandItem()
    {
        ToastView toastView = GetUIMananger<UIGameManager>().toastView;
        toastView.ToastHint("开发中");
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
        GameItemsManager gameItemsManager = GetUIMananger<UIGameManager>().gameItemsManager;
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
        for (int i = 0; i < mListCharacterRound.Count; i++)
        {
            ItemMiniGameCombatCharacterRoundCpt itemCpt = mListCharacterRound[i];
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

    /// <summary>
    /// 开始回合计时
    /// </summary>
    public void StartRound()
    {
        GameItemsManager gameItemsManager = GetUIMananger<UIGameManager>().gameItemsManager;
        for (int i = 0; i < mListCharacterRound.Count; i++)
        {
            ItemMiniGameCombatCharacterRoundCpt itemCpt = mListCharacterRound[i];
            RectTransform itemRTF = (RectTransform)itemCpt.transform;

            itemCpt.gameCharacterData.characterData.GetAttributes(gameItemsManager, out CharacterAttributesBean characterAttributes);
            //计算角色的回合的速度和到达时间
            float characterSpeed = (1 + characterAttributes.speed) * 5;
            float roundTime = (mRoundContainerW - itemRTF.anchoredPosition.x) / characterSpeed;
            itemRTF.DOAnchorPosX((mRoundContainerW), roundTime)
                .SetEase(Ease.Linear)
                .OnComplete(
                    delegate ()
                    {
                        //停止当前其他回合
                        foreach (ItemMiniGameCombatCharacterRoundCpt tempItemCpt in mListCharacterRound)
                        {
                            DOTween.Kill(tempItemCpt.transform);
                        }

                        //通知轮到角色回合
                        if (mCallBack != null)
                            mCallBack.CharacterRoundCombat(itemCpt.gameCharacterData);
                        //设置为选中状态
                        itemCpt.SetStatus(true);
                    }
             );
        }
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
        characterRoundCpt.SetData(gameCharacterData);
        mListCharacterRound.Add(characterRoundCpt);
        //添加动画
        objItem.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f).From().SetDelay(0.1f * position).SetEase(Ease.OutBack);
        objRoundItem.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f).From().SetDelay(0.1f * position).SetEase(Ease.OutBack);
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
        void CharacterRoundCombat(MiniGameCharacterBean gameCharacterData);

        /// <summary>
        /// 指令 战斗
        /// </summary>
        void CommandFight(int detials);

        /// <summary>
        /// 指令 防御
        /// </summary>
        void CommandDefend(int detials);

        /// <summary>
        /// 力度测试
        /// </summary>
        void PowerTestEnd(float resultsAccuracy, float resultsForce);
    }

}