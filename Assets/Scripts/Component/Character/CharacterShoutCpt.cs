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
        if (shoutObj == null || tvShout == null|| content==null)
            return;
        tvShout.text = content;

        //调整大小
        if (content.Length <= 4)
        {
            objBackground.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            byte[] byte_len = System.Text.Encoding.Default.GetBytes(content);
            objBackground.transform.localScale = new Vector3(1 + (0.04f * (byte_len.Length - 4)), 1, 1);
        }

        shoutObj.SetActive(true);
        shoutObj.transform.localScale = new Vector3(2, 2, 2);
        shoutObj.transform.DOKill();
        shoutObj.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBack).From();
        shoutObj.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetDelay(2).OnComplete(delegate ()
        {
            if(this!=null && shoutObj != null)
                shoutObj.SetActive(false);
        });
    }
}