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

    public DialogBean dialogData;
    private float mTimeDelayDelete;
    private void Start()
    {
        InitData();
    }

    public void OnEnable()
    {
        if (cgDialog != null)
            cgDialog.DOFade(1, 0.5f);
    }

    public void InitData()
    {

        if (btSubmit != null)
        {
            btSubmit.onClick.RemoveAllListeners();
            btSubmit.onClick.AddListener(SubmitOnClick);
        }
        if (btCancel != null)
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
            mCallBack.Submit(this, dialogData);
        }
        DestroyDialog();

  
    }
    public void CancelOnClick()
    {
        if (mCallBack != null)
        {
            mCallBack.Cancel(this, dialogData);
        }
        DestroyDialog();
    }

    public void DestroyDialog()
    {
        if (mTimeDelayDelete != 0)
        {
            transform.DOScale(new Vector3(1, 1, 1), mTimeDelayDelete).OnComplete(delegate () { Destroy(gameObject); });
        }
        else
            Destroy(gameObject);
    }

    public void SetCallBack(IDialogCallBack callBack)
    {
        this.mCallBack = callBack;
    }

    public void SetData(DialogBean dialogData)
    {
        if (dialogData == null)
            return;
        this.dialogData = dialogData;

        if (dialogData.title != null)
        {
            tvTitle.text = dialogData.title;
        }
        if (dialogData.content != null)
        {
            tvContent.text = dialogData.content;
        }
        if (dialogData.submitStr != null)
        {
            tvSubmit.text = dialogData.submitStr;
        }
        if (dialogData.cancelStr != null)
        {
            tvCancel.text = dialogData.cancelStr;
        }
    }

    /// <summary>
    /// 设置延迟删除
    /// </summary>
    /// <param name="delayTime"></param>
    public void SetDelayDelete(float delayTime)
    {
        this.mTimeDelayDelete = delayTime;
    }

    public interface IDialogCallBack
    {
        void Submit(DialogView dialogView,DialogBean dialogBean);
        void Cancel(DialogView dialogView,DialogBean dialogBean);
    }
}