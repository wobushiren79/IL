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

    public virtual void Start()
    {
        InitData();
    }

    public void OnEnable()
    {
        if (cgDialog != null)
            cgDialog.DOFade(1, 0.5f);
    }

    public virtual void InitData()
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

    public virtual void SubmitOnClick()
    {
        if (mCallBack != null)
        {
            mCallBack.Submit(this, dialogData);
        }
        DestroyDialog();
    }
    public virtual void CancelOnClick()
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
            SetTitile(dialogData.title);
        }
        if (dialogData.content != null)
        {
            SetContent(dialogData.content);
        }
        if (dialogData.submitStr != null)
        {
            SetSubmitStr(dialogData.submitStr);
        }
        if (dialogData.cancelStr != null)
        {
            SetCancelStr(dialogData.cancelStr);
        }
    }

    /// <summary>
    /// 设置标题
    /// </summary>
    /// <param name="title"></param>
    public void SetTitile(string title)
    {
        if (tvTitle != null)
        {
            tvTitle.text = title;
        }
    }

    /// <summary>
    /// 设置内容
    /// </summary>
    /// <param name="content"></param>
    public void SetContent(string content)
    {
        if (tvContent != null)
        {
            tvContent.text = content;
        }
    }

    /// <summary>
    /// 设置提交按钮问题
    /// </summary>
    /// <param name="str"></param>
    public void SetSubmitStr(string str)
    {
        if (tvSubmit!=null)
        {
            tvSubmit.text = str;
        }
    }

    /// <summary>
    /// 设置取消按钮文字
    /// </summary>
    /// <param name="str"></param>
    public void SetCancelStr(string str)
    {
        if (tvCancel != null)
        {
            tvCancel.text = str;
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