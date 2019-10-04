using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;

public class UIMiniGameCountDown : BaseUIComponent
{
    public GameObject objTarget;
    public GameObject objCountDown;

    public Text tvTargetTitle;
    public Text tvTargetWinConditions;
    public Button btTargetStart;

    public Text tvCountDown;

    private ICallBack mCallBack;

    private void Start()
    {
        if (btTargetStart != null)
            btTargetStart.onClick.AddListener(StartCountDown);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        if (GetUIMananger<UIGameManager>().controlHandler != null)
            GetUIMananger<UIGameManager>().controlHandler.StopControl();
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="titleStr">标题</param>
    /// <param name="listWinRequired">胜利条件</param>
    public void SetData(string titleStr, List<string> listWinConditions)
    {
        objTarget.SetActive(true);
        objCountDown.SetActive(false);
        SetTargetContent(titleStr, listWinConditions);
    }

    /// <summary>
    /// 设置目标内容
    /// </summary>
    /// <param name="titleStr"></param>
    public void SetTargetContent(string titleStr, List<string> listWinRequired)
    {
        if (tvTargetTitle != null)
            tvTargetTitle.text = titleStr;
        if (tvTargetWinConditions != null && !CheckUtil.ListIsNull(listWinRequired))
        {
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
        objTarget.SetActive(false);
        objCountDown.SetActive(true);
        //恢复控制
        if (GetUIMananger<UIGameManager>().controlHandler != null)
            GetUIMananger<UIGameManager>().controlHandler.RestoreControl();
        //倒计时
        StartCoroutine(ShowCountDown());
    }

    public IEnumerator ShowCountDown()
    {
        tvCountDown.text = "";
        for (int i = 0; i <= 3; i++)
        {
            string numberStr = "";
            if (i == 0)
                numberStr = "三";
            else if (i == 1)
                numberStr = "二";
            else if (i == 2)
                numberStr = "一";
            else if (i == 3)
                numberStr = "开始";
            tvCountDown.text = numberStr;
            tvCountDown.transform.localScale = new Vector3(1, 1, 1);
            tvCountDown.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f).From().SetEase(Ease.OutBack);
            yield return new WaitForSeconds(1.3f);
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
        //游戏准备倒计时
        void GamePreCountDownEnd();
    }

}
