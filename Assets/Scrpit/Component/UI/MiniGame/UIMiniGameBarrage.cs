using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
public class UIMiniGameBarrage : UIBaseMiniGame
{
    //血条
    public Slider sliderLife;
    //血量
    public Text tvLife;
    //剩余时间  
    public Text tvTime;

    private void Update()
    {
        //更新血量
        if (gameData != null)
        {
            MiniGameBarrageBean barrageData = ((MiniGameBarrageBean)gameData);
            MiniGameCharacterBean userGameData = barrageData.userGameData;
            SetUserLife(userGameData.characterMaxLife, userGameData.characterCurrentLife);
        }
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="barrageData"></param>
    public void SetData(MiniGameBarrageBean barrageData)
    {
        gameData = barrageData;
        MiniGameCharacterBean userGameData = barrageData.userGameData;
        SetUserLife(userGameData.characterMaxLife, userGameData.characterCurrentLife);
    }

    /// <summary>
    /// 设置操作对象生命值
    /// </summary>
    /// <param name="maxLife"></param>
    /// <param name="currentLife"></param>
    public void SetUserLife(int maxLife, int currentLife)
    {
        if (sliderLife != null)
            sliderLife.value = (float)currentLife / (float)maxLife;
        if (tvLife != null)
            tvLife.text = currentLife + "/" + maxLife;
    }

    /// <summary>
    /// 设置时间
    /// </summary>
    /// <param name="time"></param>
    public void SetTime(float time)
    {
        if (tvTime != null)
        {
            tvTime.text = time + "";
            tvTime.transform.localScale = new Vector3(1, 1, 1);
            tvTime.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), 0.5f).From().SetEase(Ease.OutBack);
        }
    }
}