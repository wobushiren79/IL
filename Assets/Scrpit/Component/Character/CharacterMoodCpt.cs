using UnityEngine;
using UnityEditor;

public class CharacterMoodCpt : BaseMonoBehaviour
{
    public SpriteRenderer srMood;

    public Sprite spAnger;
    public Sprite spDisappointed;
    public Sprite spOrdinary;
    public Sprite spOkay;
    public Sprite spHappy;
    public Sprite spExcited;

    public void OpenMood()
    {
        srMood.gameObject.SetActive(true);
    }
    public void CloseMood()
    {
        srMood.gameObject.SetActive(false);
    }

    public void SetMood(float mood)
    {
        if (srMood == null)
            return;
        if (mood <= 0)
        {
            srMood.sprite = spAnger;
        }
        else if (mood > 0 && mood <= 20)
        {
            srMood.sprite = spDisappointed;
        }
        else if (mood > 20 && mood <= 40)
        {
            srMood.sprite = spOrdinary;
        }
        else if (mood > 40 && mood <= 60)
        {
            srMood.sprite = spOkay;
        }
        else if (mood > 60 && mood <= 80)
        {
            srMood.sprite = spHappy;
        }
        else if (mood > 80 && mood <= 100)
        {
            srMood.sprite = spExcited;
        }
    }
}