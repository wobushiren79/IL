using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MiniGameBirthHandler : BaseMiniGameHandler<MiniGameBirthBuilder, MiniGameBirthBean>, DialogView.IDialogCallBack
{

    public List<MiniGameBirthSpermBean> listSperm = new List<MiniGameBirthSpermBean>();

    protected override void Awake()
    {
        builderName = "MiniGameBirthBuilder";
        base.Awake();
    }

    public override void InitGame(MiniGameBirthBean miniGameData)
    {
        listSperm.Clear();
        base.InitGame(miniGameData);
        //打开倒计时UI
        OpenCountDownUI(miniGameData, false);
    }

    public override void StartGame()
    {
        base.StartGame();
        UIHandler.Instance.manager.OpenUIAndCloseOther<UIMiniGameBirth>(UIEnum.MiniGameBirth);
    }

    public override void EndGame(MiniGameResultEnum gameResult, bool isSlow)
    {
        //每日限制减少
        GameCommonInfo.DailyLimitData.numberForBirth--;
        listSperm.Clear();
        //检测是否达到生孩子标准
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        FamilyDataBean familyData = gameData.GetFamilyData();

        //如果可以生孩子
        if (familyData.birthPro >= 1)
        {
            familyData.birthPro = 0;
            if (familyData.listChildCharacter.Count >= 3)
            {
                ToastHandler.Instance.ToastHint(TextHandler.Instance.manager.GetTextById(1351));
            }
            else
            {
                if (familyData.mateCharacter.body.GetSex() == gameData.userCharacter.body.GetSex())
                {
                    //同性
                    ToastHandler.Instance.ToastHint(TextHandler.Instance.manager.GetTextById(7032), 10);
                }
                else
                {
                    //异性
                    ToastHandler.Instance.ToastHint(TextHandler.Instance.manager.GetTextById(7031), 10);
                }

                DialogBean dialogData = new DialogBean();
                dialogData.title = TextHandler.Instance.manager.GetTextById(8011);
                DialogHandler.Instance.CreateDialog<InputTextDialogView>(DialogEnum.InputText, this, dialogData);
                //设置游戏数据
                miniGameData.SetGameResult(MiniGameResultEnum.Win);
                return;
            }
        }
        base.EndGame(gameResult, isSlow);
    }

    public override void GamePreCountDownEnd()
    {
        base.GamePreCountDownEnd();
        StartGame();
    }

    /// <summary>
    /// 到达卵子
    /// </summary>
    public void ArriveEgg(MiniGameBirthSpermBean spermData)
    {
        //获取家族数据
        GameDataBean gameData = GameDataHandler.Instance.manager.GetGameData();
        FamilyDataBean familyData = gameData.GetFamilyData();
        //增加怀孕进度
        familyData.addBirthPro(miniGameData.addBirthPro);
        DestroySperm(spermData);
        //刷新UI
        BaseUIComponent ui = UIHandler.Instance.manager.GetOpenUI();
        ui.RefreshUI();
    }

    /// <summary>
    /// 删除精子
    /// </summary>
    /// <param name="spermData"></param>
    public void DestroySperm(MiniGameBirthSpermBean spermData)
    {
        if (listSperm.Contains(spermData))
        {
            listSperm.Remove(spermData);
        }
        CheckGameOver();
    }

    /// <summary>
    /// 发射精子
    /// </summary>
    public bool FireSperm(out MiniGameBirthSpermBean spermData)
    {
        CheckGameOver();
        spermData = null;
        //没有次数了
        if (miniGameData.fireNumber <= 0)
            return false;
        spermData = new MiniGameBirthSpermBean
        {
            timeForSpeed = miniGameData.playSpeed,
        };
        listSperm.Add(spermData);
        //减少次数
        miniGameData.fireNumber--;
        return true;
    }

    /// <summary>
    /// 检测游戏是否结束
    /// </summary>
    /// <returns></returns>
    public bool CheckGameOver()
    {
        if (miniGameData.gameResult != MiniGameResultEnum.Win && miniGameData.fireNumber <= 0 && listSperm.Count <= 0)
        {
            EndGame(MiniGameResultEnum.Win, false);
            return true;
        }
        return false;
    }


    #region 弹窗检测回调
    public void Submit(DialogView dialogView, DialogBean dialogBean)
    {
        InputTextDialogView inputTextDialog = dialogView as InputTextDialogView;
        AudioHandler.Instance.PlaySound(AudioSoundEnum.Reward);
        RewardTypeBean rewardTypeData = new RewardTypeBean();
        rewardTypeData.dataType = RewardTypeEnum.AddChild;
        rewardTypeData.data = inputTextDialog.GetText();
        miniGameData.listReward.Add(rewardTypeData);
        base.EndGame(MiniGameResultEnum.Win, false);
    }

    public void Cancel(DialogView dialogView, DialogBean dialogBean)
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.Error);
        base.EndGame(MiniGameResultEnum.Win, false);
    }
    #endregion
}