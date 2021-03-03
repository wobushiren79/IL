using UnityEngine;
using UnityEditor;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class DialogView : BaseMonoBehaviour
{
    public GameObject objDialog;

    public Button btSubmit;
    public Text tvSubmit;

    public Button btCancel;
    public Text tvCancel;

    public Button btBackground;
    public Text tvTitle;
    public Text tvContent;
    public CanvasGroup cgDialog;

    protected IDialogCallBack callBack;

    protected Action<DialogView, DialogBean> actionSubmit;
    protected Action<DialogView, DialogBean> actionCancel;

    public DialogBean dialogData;

    protected float timeDelayDelete;

    protected bool isSubmitDestroy = true;

    public virtual void Awake()
    {
        AutoLinkUI();
    }

    public virtual void Start()
    {
        InitData();
    }

    public virtual void OnEnable()
    {
        if (cgDialog != null)
            cgDialog.DOFade(1, 0.5f).SetUpdate(true);
        if (objDialog != null)
            objDialog.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBack).From().SetUpdate(true);
    }

    public virtual void OnDestroy()
    {
        DialogHandler.Instance.manager.RemoveDialog(this);
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


    public void SetSubmitDestroy(bool isSubmitDestroy)
    {
        this.isSubmitDestroy = isSubmitDestroy;
    }

    public virtual void SubmitOnClick()
    {
        callBack?.Submit(this, dialogData);
        actionSubmit?.Invoke(this, dialogData);
        if (isSubmitDestroy)
        {
            DestroyDialog();
        }
    }
    public virtual void CancelOnClick()
    {
        callBack?.Cancel(this, dialogData);
        actionCancel?.Invoke(this, dialogData);
        DestroyDialog();
    }

    public virtual void DestroyDialog()
    {
        if (timeDelayDelete != 0)
        {
            transform.DOScale(new Vector3(1, 1, 1), timeDelayDelete).OnComplete(delegate () { Destroy(gameObject); });
        }
        else
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    public void SetCallBack(IDialogCallBack callBack)
    {
        this.callBack = callBack;
    }

    public void SetAction(Action<DialogView, DialogBean> actionSubmit, Action<DialogView, DialogBean> actionCancel)
    {
        this.actionSubmit = actionSubmit;
        this.actionCancel = actionCancel;
    }

    public void SetData(DialogBean dialogData)
    {
        if (dialogData == null)
            return;
        this.dialogData = dialogData;

        if (dialogData.title != null)
        {
            SetTitle(dialogData.title);
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
    public void SetTitle(string title)
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
        if (tvSubmit != null)
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
        this.timeDelayDelete = delayTime;
    }

    public interface IDialogCallBack
    {
        void Submit(DialogView dialogView, DialogBean dialogBean);
        void Cancel(DialogView dialogView, DialogBean dialogBean);
    }
}