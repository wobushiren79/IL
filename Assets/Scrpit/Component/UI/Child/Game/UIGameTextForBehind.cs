using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;

public class UIGameTextForBehind : BaseUIChildComponent<UIGameText>
{
    public Text tvBehind;

    public void SetData(string content, float time)
    {
        if (tvBehind != null)
        {
            tvBehind.text = uiComponent.SetContentDetails(content);
            tvBehind.DOFade(0, time).From().OnComplete(delegate
           {
               uiComponent.NextText();
           });
        }
    }
}