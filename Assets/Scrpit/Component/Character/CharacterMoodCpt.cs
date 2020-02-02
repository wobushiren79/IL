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

    public void SetMood(float mood)
    {
        Sprite spIcon = null;
        if (mood <= 0)
        {
            spIcon = spAnger;
        }
        else if (mood > 0 && mood <= 20)
        {
            spIcon = spDisappointed;
        }
        else if (mood > 20 && mood <= 40)
        {
            spIcon = spOrdinary;
        }
        else if (mood > 40 && mood <= 60)
        {
            spIcon = spOkay;
        }
        else if (mood > 60 && mood <= 80)
        {
            spIcon = spHappy;
        }
        else if (mood > 80 && mood <= 100)
        {
            spIcon = spExcited;
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
}