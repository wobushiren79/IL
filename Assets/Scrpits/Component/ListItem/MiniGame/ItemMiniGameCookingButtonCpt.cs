using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
public class ItemMiniGameCookingButtonCpt : ItemGameBaseCpt
{
    public enum MiniGameCookingButtonTypeEnum
    {
        Up,
        Down,
        Left,
        Right,
        One,
        Two,
        Three,
        Four
    }

    public Sprite spUp;
    public Sprite spDown;
    public Sprite spLeft;
    public Sprite spRight;
    public Sprite spOne;
    public Sprite spTwo;
    public Sprite spThree;
    public Sprite spFour;

    public Image ivButton;
    public Image ivError;
    public Image ivBox;

    public MiniGameCookingButtonTypeEnum buttonType;
    public int buttonStatus = 0;
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="type"></param>
    public void SetData(MiniGameCookingButtonTypeEnum type)
    {
        SetButtonType(type);
    }

    /// <summary>
    /// 设置按钮类型
    /// </summary>
    /// <param name="type"></param>
    public void SetButtonType(MiniGameCookingButtonTypeEnum buttonType)
    {
        this.buttonType = buttonType;
        Sprite spButton = null;
        switch (buttonType)
        {
            case MiniGameCookingButtonTypeEnum.Up:
                spButton = spUp;
                break;
            case MiniGameCookingButtonTypeEnum.Down:
                spButton = spDown;
                break;
            case MiniGameCookingButtonTypeEnum.Left:
                spButton = spLeft;
                break;
            case MiniGameCookingButtonTypeEnum.Right:
                spButton = spRight;
                break;
            case MiniGameCookingButtonTypeEnum.One:
                spButton = spOne;
                break;
            case MiniGameCookingButtonTypeEnum.Two:
                spButton = spTwo;
                break;
            case MiniGameCookingButtonTypeEnum.Three:
                spButton = spThree;
                break;
            case MiniGameCookingButtonTypeEnum.Four:
                spButton = spFour;
                break;
        }
        if (ivButton != null)
            ivButton.sprite = spButton;
        SetButtonUnClick();
    }

    /// <summary>
    /// 设置按钮未点击
    /// </summary>
    public void SetButtonUnClick()
    {
        buttonStatus = 0;
        ivButton.color = new Color(1, 1, 1, 0.3f);
        ivError.gameObject.SetActive(false);
    }

    /// <summary>
    /// 设置按钮点击错误
    /// </summary>
    public void SetButtonClickError()
    {
        buttonStatus = 2;
        ivButton.color = new Color(1, 1, 1, 0.3f);
        ivError.gameObject.SetActive(true);
        ivError.transform.DOScale(Vector3.zero, 0.2f).From().SetEase(Ease.OutBack);
    }

    /// <summary>
    ///  设置按钮点击正确
    /// </summary>
    public void SetButtonClickCorrect()
    {
        buttonStatus = 1;
        ivButton.color = new Color(1, 1, 1, 1f);
        ivError.gameObject.SetActive(false);
        ivButton.transform.DOScale(new Vector3(1.5f,1.5f,1.5f), 0.2f).From().SetEase(Ease.OutBack);
    }

    /// <summary>
    /// 设置选中状态
    /// </summary>
    /// <param name="isSelect"></param>
    public void SetSelectedStatus(bool isSelect)
    {
        ivBox.gameObject.SetActive(isSelect);
        if (isSelect)
        {
            ivBox.transform.localScale = new Vector3(1,1,1);
            ivBox.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            ivBox.transform.DOKill();
        }
    }
}