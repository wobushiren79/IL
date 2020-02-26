﻿using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using System.Collections;
public class InfoPromptPopupButton : PopupButtonView
{
    // 内容
    public string content;
    //是否播放音效
    public bool isAudio = false;

    protected AudioHandler audioHandler;
    private void Awake()
    {
        audioHandler = Find<AudioHandler>(ImportantTypeEnum.AudioHandler);
    }

    public void SetContent(string content)
    {
        this.content = content;
    }

    public override void OpenPopup()
    {
        if (popupShow != null)
        {
            //设置内容
            ((InfoPromptPopupShow)popupShow).SetContent(content);
            //设置音效
            if (audioHandler != null && isAudio)
                audioHandler.PlaySound(SoundEnum.ButtonForHighLight, 0.1f);
            popupShow.gameObject.SetActive(false);
            StartCoroutine(CoroutineForShow());
        }
        else
            LogUtil.Log("popupShow is null");
    }

    public override void ClosePopup()
    {

    }

    /// <summary>
    /// 协程,延迟展示
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoroutineForShow()
    {
        yield return new WaitForSeconds(1);
        popupShow.gameObject.SetActive(true);
    }
}