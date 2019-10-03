using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;

public class ItemTownGuildImproveCharacterCpt : BaseMonoBehaviour
{
    [Header("控件")]
    public CharacterUICpt characterUICpt;
    public Image ivWorker;
    public Text tvName;

    public Text tvLowLevelName;
    public Image ivLowLevelIcon;
    public Text tvHighLevelName;
    public Image ivHighLevelIcon;

    public Image ivMoney;
    public Text tvMoney;
    public Text tvTime;

    [Header("数据")]
    public Sprite spWorkerChef;
    public Sprite spWorkerWaiter;
    public Sprite spWorkerAccounting;
    public Sprite spWorkerAccost;
    public Sprite spWorkerBeater;

    public Sprite spWorkerLevel_1;
    public Sprite spWorkerLevel_2;
    public Sprite spWorkerLevel_3;
    public Sprite spWorkerLevel_4;
    public Sprite spWorkerLevel_5;
    public Sprite spWorkerLevel_6;

    public Sprite spMoneyL;
    public Sprite spMoneyM;
    public Sprite spMoneyS;

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="workerType"></param>
    /// <param name="characterData"></param>
    /// <param name="workerData"></param>
    public void SetData(WorkerEnum workerType, CharacterBean characterData, CharacterWorkerBaseBean workerData, StoreInfoBean levelData)
    {
        SetWorkerIcon(workerType);
        SetName(characterData.baseInfo.name);
        SetCharacter(characterData);
        SetLowLevelData(workerType, workerData.workerLevel);
        SetHighLevelData(workerType, workerData.workerLevel + 1);
        SetMoney(levelData.price_l, levelData.price_m, levelData.price_s);
        SetTime(int.Parse(levelData.mark));
    }

    /// <summary>
    /// 设置角色形象
    /// </summary>
    /// <param name="characterData"></param>
    public void SetCharacter(CharacterBean characterData)
    {
        if (characterUICpt != null)
            characterUICpt.SetCharacterData(characterData.body, characterData.equips);
    }

    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        if (tvName != null)
            tvName.text = name;
    }

    /// <summary>
    /// 设置职业图标
    /// </summary>
    /// <param name="spWorker"></param>
    public void SetWorkerIcon(WorkerEnum workerType)
    {
        Sprite spWorker = null;
        switch (workerType)
        {
            case WorkerEnum.Chef:
                spWorker = spWorkerChef;
                break;
            case WorkerEnum.Waiter:
                spWorker = spWorkerWaiter;
                break;
            case WorkerEnum.Accounting:
                spWorker = spWorkerAccounting;
                break;
            case WorkerEnum.Accost:
                spWorker = spWorkerAccost;
                break;
            case WorkerEnum.Beater:
                spWorker = spWorkerBeater;
                break;
        }
        if (ivWorker != null)
            ivWorker.sprite = spWorker;
    }

    /// <summary>
    /// 设置低等级数据
    /// </summary>
    /// <param name="workerType"></param>
    /// <param name="level"></param>
    /// <param name="spLevel"></param>
    public void SetLowLevelData(WorkerEnum workerType, int level)
    {
        GetLevelData(workerType, level, out string name, out Sprite spLevel);
        if (tvLowLevelName != null)
            tvLowLevelName.text = name;
        if (ivLowLevelIcon != null)
        {
            if (spLevel == null)
                ivLowLevelIcon.color = new Color(0, 0, 0, 0);
            else
                ivLowLevelIcon.sprite = spLevel;
        }
    }

    /// <summary>
    /// 设置高等级数据
    /// </summary>
    /// <param name="workerType"></param>
    /// <param name="level"></param>
    /// <param name="spLevel"></param>
    public void SetHighLevelData(WorkerEnum workerType, int level)
    {
        GetLevelData(workerType, level, out string name, out Sprite spLevel);
        if (tvHighLevelName != null)
            tvHighLevelName.text = name;
        if (ivHighLevelIcon != null)
            if (spLevel == null)
                ivHighLevelIcon.color = new Color(0, 0, 0, 0);
            else
                ivHighLevelIcon.sprite = spLevel;
    }

    /// <summary>
    /// 获取等级数据
    /// </summary>
    /// <param name="workerType"></param>
    /// <param name="level"></param>
    /// <param name="name"></param>
    /// <param name="spLevel"></param>
    private void GetLevelData(WorkerEnum workerType, int level, out string name, out Sprite spLevel)
    {
        string workName = CharacterWorkerBaseBean.GetWorkerName(workerType);
        string workLevelName = CharacterWorkerBaseBean.GetWorkerLevelName(level);
        name = workLevelName + workName;
        spLevel = null;
        switch (level)
        {
            case 0:
                break;
            case 1:
                spLevel = spWorkerLevel_1;
                break;
            case 2:
                spLevel = spWorkerLevel_2;
                break;
            case 3:
                spLevel = spWorkerLevel_3;
                break;
            case 4:
                spLevel = spWorkerLevel_4;
                break;
            case 5:
                spLevel = spWorkerLevel_5;
                break;
            case 6:
                spLevel = spWorkerLevel_6;
                break;
        }
    }

    /// <summary>
    /// 设置金钱
    /// </summary>
    /// <param name="moneyL"></param>
    /// <param name="moneyM"></param>
    /// <param name="moneyS"></param>
    public void SetMoney(long moneyL, long moneyM, long moneyS)
    {
        if (moneyL != 0)
        {
            ivMoney.sprite = spMoneyL;
            tvMoney.text = moneyL + "";
        }
        else if (moneyM != 0)
        {
            ivMoney.sprite = spMoneyM;
            tvMoney.text = moneyM + "";
        }
        else if (moneyS != 0)
        {
            ivMoney.sprite = spMoneyS;
            tvMoney.text = moneyS + "";
        }
    }

    /// <summary>
    /// 设置时间
    /// </summary>
    /// <param name="time"></param>
    public void SetTime(int time)
    {
        if (tvTime != null)
            tvTime.text = time + GameCommonInfo.GetUITextById(37);
    }

}