using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;

public class UIGameTextForBehind : BaseUIChildComponent<UIGameText>
{
    public Text tvBehind;

    public void SetData(TextInfoBean textInfo)
    {
        if (tvBehind != null)
        {
            tvBehind.text = uiComponent.SetContentDetails(textInfo.content);
            tvBehind.DOFade(0, textInfo.wait_time).From().OnComplete(delegate
           {
               AddReward(textInfo.reward_data);
               uiComponent.NextText();
           });
        }
    }
    /// <summary>
    /// 增加奖励
    /// </summary>
    /// <param name="reward"></param>
    public void AddReward(string reward)
    {
        if (CheckUtil.StringIsNull(reward))
            return;
        RewardTypeEnumTools.CompleteReward(null, reward);
    }
}