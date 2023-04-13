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
        GameControlHandler.Instance.StopControl();

        //如果是在无尽之塔的斗武 并且开启自动战斗 则自动点击
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        if (SceneUtil.GetCurrentScene() == ScenesEnum.GameInfiniteTowersScene && gameData.isAutoForCombat)
        {    
            if (MiniGameHandler.Instance.handlerForCombat.miniGameData.gameType == MiniGameEnum.Combat)
            {
                StartAutoEnd();
            }
        }
    }


    /// <summary>
    /// 自动结束
    /// </summary>
    public void StartAutoEnd()
    {
        this.WaitExecuteSeconds(2, () =>
        {
            StartCountDown();
        });
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
        StopAllCoroutines();
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        objTarget.SetActive(false);
        objCountDown.SetActive(true);
        //恢复控制
        GameControlHandler.Instance.RestoreControl();

        if (mIsCountDown)
        {
            //倒计时
            StartCoroutine(ShowCountDown());
        }
        else
        {
            //没有倒计时 直接结束
            EventHandler.Instance.TriggerEvent(EventsInfo.MiniGame_GamePreCountDownEnd);
        }
    }

    public IEnumerator ShowCountDown()
    {
        //回调
        EventHandler.Instance.TriggerEvent(EventsInfo.MiniGame_GamePreCountDownStart);
        //恢复控制
        GameControlHandler.Instance.RestoreControl();

        tvCountDown.text = "";
        for (int i = 0; i <= 3; i++)
        {
            string numberStr = "";
            if (i == 0)
                numberStr = TextHandler.Instance.manager.GetTextById(252);
            else if (i == 1)
                numberStr = TextHandler.Instance.manager.GetTextById(253);
            else if (i == 2)
                numberStr = TextHandler.Instance.manager.GetTextById(254);
            else if (i == 3)
                numberStr = TextHandler.Instance.manager.GetTextById(255);
            tvCountDown.text = numberStr;
            tvCountDown.transform.localScale = new Vector3(1, 1, 1);
            tvCountDown.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f).From().SetEase(Ease.OutBack);
            if (i == 3)
            {
                AudioHandler.Instance.PlaySound(AudioSoundEnum.CountDownEnd);
            }
            else
            {
                AudioHandler.Instance.PlaySound(AudioSoundEnum.CountDownStart);
            }
            yield return new WaitForSeconds(0.8f);
        }
        EventHandler.Instance.TriggerEvent(EventsInfo.MiniGame_GamePreCountDownEnd);
    }
}
