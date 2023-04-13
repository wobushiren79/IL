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

    private bool mIsPlay = false;
    protected int buttonNumber = 45;
    protected int buttonPosition = 0;

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
    public void SetData(MiniGameCookingBean gameCookingData, float gameTiming)
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
        SetTitle(TextHandler.Instance.manager.GetTextById(231));
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
        SetTitle(TextHandler.Instance.manager.GetTextById(232));
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
        SetTitle(TextHandler.Instance.manager.GetTextById(233));
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
        if (gameCookingData.cookButtonNumber > 2)
        {
            if (gameCookingData.cookButtonNumber >= 3)
            {
                listRandomType.Add(ItemMiniGameCookingButtonCpt.MiniGameCookingButtonTypeEnum.Up);
            }
            if (gameCookingData.cookButtonNumber >= 4)
            {
                listRandomType.Add(ItemMiniGameCookingButtonCpt.MiniGameCookingButtonTypeEnum.Down);
            }
            if (gameCookingData.cookButtonNumber >= 5)
            {
                listRandomType.Add(ItemMiniGameCookingButtonCpt.MiniGameCookingButtonTypeEnum.One);
            }
            if (gameCookingData.cookButtonNumber >= 6)
            {
                listRandomType.Add(ItemMiniGameCookingButtonCpt.MiniGameCookingButtonTypeEnum.Two);
            }
            if (gameCookingData.cookButtonNumber >= 7)
            {
                listRandomType.Add(ItemMiniGameCookingButtonCpt.MiniGameCookingButtonTypeEnum.Three);
            }
            if (gameCookingData.cookButtonNumber >= 8)
            {
                listRandomType.Add(ItemMiniGameCookingButtonCpt.MiniGameCookingButtonTypeEnum.Four);
            }
        }
        for (int i = 0; i < buttonNumber; i++)
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
        if (objTitle != null && tvTitle != null)
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
        buttonPosition = 0;
        int correctNumber = 0;
        int errorNumber = 0;
        int unfinishNumber = 0;
        foreach (ItemMiniGameCookingButtonCpt itemCpt in mListButton)
        {
            switch (itemCpt.buttonStatus)
            {
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
            tvCountDown.text = TextHandler.Instance.manager.GetTextById(256);
            tvCountDown.transform.DOScale(new Vector3(3, 3, 3), 0.5f).From().SetEase(Ease.OutBack);
            yield return new WaitForSeconds(3);
        }

        MiniGameCookingSettleBean cookingSettleData = new MiniGameCookingSettleBean
        {
            maxTime = sliderTime.maxValue,
            residueTime = sliderTime.value,
            correctNumber = correctNumber,
            errorNumber = errorNumber,
            unfinishNumber = unfinishNumber
        };
        EventHandler.Instance.TriggerEvent(EventsInfo.MiniGameCooking_CookingSettle, mPhaseType, cookingSettleData);
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
        if (buttonPosition == 0)
        {
            //倒计时开始计时
            StartCoroutine(CoroutineForTiming());
        }
        ItemMiniGameCookingButtonCpt itemButton = mListButton[buttonPosition];
        if (itemButton.buttonType == type)
        {
            AudioHandler.Instance.PlaySound(AudioSoundEnum.Correct);
            itemButton.SetButtonClickCorrect();
        }
        else
        {
            sliderTime.value -= 1;
            AudioHandler.Instance.PlaySound(AudioSoundEnum.Error);
            itemButton.SetButtonClickError();
        }
        //取消选中状态
        itemButton.SetSelectedStatus(false);
        buttonPosition++;
        if (buttonPosition >= buttonNumber)
        {
            StartCoroutine(SettleGame());
            return;
        }
        //设置选中状态
        mListButton[buttonPosition].SetSelectedStatus(true);
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
        yield return new WaitForSeconds(2);
        AudioHandler.Instance.PlaySound(AudioSoundEnum.CountDownStart);
        tvCountDown.text = TextHandler.Instance.manager.GetTextById(252);
        tvCountDown.transform.DOScale(new Vector3(3, 3, 3), 0.5f).From().SetEase(Ease.OutBack);
        yield return new WaitForSeconds(0.7f);
        AudioHandler.Instance.PlaySound(AudioSoundEnum.CountDownStart);
        tvCountDown.text = TextHandler.Instance.manager.GetTextById(253);
        tvCountDown.transform.DOScale(new Vector3(3, 3, 3), 0.5f).From().SetEase(Ease.OutBack);
        yield return new WaitForSeconds(0.7f);
        AudioHandler.Instance.PlaySound(AudioSoundEnum.CountDownStart);
        tvCountDown.text = TextHandler.Instance.manager.GetTextById(254);
        tvCountDown.transform.DOScale(new Vector3(3, 3, 3), 0.5f).From().SetEase(Ease.OutBack);
        yield return new WaitForSeconds(0.7f);
        AudioHandler.Instance.PlaySound(AudioSoundEnum.CountDownEnd);
        tvCountDown.text = TextHandler.Instance.manager.GetTextById(255);
        tvCountDown.transform.DOScale(new Vector3(3, 3, 3), 0.5f).From().SetEase(Ease.OutBack);
        yield return new WaitForSeconds(0.7f);
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
            sliderTime.value -= 0.1f;
            if (sliderTime.value <= 0)
            {
                StartCoroutine(SettleGame());
            }
        }
    }
}