using UnityEngine;
using UnityEditor;
using DG.Tweening;
public class CharacterShoutCpt : BaseMonoBehaviour
{
    public GameObject shoutObj;
    public TextMesh tvShout;
    
    public void Shout(string content)
    {
        if (shoutObj == null || tvShout == null)
            return;
        tvShout.text = content;
        shoutObj.SetActive(true);
        shoutObj.transform.localScale = new Vector3(1,1,1);
        shoutObj.transform.DOKill();
        shoutObj.transform.DOScale(new Vector3(0,0,0),0.5f).SetEase(Ease.OutBack).From();
        shoutObj.transform.DOScale(new Vector3(0,0,0),0.5f).SetDelay(3).OnComplete(delegate() {
            shoutObj.SetActive(false);
        });
    }
}