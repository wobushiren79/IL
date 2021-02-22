using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class TextSearchView : BaseMonoBehaviour
{
    public InputField etText;
    public Button btSubmit;

    public ICallBack callBack;


    public void Start()
    {
        if (btSubmit)
            btSubmit.onClick.AddListener(OnClickForSubmit);
    }
    public void SetCallBack(ICallBack callBack)
    {
        this.callBack = callBack;
    }

    public void OnClickForSubmit()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        if (callBack!=null)
        {
            callBack.SearchTextStart(etText.text);
        }
    }

    public interface ICallBack
    {
        void SearchTextStart(string texts);
    }

}