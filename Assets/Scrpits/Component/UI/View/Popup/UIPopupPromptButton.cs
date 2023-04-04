using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using System.Collections;
public class UIPopupPromptButton : PopupButtonView<UIPopupPromptShow>
{
    // 内容
    public string content;
    //是否播放音效
    public bool isAudio = false;
    //延迟时间
    public float delayTime = 1;

    public void SetContent(string content)
    {
        this.content = content;
    }

    /// <summary>
    /// 协程,延迟展示
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoroutineForShow()
    {
        yield return new WaitForSeconds(delayTime);
        popupShow.gameObject.SetActive(true);
    }

    public override void PopupShow()
    {
        if (popupShow != null)
        {
            //设置内容
            popupShow.SetContent(content);
            //设置音效
            AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForHighLight);

            popupShow.gameObject.SetActive(false);
            StartCoroutine(CoroutineForShow());
        }
        else
            LogUtil.Log("popupShow is null");
    }

    public override void PopupHide()
    {

    }
}