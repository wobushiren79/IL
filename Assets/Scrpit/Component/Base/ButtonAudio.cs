using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonAudio : BaseMonoBehaviour
{
    public Button button;
    public List<AudioClip> clickClip;
    public float volume = 1;

    private void Start()
    {
        if (button != null)
            button.onClick.AddListener(PlayClip);
    }

    public virtual void PlayClip()
    {
        if (CheckUtil.ListIsNull(clickClip))
            return;
        volume = GameCommonInfo.GameConfig.soundVolume;
        AudioClip audioClip = RandomUtil.GetRandomDataByList(clickClip);
        AudioSource.PlayClipAtPoint(audioClip, button.transform.position, volume);
    }

}