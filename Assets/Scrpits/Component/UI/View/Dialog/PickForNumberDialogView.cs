using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class PickForNumberDialogView : DialogView
{
    public Image ivIcon;
    public InputField etNumber;
    public Button btAddNumber;
    public Button btSubNumber;

    public long pickNumber;
    public long maxPickNumber;

    public override void Awake()
    {
        base.Awake();
    }
    public override void Start()
    {
        base.Start();
        if (btAddNumber)
        {
            btAddNumber.onClick.AddListener(OnClickAddNumber);
        }
        if (btSubNumber)
        {
            btSubNumber.onClick.AddListener(OnClickSubNumber);
        }
        if (etNumber)
        {
            etNumber.onValueChanged.AddListener(ListenerForChange);
        }
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="maxPickNumber"></param>
    public void SetData(Sprite spIcon,long maxPickNumber)
    {
        this.maxPickNumber = maxPickNumber;
        SetNumber(1);
        SetIcon(spIcon);
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="spIcon"></param>
    public void SetIcon(Sprite spIcon)
    {
        if (ivIcon)
        {
            ivIcon.sprite = spIcon;
        }
    }

    /// <summary>
    /// 设置数量
    /// </summary>
    /// <param name="number"></param>
    public void SetNumber(long number)
    {
        pickNumber = number;
        if (pickNumber > maxPickNumber)
        {
            pickNumber = maxPickNumber;
        }
        else if (pickNumber < 1)
        {
            pickNumber = 1;
        }
        if (etNumber != null)
        {
            etNumber.text = pickNumber + "";
        }
    }

    /// <summary>
    /// 获取数量
    /// </summary>
    /// <returns></returns>
    public long GetPickNumber()
    {
        return pickNumber;
    }

    public void ListenerForChange(string change)
    {
        if (long.TryParse(change, out long value))
        {
            SetNumber(value);
        }
        else
        {
            SetNumber(1);
        }
    }

    public void OnClickAddNumber()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        SetNumber(pickNumber + 1);
    }

    public void OnClickSubNumber()
    {
        AudioHandler.Instance.PlaySound(AudioSoundEnum.ButtonForNormal);
        SetNumber(pickNumber - 1);
    }
}