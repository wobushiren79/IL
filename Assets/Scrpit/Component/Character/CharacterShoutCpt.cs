using UnityEngine;
using UnityEditor;
using DG.Tweening;
public class CharacterShoutCpt : BaseMonoBehaviour
{
    public GameObject shoutObj;
    public GameObject objBackground;
    public TextMesh tvShout;

    public void Shout(string content)
    {
        if (shoutObj == null || tvShout == null)
            return;
        tvShout.text = content;
        tvShout.color = Color.red;

        //调整大小
        if (content.Length <= 4)
        {
            objBackground.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            objBackground.transform.localScale = new Vector3(1 + (0.15f * (content.Length - 4)), 1, 1);
        }

        shoutObj.SetActive(true);
        shoutObj.transform.localScale = new Vector3(2, 2, 2);
        shoutObj.transform.DOKill();
        shoutObj.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBack).From();
        shoutObj.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetDelay(2).OnComplete(delegate ()
        {
            shoutObj.SetActive(false);
        });
    }
}