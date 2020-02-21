using UnityEngine;
using UnityEditor;

public class CharacterMoodCpt : BaseMonoBehaviour
{
    public CharacterStatusIconCpt characterStatusIcon;

    public Sprite spAnger;
    public Sprite spDisappointed;
    public Sprite spOrdinary;
    public Sprite spOkay;
    public Sprite spHappy;
    public Sprite spExcited;

    private Sprite spCurrent;

    public void CloseMood()
    {
        characterStatusIcon.RemoveStatusIconByType(CharacterStatusIconEnum.Mood);
    }

    public void SetMood(PraiseTypeEnum mood)
    {
        Sprite spIcon = null;
        switch (mood)
        {
            case PraiseTypeEnum.Excited:
                spIcon = spExcited;
                break;
            case PraiseTypeEnum.Happy:
                spIcon = spHappy;
                break;
            case PraiseTypeEnum.Okay:
                spIcon = spOkay;
                break;
            case PraiseTypeEnum.Ordinary:
                spIcon = spOrdinary;
                break;
            case PraiseTypeEnum.Disappointed:
                spIcon = spDisappointed;
                break;
            case PraiseTypeEnum.Anger:
                spIcon = spAnger;
                break;
        }
        //避免实时更新带来的多次调用
        if (spCurrent!= spIcon)
        {
            CharacterStatusIconBean statusIconData = new CharacterStatusIconBean();
            statusIconData.iconStatus = CharacterStatusIconEnum.Mood;
            statusIconData.spIcon = spIcon;
            characterStatusIcon.ChangeStatusIcon(statusIconData);
        }
        spCurrent = spIcon;
    }

    /// <summary>
    /// 获取当前表情
    /// </summary>
    public Sprite GetCurrentMoodSprite()
    {
        return spCurrent;
    }
}