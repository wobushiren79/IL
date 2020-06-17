using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using static ControlHandler;

public class ItemGameBuildCpt : ItemGameBaseCpt
{
    [Header("控件")]
    public Button btBuild;
    public Image ivIcon;
    public Text tvName;
    public Text tvNumber;
    public Text tvAesthetics;

    [Header("数据")]
    public ItemBean itemData;
    public BuildItemBean buildData;

    private void Awake()
    {
        btBuild = GetComponent<Button>();
    }

    private void Start()
    {
        if (btBuild != null)
            btBuild.onClick.AddListener(StartBuild);
    }

    public void RefreshUI()
    {
        SetNumber(itemData.itemNumber);
    }

    public void SetData(ItemBean itemData, BuildItemBean buildData)
    {
        this.itemData = itemData;
        this.buildData = buildData;
        SetName(buildData.name);
        SetNumber(itemData.itemNumber);
        SetAesthetics(buildData.aesthetics);
        SetIcon(buildData);
    }

    /// <summary>
    /// 设置头像
    /// </summary>
    /// <param name="buildItemType"></param>
    /// <param name="iconKey"></param>
    public void SetIcon(BuildItemBean buildData)
    {
        Sprite spIcon = null;
        InnBuildManager innBuildManager = GetUIManager<UIGameManager>().innBuildManager;
        spIcon = BuildItemTypeEnumTools.GetBuildItemSprite(innBuildManager, buildData); 
        if (ivIcon != null)
            ivIcon.sprite = spIcon;
    }

    /// <summary>
    /// 设置数量
    /// </summary>
    /// <param name="number"></param>
    public void SetNumber(long number)
    {
        if (tvNumber != null)
            tvNumber.text = "x " + number;
    }

    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        if (tvName != null)
            tvName.text = name;
    }

    /// <summary>
    /// 设置美观值
    /// </summary>
    /// <param name="aesthetics"></param>
    public void SetAesthetics(float aesthetics)
    {
        if (tvAesthetics != null)
            tvAesthetics.text = "+ " + aesthetics;
    }

    /// <summary>
    /// 开始建造
    /// </summary>
    public void StartBuild()
    {
        ControlForBuildCpt controlForBuild = (ControlForBuildCpt)GetUIManager<UIGameManager>().controlHandler.GetControl(ControlEnum.Build);
        controlForBuild.ShowBuildItem(buildData.id);
    }

}