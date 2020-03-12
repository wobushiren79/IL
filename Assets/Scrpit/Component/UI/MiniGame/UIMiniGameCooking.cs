using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;

public class UIMiniGameCooking : BaseUIComponent
{
    public enum MiniGameCookingPhaseTypeEnum
    {
        Null,
        Pre,//备料
        Making,//烹饪
        End//摆盘
    }

    public GameObject objTitle;
    public Text tvTitle;
    public GameObject objCountDown;
    public Text tvCountDown;

    public Slider sliderTime;

    public GameObject objCookingContainer;
    public GameObject objCookingModel;

    public MiniGameCookingBean gameCookingData;
    public float gameTiming;//游戏计时时间

    private ICallBack mCallBack;

    private bool mIsPlay = false;
    private int mButtonNumber = 60;
    private int mButtonPosition = 0;

    private MiniGameCookingPhaseTypeEnum mPhaseType = MiniGameCookingPhaseTypeEnum.Null;

    private List<ItemMiniGameCookingButtonCpt> mListButton = new List<ItemMiniGameCookingButtonCpt>();

    private void Update()
    {
        if (!mIsPlay)
            return;
        if (Input.GetButtonDown(InputInfo.Direction_Left))
        {
            ButtonClick(ItemMiniGameCookingButtonCpt.MiniGameCookingButtonTypeEnum.Left);
            return;
        }
        if (Input.GetButtonDown(InputInfo.Direction_Right))
        {
            ButtonClick(ItemMiniGameCookingButtonCpt.MiniGameCookingButtonTypeEnum.Right);
            return;
        }
        if (Input.GetButtonDown(InputInfo.Direction_Up))
        {
            ButtonClick(ItemMiniGameCookingButtonCpt.MiniGameCookingButtonTypeEnum.Up);
            return;
        }
        if (Input.GetButtonDown(InputInfo.Direction_Down))
        {
            ButtonClick(ItemMiniGameCookingButtonCpt.MiniGameCookingButtonTypeEnum.Down);
            return;
        }
        if (Input.GetButtonDown(InputInfo.Number_1))
        {
            ButtonClick(ItemMiniGameCookingButtonCpt.MiniGameCookingButtonTypeEnum.One);
            return;
        }
        if (Input.GetButtonDown(InputInfo.Number_2))
        {
            ButtonClick(ItemMiniGameCookingButtonCpt.MiniGameCookingButtonTypeEnum.Two);
            return;
        }
        if (Input.GetButtonDown(InputInfo.Number_3))
        {
            ButtonClick(ItemMiniGameCookingButtonCpt.MiniGameCookingButtonTypeEnum.Three);
            return;
        }
        if (Input.GetButtonDown(InputInfo.Number_4))
        {
            ButtonClick(ItemMiniGameCookingButtonCpt.MiniGameCookingButtonTypeEnum.Four);
            return;
        }
    }

    public void SetCallBack(ICallBack callBack)
    {
        this.mCallBack = callBack;
    }

    public void SetData(MiniGameCookingBean gameCookingData,float gameTiming)
    {
        this.gameCookingData = gameCookingData;
        this.gameTiming = gameTiming;
    }

    /// <summary>
    /// 开始备料
    /// </summary>
    public void StartCookingPre()
    {
        mPhaseType = MiniGameCookingPhaseTypeEnum.Pre;
        SetTitle(GameCommonInfo.GetUITextById(231));
        //创建按钮
        CreateRandomCookingButton();
        StartCountDown();
    }

    /// <summary>
    /// 开始烹饪
    /// </summary>
    public void StartCookingMaking()
    {
        mPhaseType = MiniGameCookingPhaseTypeEnum.Making;
        SetTitle(GameCommonInfo.GetUITextById(232));
        //创建按钮
        CreateRandomCookingButton();
        StartCountDown();
    }

    /// <summary>
    /// 开始摆盘
    /// </summary>
    public void StartCookingEnd()
    {
        mPhaseType = MiniGameCookingPhaseTypeEnum.End;
        SetTitle(GameCommonInfo.GetUITextById(233));
        //创建按钮
        CreateRandomCookingButton();
        StartCountDown();
    }

    public void StartCountDown()
    {
        //开始倒计时
        StartCoroutine(CoroutineForCountDown());
    }

    /// <summary>
    /// 创建游戏按键
    /// </summary>
    public void CreateRandomCookingButton()
    {
        CptUtil.RemoveChildsByActive(objCookingContainer);
        mListButton.Clear();
        List<ItemMiniGameCookingButtonCpt.MiniGameCookingButtonTypeEnum> listRandomType = new List<ItemMiniGameCookingButtonCpt.MiniGameCookingButtonTypeEnum>();
        listRandomType.Add(ItemMiniGameCookingButtonCpt.MiniGameCookingButtonTypeEnum.Left);
        listRandomType.Add(ItemMiniGameCookingButtonCpt.MiniGameCookingButtonTypeEnum.Right);
        if (gameCookingData.cookButtonNumber == 4 )
        {
            listRandomType.Add(ItemMiniGameCookingButtonCpt.MiniGameCookingButtonTypeEnum.Up);
            listRandomType.Add(ItemMiniGameCookingButtonCpt.MiniGameCookingButtonTypeEnum.Down);
        }
        if (gameCookingData.cookButtonNumber == 6)
        {
            listRandomType.Add(ItemMiniGameCookingButtonCpt.MiniGameCookingButtonTypeEnum.One);
            listRandomType.Add(ItemMiniGameCookingButtonCpt.MiniGameCookingButtonTypeEnum.Two);
        }
        if (gameCookingData.cookButtonNumber == 8)
        {
            listRandomType.Add(ItemMiniGameCookingButtonCpt.MiniGameCookingButtonTypeEnum.Three);
            listRandomType.Add(ItemMiniGameCookingButtonCpt.MiniGameCookingButtonTypeEnum.Four);
        }
        for (int i = 0; i < mButtonNumber; i++)
        {
            ItemMiniGameCookingButtonCpt.MiniGameCookingButtonTypeEnum randomType = RandomUtil.GetRandomDataByList(listRandomType);
            CreateCookingButtonItem(i, randomType);
        }
        //默认第一个选中
        mListButton[0].SetSelectedStatus(true);
    }

    /// <summary>
    /// 设置标题
    /// </summary>
    /// <param name="name"></param>
    public void SetTitle(string name)
    {
        if (objTitle!=null&&tvTitle != null)
        {
            tvTitle.text = name;
            objTitle.transform.DOScale(new Vector3(3, 3, 3), 1).From().SetEase(Ease.OutBack);
        }
    }

    /// <summary>
    /// 结算游戏成绩
    /// </summary>
    public IEnumerator SettleGame()
    {
        mIsPlay = false;
        mButtonPosition = 0;
        int correctNumber = 0;
        int errorNumber = 0;
        int unfinishNumber = 0;
        foreach (ItemMiniGameCookingButtonCpt itemCpt in mListButton)
        {
            switch (itemCpt.buttonStatus) {
                case 0:
                    unfinishNumber++;
                    break;
                case 1:
                    correctNumber++;
                    break;
                case 2:
                    errorNumber++;
                    break;
            }
        }
        if (mPhaseType == MiniGameCookingPhaseTypeEnum.End)
        {
            objCountDown.SetActive(true);
            tvCountDown.text = GameCommonInfo.GetUITextById(256);
            tvCountDown.transform.DOScale(new Vector3(3, 3, 3), 0.5f).From().SetEase(Ease.OutBack);
            yield return new WaitForSeconds(3);
        }
        if (mCallBack != null)
        {
            MiniGameCookingSettleBean cookingSettleData = new MiniGameCookingSettleBean
            {
                maxTime = sliderTime.maxValue,
                residueTime = sliderTime.value,
                correctNumber = correctNumber,
                errorNumber = errorNumber,
                unfinishNumber = unfinishNumber
            };
            mCallBack.UIMiniGameCookingSettle(mPhaseType, cookingSettleData);
        }
    }

    /// <summary>
    ///  创建游戏按键Item
    /// </summary>
    private GameObject CreateCookingButtonItem(int position, ItemMiniGameCookingButtonCpt.MiniGameCookingButtonTypeEnum type)
    {
        GameObject objItem = Instantiate(objCookingContainer, objCookingModel);
        ItemMiniGameCookingButtonCpt itemCpt = objItem.GetComponent<ItemMiniGameCookingButtonCpt>();
        itemCpt.SetData(type);
        objItem.transform.DOScale(new Vector3(0, 0, 0), 0.5f).From().SetEase(Ease.OutBack).SetDelay(position * 0.05f);

        mListButton.Add(itemCpt);
        return objItem;
    }

    /// <summary>
    /// 按钮点击
    /// </summary>
    /// <param name="type"></param>
    private void ButtonClick(ItemMiniGameCookingButtonCpt.MiniGameCookingButtonTypeEnum type)
    {
        //当第一次点击后开始计时
        if (mButtonPosition == 0)
        {
            //倒计时开始计时
            StartCoroutine(CoroutineForTiming());
        }
        ItemMiniGameCookingButtonCpt itemButton= mListButton[mButtonPosition];
        if(itemButton.buttonType== type)
        {
            itemButton.SetButtonClickCorrect();
        }
        else
        {
            itemButton.SetButtonClickError();
        }
        //取消选中状态
        itemButton.SetSelectedStatus(false);
        mButtonPosition++;
        if (mButtonPosition >= mButtonNumber)
        {
            StartCoroutine(SettleGame());
            return;
        }
        //设置选中状态
        mListButton[mButtonPosition].SetSelectedStatus(true);
    }

    /// <summary>
    /// 协程 倒计时
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoroutineForCountDown()
    {
        sliderTime.maxValue = gameTiming;
        sliderTime.value = sliderTime.maxValue;
        objCountDown.SetActive(true);
        tvCountDown.text = tvTitle.text;
        tvCountDown.transform.DOScale(new Vector3(3, 3, 3), 0.5f).From().SetEase(Ease.OutBack);
        yield return new WaitForSeconds(5);
        tvCountDown.text = GameCommonInfo.GetUITextById(252);
        tvCountDown.transform.DOScale(new Vector3(3, 3, 3), 0.5f).From().SetEase(Ease.OutBack);
        yield return new WaitForSeconds(1);
        tvCountDown.text = GameCommonInfo.GetUITextById(253);
        tvCountDown.transform.DOScale(new Vector3(3, 3, 3), 0.5f).From().SetEase(Ease.OutBack);
        yield return new WaitForSeconds(1);
        tvCountDown.text = GameCommonInfo.GetUITextById(254);
        tvCountDown.transform.DOScale(new Vector3(3, 3, 3), 0.5f).From().SetEase(Ease.OutBack);
        yield return new WaitForSeconds(1);
        tvCountDown.text = GameCommonInfo.GetUITextById(255);
        tvCountDown.transform.DOScale(new Vector3(3, 3, 3), 0.5f).From().SetEase(Ease.OutBack);
        yield return new WaitForSeconds(1);
        objCountDown.SetActive(false);
        mIsPlay = true;
    }

    /// <summary>
    /// 协程 计时
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoroutineForTiming()
    {
        if (gameTiming < 1)
            gameTiming = 1;
        sliderTime.maxValue = gameTiming;
        sliderTime.value = sliderTime.maxValue;
        while (mIsPlay)
        {
            yield return new WaitForSeconds(0.1f);
            sliderTime.value-=0.1f;
            if (sliderTime.value <= 0)
            {
                StartCoroutine(SettleGame());
            }
        }
    }

    public interface ICallBack
    {
        /// <summary>
        /// 游戏完成结算
        /// </summary>
        /// <param name="type">1备料  2烹饪 3摆盘</param>
        void UIMiniGameCookingSettle(MiniGameCookingPhaseTypeEnum type, MiniGameCookingSettleBean settleData);
    }
}