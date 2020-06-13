using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;

public class GambleTrickyCupItem : BaseMonoBehaviour
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
    public Button btSubmit;

    //是否有骰子
    public bool hasDice = false;
    public CupStatusEnum cupStatus = CupStatusEnum.Idle;
    protected ICallBack callBack;

    private void Start()
    {
        if (btSubmit != null)
            btSubmit.onClick.AddListener(OnClickSubmit);
    }

    /// <summary>
    /// 设置回调
    /// </summary>
    /// <param name="callBack"></param>
    public void SetCallBack(ICallBack callBack)
    {
        this.callBack = callBack;
    }


    /// <summary>
    /// 检测是否有骰子
    /// </summary>
    /// <returns></returns>
    public bool CheckHasDice()
    {
        return hasDice;
    }

    /// <summary>
    /// 设置状态
    /// </summary>
    /// <param name="cupStatus"></param>
    public void SetStatus(CupStatusEnum cupStatus)
    {
        this.cupStatus = cupStatus;
        btSubmit.gameObject.SetActive(false);
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

    /// <summary>
    /// 确认选择
    /// </summary>
    public void OnClickSubmit()
    {
        if (callBack != null)
            callBack.CupChoose(this);
    }

    protected void SetStatusForIdle()
    {
        if (hasDice)
        {
            ivDice.gameObject.SetActive(true);
        }
        else
        {
            ivDice.gameObject.SetActive(false);
        }
    }

    protected void SetStatusForChanging()
    {
        //盖上罩子
        ivCup.rectTransform.DOAnchorPosY(ivCup.rectTransform.anchoredPosition.y - 100, 0.5f);
    }

    protected void SetStatusForChoosing()
    {
        btSubmit.gameObject.SetActive(true);
    }

    protected void SetStatusForResult()
    {
        //移开杯子
        Vector2 startPosition = ivCup.rectTransform.anchoredPosition;
        ivCup.rectTransform.DOAnchorPosY(ivCup.rectTransform.anchoredPosition.y + 100, 0.5f);
    }

    public interface ICallBack
    {
        void CupChoose(GambleTrickyCupItem chooseCup);
    }

}