using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ItemMiniGameDebateCardCpt : ItemGameBaseCpt, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public enum DebateCardTypeEnun
    {
        Rock = 1,//石头
        Scissors = 2,//剪刀
        Paper = 3,//布
    }

    [Header("控件")]
    public Image ivIcon;
    public Text tvName;

    [Header("数据")]
    public Sprite spRock;
    public Sprite spPaper;
    public Sprite spScissors;

    public DebateCardTypeEnun debateCardType;
    public int ownType;//拥有着类型。1 玩家 2敌人
    public bool isOpenPointer = false;


    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="cardType"></param>
    public void SetData(DebateCardTypeEnun cardType, int ownType)
    {
        this.debateCardType = cardType;
        this.ownType = ownType;
        Sprite spIcon = null;
        string name = "???";
        switch (cardType)
        {
            case DebateCardTypeEnun.Rock:
                spIcon = spRock;
                name = GameCommonInfo.GetUITextById(261);
                break;
            case DebateCardTypeEnun.Paper:
                spIcon = spPaper;
                name = GameCommonInfo.GetUITextById(262);
                break;
            case DebateCardTypeEnun.Scissors:
                spIcon = spScissors;
                name = GameCommonInfo.GetUITextById(263);
                break;
        }
        SetIcon(spIcon);
        SetName(name);

    }

    public void OpenPointerListener()
    {
        isOpenPointer = true;
    }
    public void ClosePointerListener()
    {
        isOpenPointer = false;
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="spIcon"></param>
    public void SetIcon(Sprite spIcon)
    {
        if (ivIcon != null)
        {
            ivIcon.sprite = spIcon;
        }
    }

    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        if (tvName != null)
        {
            tvName.text = name;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isOpenPointer)
            return;
        transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isOpenPointer)
            return;
        transform.DOKill();
        transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isOpenPointer)
            return;
        if (ownType == 1)
        {
            AudioHandler.Instance.PlaySound(AudioSoundEnum.SetCard);
            transform.DOKill();
            transform.localScale = new Vector3(1, 1, 1);
            MiniGameHandler.Instance.handlerForDebate.StartCombat(this);
        }
    }
}