using UnityEngine;
using UnityEditor;
using DG.Tweening;
using UnityEngine.UI;

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
    private IDialogCallBack mCallBack;

    public DialogBean dialogData;

    protected float timeDelayDelete;
    protected AudioHandler audioHandler;
    protected DialogManager dialogManager;

    protected bool isSubmitDestroy = true;

    public virtual void Awake()
    {
        audioHandler = Find<AudioHandler>(ImportantTypeEnum.AudioHandler);
        dialogManager = Find<DialogManager>(ImportantTypeEnum.DialogManager);
    }

    public virtual void Start()
    {
        InitData();
    }

    public virtual void OnEnable()
    {
        if (cgDialog != null)
            cgDialog.DOFade(1, 0.5f).SetUpdate(true);
        if(objDialog!=null)
            objDialog.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutBack).From().SetUpdate(true);
    }

    public virtual void OnDestroy()
    {
        dialogManager.RemoveDialog(this);
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
        if (audioHandler != null)
            audioHandler.PlaySound(AudioSoundEnum.ButtonForNormal);
        if (mCallBack != null)
        {
            mCallBack.Submit(this, dialogData);
        }
        if (isSubmitDestroy)
        {
            DestroyDialog();
        }
    }
    public virtual void CancelOnClick()
    {
        if (audioHandler != null)
            audioHandler.PlaySound(AudioSoundEnum.ButtonForBack);
        if (mCallBack != null)
        {
            mCallBack.Cancel(this, dialogData);
        }
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
        this.mCallBack = callBack;
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