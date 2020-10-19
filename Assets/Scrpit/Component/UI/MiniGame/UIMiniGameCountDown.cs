using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;

public class UIMiniGameCountDown : UIGameComponent
{
    public GameObject objTarget;
    public GameObject objCountDown;

    public Text tvTargetTitle;
    public Text tvTargetWinConditions;
    public Button btTargetStart;

    public Text tvCountDown;

    private ICallBack mCallBack;
    //是否倒计时
    private bool mIsCountDown;
    private void Start()
    {
        if (btTargetStart != null)
            btTargetStart.onClick.AddListener(StartCountDown);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        if (uiGameManager.controlHandler != null)
            uiGameManager.controlHandler.StopControl();
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="titleStr">标题</param>
    /// <param name="listWinRequired">胜利条件</param>
    public void SetData(string titleStr, List<string> listWinConditions, bool isCountDown)
    {
        this.mIsCountDown = isCountDown;

        objTarget.SetActive(true);
        objCountDown.SetActive(false);
        SetTitle(titleStr);
        SetTargetContent(listWinConditions);

        //UI动画
        objTarget.transform.localScale = new Vector3(1, 1, 1);
        objTarget.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f).From().SetEase(Ease.OutBack);
    }

    /// <summary>
    /// 设置标题内容
    /// </summary>
    /// <param name="titleStr"></param>
    public void SetTitle(string titleStr)
    {
        if (tvTargetTitle != null)
            tvTargetTitle.text = titleStr;
    }

    /// <summary>
    /// 设置目标内容
    /// </summary>
    /// <param name="titleStr"></param>
    public void SetTargetContent(List<string> listWinRequired)
    {
        if (tvTargetWinConditions != null && listWinRequired != null)
        {
            tvTargetWinConditions.text = "";
            foreach (string required in listWinRequired)
            {
                tvTargetWinConditions.text += ("►" + required + "\n");
            }
        }
    }


    /// <summary>
    /// 开始倒计时
    /// </summary>
    public void StartCountDown()
    {
        uiGameManager.audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        objTarget.SetActive(false);
        objCountDown.SetActive(true);
        //恢复控制
        if (GetUIManager<UIGameManager>().controlHandler != null)
            GetUIManager<UIGameManager>().controlHandler.RestoreControl();

        if (mIsCountDown)
        {
            //倒计时
            StartCoroutine(ShowCountDown());
        }
        else
        {
            //没有倒计时 直接结束
            if (mCallBack != null)
                mCallBack.GamePreCountDownEnd();
        }
    }

    public IEnumerator ShowCountDown()
    {
        //回调
        if (mCallBack != null)
            mCallBack.GamePreCountDownStart();
        //恢复控制
        if (GetUIManager<UIGameManager>().controlHandler != null)
            GetUIManager<UIGameManager>().controlHandler.RestoreControl();
        tvCountDown.text = "";
        for (int i = 0; i <= 3; i++)
        {
            string numberStr = "";
            if (i == 0)
                numberStr = GameCommonInfo.GetUITextById(252);
            else if (i == 1)
                numberStr = GameCommonInfo.GetUITextById(253);
            else if (i == 2)
                numberStr = GameCommonInfo.GetUITextById(254);
            else if (i == 3)
                numberStr = GameCommonInfo.GetUITextById(255);
            tvCountDown.text = numberStr;
            tvCountDown.transform.localScale = new Vector3(1, 1, 1);
            tvCountDown.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f).From().SetEase(Ease.OutBack);
            if (i == 3)
            {
                uiGameManager.audioHandler.PlaySound(AudioSoundEnum.CountDownEnd);
            }
            else
            {
                uiGameManager.audioHandler.PlaySound(AudioSoundEnum.CountDownStart);
            }
            yield return new WaitForSeconds(0.8f);
        }
        if (mCallBack != null)
            mCallBack.GamePreCountDownEnd();
    }

    public void SetCallBack(ICallBack callBack)
    {
        mCallBack = callBack;
    }

    public interface ICallBack
    {
        //游戏开始倒计时
        void GamePreCountDownStart();
        //游戏倒计时结束
        void GamePreCountDownEnd();
    }

}
