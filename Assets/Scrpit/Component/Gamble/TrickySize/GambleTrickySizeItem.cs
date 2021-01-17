using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;

public class GambleTrickySizeItem : BaseMonoBehaviour
{
    public enum CupStatusEnum
    {
        Idle = 0,//闲置中
        Changing,//变化中
        Choosing,//选择中
        Result,//结果展示中
    }

    public Image ivDice;
    public Image ivCup;
    public Button btBig;
    public Button btSmall;

    public Sprite spDiceOne;
    public Sprite spDiceTwo;
    public Sprite spDiceThree;
    public Sprite spDiceFour;
    public Sprite spDiceFive;
    public Sprite spDiceSix;


    public CupStatusEnum cupStatus = CupStatusEnum.Idle;

    protected ICallBack callBack;
    protected AudioHandler audioHandler;

    private void Awake()
    {
        audioHandler = Find<AudioHandler>(ImportantTypeEnum.AudioHandler);
    }

    public void SetCallBack(ICallBack callBack)
    {
        this.callBack = callBack;
    }

    /// <summary>
    /// 设置状态
    /// </summary>
    /// <param name="cupStatus"></param>
    public void SetStatus(CupStatusEnum cupStatus)
    {
        this.cupStatus = cupStatus;
        btBig.gameObject.SetActive(false);
        btSmall.gameObject.SetActive(false);
        switch (cupStatus)
        {
            case CupStatusEnum.Idle:
                SetStatusForIdle();
                break;
            case CupStatusEnum.Changing:
                SetStatusForChanging();
                break;
            case CupStatusEnum.Choosing:
                SetStatusForChoosing();
                break;
            case CupStatusEnum.Result:
                SetStatusForResult();
                break;
        }
    }

    public void SetResult(int number)
    {
        switch (number)
        {
            case 1:
                ivDice.sprite = spDiceOne;
                break;
            case 2:
                ivDice.sprite = spDiceTwo;
                break;
            case 3:
                ivDice.sprite = spDiceThree;
                break;
            case 4:
                ivDice.sprite = spDiceFour;
                break;
            case 5:
                ivDice.sprite = spDiceFive;
                break;
            case 6:
                ivDice.sprite = spDiceSix;
                break;
            default:
                ivDice.sprite = spDiceOne;
                break;
        }
    }

    private void Start()
    {
        if (btBig != null)
            btBig.onClick.AddListener(OnClickBig);
        if (btSmall != null)
            btSmall.onClick.AddListener(OnClickSmall);
    }

    protected void SetStatusForIdle()
    {

    }

    protected void SetStatusForChanging()
    {
        //盖上罩子
        ivCup.rectTransform
            .DOAnchorPosY(ivCup.rectTransform.anchoredPosition.y - 100, 0.5f);
        //摇晃
        ((RectTransform)transform)
               .DOAnchorPosY(ivCup.rectTransform.anchoredPosition.y + 20, 0.2f)
               .SetLoops(10, LoopType.Yoyo)
               .SetDelay(0.6f)
               .OnStart(() =>
               {
                   AudioHandler.Instance.PlaySound(AudioSoundEnum.Dice);
               });
    }

    protected void SetStatusForChoosing()
    {
        btBig.gameObject.SetActive(true);
        btSmall.gameObject.SetActive(true);
    }

    protected void SetStatusForResult()
    {
        //移开杯子
        Vector2 startPosition = ivCup.rectTransform.anchoredPosition;
        ivCup.rectTransform.DOAnchorPosY(ivCup.rectTransform.anchoredPosition.y + 100, 0.5f);
    }

    public void OnClickBig()
    {
        if (callBack != null)
            callBack.SizeChoose(1);
    }

    public void OnClickSmall()
    {
        if (callBack != null)
            callBack.SizeChoose(0);
    }

    public interface ICallBack
    {
        void SizeChoose(int size);
    }
}