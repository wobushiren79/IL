using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;

public class ItemGameWorkerDetailsWorkerCpt : BaseMonoBehaviour
{
    public Text tvLevelName;
    public Image ivLevel;

    public ProgressView pvLevel;

    public Sprite spSliderMax;
    public Sprite spSliderNormal;

    public Sprite spLevel_1;
    public Sprite spLevel_2;
    public Sprite spLevel_3;
    public Sprite spLevel_4;
    public Sprite spLevel_5;
    public Sprite spLevel_6;

    public Color colorLevel_1;
    public Color colorLevel_2;

    public void SetData(WorkerEnum workerType, CharacterWorkerBaseBean workInfo)
    {
        if (workInfo == null)
            return;
        //设置等级名称
        string workerLevelName =
            CharacterWorkerBaseBean.GetWorkerLevelName(workInfo.workerLevel) + CharacterWorkerBaseBean.GetWorkerName(workerType);
        SetLevelName(workerLevelName);
        //设置经验条
        workInfo.GetWorkerExp(out long nextLevelExp, out long currentExp, out float levelProportion);
        SetExp(workInfo.workerLevel,currentExp, nextLevelExp, levelProportion);
        // 设置等级图标
        SetLevelIcon(workInfo.workerLevel);
    }

    /// <summary>
    /// 设置等级名字
    /// </summary>
    /// <param name="name"></param>
    public void SetLevelName(string name)
    {
        if (tvLevelName != null)
            tvLevelName.text = name;
    }

    /// <summary>
    /// 设置等级图标
    /// </summary>
    /// <param name="level"></param>
    public void SetLevelIcon(int level)
    {
        if (ivLevel == null)
            return;
        ivLevel.color = new Color(1, 1, 1, 1);
        switch (level)
        {
            case 1:
                ivLevel.sprite = spLevel_1;
                break;
            case 2:
                ivLevel.sprite = spLevel_2;
                break;
            case 3:
                ivLevel.sprite = spLevel_3;
                break;
            case 4:
                ivLevel.sprite = spLevel_4;
                break;
            case 5:
                ivLevel.sprite = spLevel_5;
                break;
            case 6:
                ivLevel.sprite = spLevel_6;
                break;
            default:
                ivLevel.color = new Color(1, 1, 1, 0);
                break;
        }
    }

    /// <summary>
    /// 设置经验条
    /// </summary>
    /// <param name="exp"></param>
    public void SetExp( int level,long expCurrent, long expLevelUp, float exp)
    {
        if (level == 6)
        {
            pvLevel.SetData(1, 1);
            pvLevel.SetContent("Max", colorLevel_2);
            return;
        }
        pvLevel.SetData(expLevelUp,expCurrent);
        string levelStr = "";
        if (expCurrent >= expLevelUp)
        {
            levelStr = TextHandler.Instance.manager.GetTextById(75);
            pvLevel.SetContent(levelStr, colorLevel_1);
        }
        else
        {
            pvLevel.SetContentColor(colorLevel_2);
        }
    }


}