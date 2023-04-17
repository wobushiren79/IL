using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;

public class UIGameTextForBehind : BaseUIView
{
    public Text tvBehind;

    public void SetData(TextInfoBean textInfo)
    {
        if (tvBehind != null)
        {
            UIGameText uiGameText = UIHandler.Instance.GetUI<UIGameText>();
            tvBehind.text = uiGameText.SetContentDetails(textInfo.content);
            tvBehind.DOFade(0, textInfo.wait_time).From().OnComplete(delegate
           {
               if (gameObject != null)
               {
                   AddReward(textInfo.reward_data);
                   uiGameText.NextText();
               }             
           });
        }
    }
    /// <summary>
    /// 增加奖励
    /// </summary>
    /// <param name="reward"></param>
    public void AddReward(string reward)
    {
        if (reward.IsNull())
            return;
        RewardTypeEnumTools.CompleteReward(null, reward);
    }
}