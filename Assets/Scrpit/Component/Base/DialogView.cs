using UnityEngine;
using UnityEditor;
using DG.Tweening;
using UnityEngine.UI;

public class DialogView : BaseMonoBehaviour
{
    public Button btSubmit;
    public Text tvSubmit;

    public Button btCancel;
    public Text tvCancel;

    public Button btBackground;
    public Text tvTitle;
    public Text tvContent;
    public CanvasGroup cgDialog;
    private IDialogCallBack mCallBack;

    private void Start()
    {
        InitData();
    }

    private void OnEnable()
    {
        if(cgDialog!=null)
        cgDialog.DOFade(1,0.5f);
    }

    public void InitData()
    {
     
        if (btSubmit != null)
        {
            btSubmit.onClick.RemoveAllListeners();
            btSubmit.onClick.AddListener(SubmitOnClick);
        }
        if (btCancel!=null)
        {
            btCancel.onClick.RemoveAllListeners();
            btCancel.onClick.AddListener(CancelOnClick);
        }
        if (btBackground != null)
        {
            btBackground.onClick.RemoveAllListeners();
            btBackground.onClick.AddListener(CancelOnClick);
        }
    }

    public void SubmitOnClick()
    {
        if (mCallBack != null)
        {
            mCallBack.Submit(this);
        }
        Destroy(gameObject);
    }
    public void CancelOnClick()
    {
        if (mCallBack != null)
        {
            mCallBack.Cancel(this);
        }
        Destroy(gameObject);
    }

    public void SetCallBack(IDialogCallBack callBack)
    {
        this.mCallBack = callBack;
    }

    public void SetData(DialogBean dialogBean)
    {
        if (dialogBean == null)
            return;
        if (dialogBean.title != null)
        {
            tvTitle.text = dialogBean.title;
        }
        if (dialogBean.content != null)
        {
            tvContent.text = dialogBean.content;
        }
        if(dialogBean.submitStr!=null)
        {
            tvSubmit.text = dialogBean.submitStr;
        }
        if (dialogBean.cancelStr != null)
        {
            tvCancel.text = dialogBean.cancelStr;
        }
    }

    public interface IDialogCallBack
    {
         void Submit(DialogView dialogView);
         void Cancel(DialogView dialogView);
    }
}