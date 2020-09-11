﻿using UnityEngine;
using UnityEditor;

public class CharacterMoodCpt : BaseMonoBehaviour
{
    public CharacterStatusIconCpt characterStatusIcon;

    private Sprite spCurrent;

    protected IconDataManager iconDataManager;


    private void Awake()
    {
        iconDataManager = Find<IconDataManager>(ImportantTypeEnum.UIManager);
    }
    public void CloseMood()
    {
        characterStatusIcon.RemoveStatusIconByType(CharacterStatusIconEnum.Mood);
    }

    public void SetMood(PraiseTypeEnum mood)
    {
        string spKey = "";
        switch (mood)
        {
            case PraiseTypeEnum.Excited:
                spKey = "customer_mood_0";
                break;
            case PraiseTypeEnum.Happy:
                spKey = "customer_mood_1";
                break;
            case PraiseTypeEnum.Okay:
                spKey = "customer_mood_2";
                break;
            case PraiseTypeEnum.Ordinary:
                spKey = "customer_mood_3";
                break;
            case PraiseTypeEnum.Disappointed:
                spKey = "customer_mood_4";
                break;
            case PraiseTypeEnum.Anger:
                spKey = "customer_mood_5";
                break;
        }
        //避免实时更新带来的多次调用
        if (spCurrent == null ||!spCurrent.name.Contains(spKey))
        {
            Sprite spIcon = iconDataManager.GetIconSpriteByName(spKey);
            CharacterStatusIconBean statusIconData = new CharacterStatusIconBean();
            statusIconData.iconStatus = CharacterStatusIconEnum.Mood;
            statusIconData.spIcon = spIcon;
            characterStatusIcon.ChangeStatusIcon(statusIconData);
            spCurrent = spIcon;
        }
    }

    /// <summary>
    /// 获取当前表情
    /// </summary>
    public Sprite GetCurrentMoodSprite()
    {
        return spCurrent;
    }
}